using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public GameObject projectile;
    public Transform turretTransform;
    public float fireRate;

    List<Health> targetInRange = new List<Health>();
    Health targetHealth;

    Health health;
    float nextAttackTime;

    private void Start()
    {
        health = GetComponent<Health>();
        nextAttackTime = 0.0f;
    }

    private void Update()
    {
        if (health.IsAlive())
        {
            UpdateTarget();
            Attack();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        targetInRange.Add(other.GetComponent<Health>());
        Debug.Log("[TowerController] Adding target: " + other.name);
    }
    
    private void OnTriggerExit(Collider other)
    {
        var otherHealth = other.GetComponent<Health>();
        targetInRange.Remove(otherHealth);
        Debug.Log("[TowerController] Removing target: " + other.name);

        // If my current target just left, stop targeting it
        if (otherHealth == targetHealth)
        {
            targetHealth = null;
        }
    }

    private void UpdateTarget()
    {
        targetInRange.RemoveAll(x => !x.IsAlive());

        if (!targetHealth || !targetHealth.IsAlive())
        {
            var closestTargetHealth = targetInRange
                .OrderBy(x => (x.transform.position - transform.position).sqrMagnitude)
                .FirstOrDefault();
            targetHealth = closestTargetHealth;
        }
    }

    private void Attack()
    {
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
