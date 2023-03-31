using UnityEngine;

public class EnemyRockThrow : MonoBehaviour
{
    public GameObject rockPrefab;   // The prefab for the rock object
    public float shootingInterval;  // The time interval between each rock shot
    public float shootingForce;     // The force with which the rock is shot
    public float shootingDistance;  // The distance from the enemy to shoot the rock from
    public float shootingRange = 10f;
    
    private float shootingTimer;    // The timer for shooting rocks

    private void Update()
    {
        // Update the shooting timer
        shootingTimer -= Time.deltaTime;

        // If the shooting timer has elapsed, shoot a rock towards the player
        if (shootingTimer <= 0)
        {
            // Find the player object
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            // Check if the player object exists and is within range
            if (player != null && Vector2.Distance(transform.position, player.transform.position) < shootingRange)
            {
                Debug.Log("Player found");
                // Calculate the direction towards the player
                Vector2 direction = player.transform.position - (transform.position + transform.right * shootingDistance);

                // Instantiate the rock object and set its position and rotation
                GameObject rock = Instantiate(rockPrefab, transform.position + transform.right * shootingDistance, Quaternion.identity);

                // Apply a force to the rock in the direction towards the player
                rock.GetComponent<Rigidbody2D>().AddForce(direction.normalized * shootingForce, ForceMode2D.Impulse);
            }

            // Reset the shooting timer
            shootingTimer = shootingInterval;
        }
    }
}
