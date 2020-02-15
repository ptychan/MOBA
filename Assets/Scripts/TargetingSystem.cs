using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    List<Health> targetInRange = new List<Health>();
    Health targetHealth;
    Team team;

    private void Start()
    {
        team = GetComponent<Team>();
    }

    public void UpdateTarget()
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

    public Health GetCurrentTarget()
    {
        return targetHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        var otherTeam = other.GetComponentInParent<Team>();
        if (otherTeam == null || otherTeam.faction == team.faction)
            return;

        targetInRange.Add(other.GetComponentInParent<Health>());
        Debug.Log("[TargetingSystem] Adding target: " + other.name);
    }

    private void OnTriggerExit(Collider other)
    {
        var otherHealth = other.GetComponentInParent<Health>();
        targetInRange.Remove(otherHealth);
        Debug.Log("[TargetingSystem] Removing target: " + other.name);

        // If my current target just left, stop targeting it
        if (otherHealth == targetHealth)
        {
            targetHealth = null;
        }
    }
}
