using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;

    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (IsAlive())
        {
            currentHealth -= amount;
            currentHealth = Mathf.Max(currentHealth, 0f);
            Debug.Log("[Health] Lost " + amount + "hp. Current health: " + currentHealth);
        }
    }

    public bool IsAlive()
    {
        return currentHealth > 0f;
    }
}
