using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damageAmount;

    public void Use(Health targetHealth)
    {
        targetHealth.TakeDamage(damageAmount);
    }
}
