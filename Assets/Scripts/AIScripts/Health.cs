using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public UIHealthBar healthBar;
    public float minimumCollisionForce;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // This method will be called whenever a collision occurs
    void OnCollisionEnter2D(Collision2D collision)
    {
        float collisionForce = collision.relativeVelocity.magnitude;
        if (collisionForce >= minimumCollisionForce)
        {
            float damage = collisionForce * 0.1f; // Adjust this value as needed
            TakeDamage(damage);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        healthBar.SetHealthBarPercentage(currentHealth / maxHealth);
        if (currentHealth <= 0.0f)
        {
            Die();
        }
    }

    private void Die()
    {
        healthBar.gameObject.SetActive(false);
    }
}
