using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100; // The player's starting health
    public int currentHealth;        // The player's current health
    public float damageThreshold = 10f; // The minimum force required to cause damage

    private void Start()
    {
        currentHealth = startingHealth; // Initialize the player's health to the starting health
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Calculate the magnitude of the collision force
        float collisionMagnitude = collision.relativeVelocity.magnitude;

        // If the collision force is sufficient, reduce the player's health
        if (collisionMagnitude >= damageThreshold)
        {
            int damage = Mathf.RoundToInt(collisionMagnitude - damageThreshold); // Calculate the amount of damage based on the excess force
            currentHealth -= damage; // Reduce the player's health by the damage amount

            Debug.Log("Player hit for " + damage + " damage. Current health: " + currentHealth);

            // Check if the player has died
            if (currentHealth <= 0)
            {
                Debug.Log("Player has died.");
                // Do whatever you want to happen when the player dies, such as restarting the level or showing a game over screen
            }
        }
    }
}
