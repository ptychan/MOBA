using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Tooltip("This should be bigger than Nav Mesh Agent's stopping distance")]
    public float arrivalDistance;
    public float attackRange;
    public float attackRate;

    enum State
    {
        MoveToNextPoint,
        ChaseTarget,
        AttackTarget,
        Dying
    }

    private State currentState = State.MoveToNextPoint;

    private NavMeshAgent agent;
    private TargetingSystem targeting;
    private Health health;
    private Weapon weapon;

    private WaypointPath path;
    private int nextPathIndex = 0;
    private Vector3 waypointOffset;

    float nextAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        targeting = GetComponent<TargetingSystem>();
        health = GetComponent<Health>();
        weapon = GetComponent<Weapon>();
        nextAttackTime = 0.0f;
    }

    void Update()
    {
        switch (currentState)
        {
            case State.MoveToNextPoint:
                UpdateMoveToNextPoint();
                break;
            case State.ChaseTarget:
                UpdateChaseTarget();
                break;
            case State.AttackTarget:
                UpdateAttackTarget();
                break;
            case State.Dying:
                UpdateDying();
                break;
        }
    }

    void UpdateMoveToNextPoint()
    {
        if (CheckDying() || IsTargetFound())
            return;

        Vector3 waypoint = path.GetWaypoint(nextPathIndex) + waypointOffset;
        if (Vector2.Distance(waypoint.XZ(), transform.position.XZ()) <= arrivalDistance)
        {
            nextPathIndex = path.GetNextIndex(nextPathIndex);
        }
        agent.SetDestination(path.GetWaypoint(nextPathIndex) + waypointOffset);   
    }

    void UpdateChaseTarget()
    {
        if (CheckDying() || NoTarget() || IsTargetInRange())
            return;

        var targetHealth = targeting.GetCurrentTarget();
        agent.SetDestination(targetHealth.transform.position);
    }

    void UpdateAttackTarget()
    {
        if (CheckDying() || NoTarget() || IsTargetOutOfRange())
            return;
        
        // Face the target
        var targetHealth = targeting.GetCurrentTarget();
        agent.transform.forward = Vector3.Normalize(targetHealth.transform.position - transform.position);

        if (nextAttackTime < Time.time)
        {
            // Use weapon on target
            weapon.Use(targetHealth);

            // Set next time to fire
            nextAttackTime = Time.time + (1.0f / attackRate);
        }
    }

    void UpdateDying()
    {
        Vector3 position = transform.position;
        position.y = Mathf.Lerp(position.y, position.y - 5.0f, Time.deltaTime);
        transform.position = position;
        if (position.y <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    bool CheckDying()
    {
        if (!health.IsAlive())
        {
            agent.enabled = false;
            currentState = State.Dying;
            return true;
        }
        return false;
    }

    bool NoTarget()
    {
        targeting.UpdateTarget();
        if (targeting.GetCurrentTarget() == null)
        {
            currentState = State.MoveToNextPoint;
            return true;
        }
        return false;
    }

    bool IsTargetFound()
    {
        targeting.UpdateTarget();
        if (targeting.GetCurrentTarget() != null)
        {
            currentState = State.ChaseTarget;
            return true;
        }
        return false;
    }

    bool IsTargetInRange()
    {
        var targetHealth = targeting.GetCurrentTarget();
        if (Vector3.Distance(targetHealth.transform.position, transform.position) < attackRange)
        {
            currentState = State.AttackTarget;
            return true;
        }
        return false;
    }

    bool IsTargetOutOfRange()
    {
        var targetHealth = targeting.GetCurrentTarget();
        if (Vector3.Distance(targetHealth.transform.position, transform.position) > attackRange)
        {
            currentState = State.ChaseTarget;
            return true;
        }
        return false;
    }

    public void SetPath(WaypointPath waypointPath)
    {
        path = waypointPath;
        nextPathIndex = 0;
        waypointOffset = transform.position - path.GetWaypoint(0);
        waypointOffset.y = 0.0f;
    }
}
