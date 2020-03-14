using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject projectile;
    public Transform projectileSpawnTransform;
    public float damageAmount;

    public void Use(Health targetHealth)
    {
        if (projectile)
        {
            // Fire projectile at target
            var newProjectile = Instantiate(projectile, projectileSpawnTransform.position, Quaternion.identity);
            var controller = newProjectile.GetComponent<ProjectileController>();
            controller.SetTarget(targetHealth);
            controller.damageAmount = damageAmount;
        }
        else
        {
            targetHealth.TakeDamage(damageAmount);
        }
    }
}
