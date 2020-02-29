using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class HeroController : NetworkBehaviour
{
    public float attackRange;
    public float attackRate;

    enum Command
    {
        None,
        Move,
        Attack,
        Skill
    }

    struct MoveCommand
    {
        public Vector3 destination;
    }

    struct AttackCommand
    {
        public Health targetHealth;
    }

    enum State
    {
        Idle = 0,
        Move = 1,
        Attack = 2,
        Dying = 3
    }

    private State currentState = State.Idle;

    private NavMeshAgent agent;
    private Animator animator;
    private Health health;
    private Team team;
    private Weapon weapon;

    private Health targetHealth;
    float nextAttackTime;

    MoveCommand moveCommand;
    AttackCommand attackCommand;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        health = GetComponent<Health>();
        team = GetComponent<Team>();
        weapon = GetComponent<Weapon>();
        nextAttackTime = 0.0f;
    }

    void Update()
    {
        Command command = CheckInputCommand();
        switch (command)
        {
            case Command.None:
                break;
            case Command.Move:
                ProcessMoveCommand();
                break;
            case Command.Attack:
                ProcessAttackCommand();
                break;
            case Command.Skill:
                break;
        }

        switch (currentState)
        {
            case State.Idle:    UpdateIdle(); break;
            case State.Move:    UpdateMove(); break;
            case State.Attack:  UpdateAttack(); break;
            case State.Dying:   UpdateDying(); break;
        }
    }

    Command CheckInputCommand()
    {
        // Only control hero that we have authority over
        if (hasAuthority && Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                var hitTeam = hit.collider.GetComponent<Team>();
                if (hitTeam && hitTeam.faction != team.faction)
                {
                    attackCommand.targetHealth = hit.collider.GetComponent<Health>();
                    return Command.Attack;
                }
                else
                {
                    moveCommand.destination = hit.point;
                    return Command.Move;
                }
            }
        }

        return Command.None;
    }

    void ProcessMoveCommand()
    {
        Cmd_Move(moveCommand.destination);
    }

    [Command]
    void Cmd_Move(Vector3 destination)
    {
        Rpc_Move(destination);
    }

    [ClientRpc]
    void Rpc_Move(Vector3 destination)
    {
        currentState = State.Move;
        agent.SetDestination(destination);
    }

    void ProcessAttackCommand()
    {
        if (attackCommand.targetHealth)
        {
            var identity = attackCommand.targetHealth.GetComponent<NetworkIdentity>();
            Cmd_Attack(identity.netId);
        }
    }

    [Command]
    void Cmd_Attack(NetworkInstanceId netId)
    {
        Rpc_Attack(netId);
    }

    [ClientRpc]
    void Rpc_Attack(NetworkInstanceId netId)
    {
        var target = NetworkServer.FindLocalObject(netId);
        if (target == null)
            target = ClientScene.FindLocalObject(netId);

        if (target)
        {
            var netTargetHealth = target.GetComponent<Health>();
            if (netTargetHealth)
            {
                currentState = State.Attack;
                targetHealth = netTargetHealth;
            }
        }
    }

    void UpdateIdle()
    {
    }

    void UpdateMove()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            currentState = State.Idle;
            animator.SetInteger("AnimationState", (int)State.Idle);
        }
        else
        {
            animator.SetInteger("AnimationState", (int)State.Move);
        }
    }

    void UpdateAttack()
    {
        if (targetHealth == null || !targetHealth.IsAlive())
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                currentState = State.Idle;
                animator.SetInteger("AnimationState", (int)State.Idle);
            }
        }
        else if (Vector3.Distance(targetHealth.transform.position, transform.position) > attackRange)
        {
            agent.SetDestination(targetHealth.transform.position);
            animator.SetInteger("AnimationState", (int)State.Move);
        }
        else
        {
            // Face the target
            agent.transform.forward = Vector3.Normalize(targetHealth.transform.position - transform.position);
            agent.SetDestination(transform.position);

            // Make sure we are back in the Idle state first
            var animationState = animator.GetInteger("AnimationState");
            if (animationState != (int)State.Idle && animationState != (int)State.Attack)
            {
                animator.SetInteger("AnimationState", (int)State.Idle);
            }
            // Check attack time
            else if (nextAttackTime < Time.time)
            {
                animator.SetInteger("AnimationState", (int)State.Attack);
                animator.SetTrigger("Attack");

                // Set next time to attack
                nextAttackTime = Time.time + (1.0f / attackRate);
            }
        }
    }

    void UpdateDying()
    {
    }

    public void OnAttack()
    {
        if (targetHealth && targetHealth.IsAlive())
        {
            // Use weapon on target
            weapon.Use(targetHealth);
        }
    }
}
