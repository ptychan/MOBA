using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float moveSpeed;
    public float damageAmount;

    private Vector3 lastTargetPosition;
    private Health targetHealth;

    private void Update()
    {
        if (targetHealth.IsAlive())
        {
            lastTargetPosition = targetHealth.transform.position;
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
