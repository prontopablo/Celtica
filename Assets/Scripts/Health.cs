
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    UIHealthBar healthBar;
    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponentInChildren<UIHealthBar>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        healthBar.SetHealthBarPercentage(currentHealth / maxHealth);
        if(currentHealth <= 0.0f)
        {
            Die();
        }
    }

    private void Die()
    {
        healthBar.gameObject.SetActive(false);
    }
}
