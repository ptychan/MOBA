using UnityEngine;

public class TowerController : MonoBehaviour
{
    public GameObject projectile;
    public Transform turretTransform;
    public float fireRate;

    TargetingSystem targeting;
    Health health;
    float nextAttackTime;

    private void Start()
    {
        targeting = GetComponent<TargetingSystem>();
        health = GetComponent<Health>();
        nextAttackTime = 0.0f;
    }

    private void Update()
    {
        if (health.IsAlive())
        {
            targeting.UpdateTarget();
            Attack();
        }
    }

    private void Attack()
    {
        var targetHealth = targeting.GetCurrentTarget();
        if (targetHealth && nextAttackTime < Time.time)
        {
            // Fire projectile at target
            var newProjectile = Instantiate(projectile, turretTransform);
            newProjectile.GetComponent<ProjectileController>().SetTarget(targetHealth);

            // Set next time to fire
            nextAttackTime = Time.time + (1.0f / fireRate);
        }
    }
}
