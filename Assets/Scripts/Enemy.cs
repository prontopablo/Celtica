using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 100f;

    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object colliding with the enemy has enough force to cause damage
        if (collision.relativeVelocity.magnitude > 10f)
        {
            // Calculate the damage based on the force of the collision
            float damage = collision.relativeVelocity.magnitude * 1f;
            Debug.Log("Enemy took damage: " + damage);
            // Reduce the enemy's health by the calculated damage
            currentHealth -= damage;

            // Check if the enemy is dead
            if (currentHealth <= 0f)
            {
                Die();
            }
        }
    }

    void Die()
    {
        Debug.Log("Enemy died");
        // Destroy the enemy object or play death animation
        Destroy(gameObject);
    }
}