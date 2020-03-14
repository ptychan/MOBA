using System.Linq;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float moveSpeed;
    public float damageAmount;

    private Vector3 lastTargetPosition;
    private Health targetHealth;

    private Vector3 GetBoundCenter(Health targetHealth)
    {
        Vector3 center = new Vector3();
        var colliders = targetHealth.GetComponentsInChildren<Collider>().Where(x => !x.isTrigger);
        foreach (var collider in colliders)
        {
            center += collider.bounds.center;
        }
        return center / colliders.Count();
    }

    private void Update()
    {
        if (targetHealth.IsAlive())
        {
            lastTargetPosition = GetBoundCenter(targetHealth);
        }

        Vector3 newPosition = Vector3.MoveTowards(transform.position, lastTargetPosition, moveSpeed * Time.deltaTime);
        transform.position = newPosition;
        if (transform.position == lastTargetPosition)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Health>() == targetHealth)
        {
            targetHealth.TakeDamage(damageAmount);
            Destroy(gameObject);
        }
    }

    public void SetTarget(Health target)
    {
        targetHealth = target;
    }
}
