using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 5f; // The speed at which the enemy moves
    public float range = 10f; // The distance at which the enemy can see the player
    public float shootInterval = 2f; // The time interval between shooting rocks at the player
    public float burstInterval = 1f; // The time interval between movement bursts
    public float burstDuration = 0.5f; // The duration of each movement burst
    public GameObject rockPrefab; // The prefab of the rock object to shoot

    private Transform player; // The player's transform component
    private Vector2 moveDirection; // The direction in which the enemy should move
    private bool canShoot = true; // Flag to indicate if the enemy can shoot
    private bool canMove = true; // Flag to indicate if the enemy can move

    void Start()
    {
        // Find the player's transform component
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Set the initial move direction to a random vector
        moveDirection = Random.insideUnitCircle.normalized;
    }

    void Update()
    {
        // Check if the player is within range
        if (Vector2.Distance(transform.position, player.position) <= range)
        {
            // Point towards the player
            transform.up = player.position - transform.position;

            // Move towards the player
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            // Shoot at the player if enough time has passed
            if (canShoot)
            {
                StartCoroutine(ShootRock());
            }
        }
        else
        {
            // Move in short bursts
            if (canMove)
            {
                StartCoroutine(MoveBurst());
            }

            // Change direction periodically
            if (Random.value < 0.02f)
            {
                moveDirection = Random.insideUnitCircle.normalized;
            }
        }
    }

    IEnumerator ShootRock()
    {
        // Set the canShoot flag to false
        canShoot = false;

        // Instantiate the rock prefab at the enemy's position and rotation
        Instantiate(rockPrefab, transform.position, transform.rotation);

        // Wait for the shoot interval
        yield return new WaitForSeconds(shootInterval);

        // Set the canShoot flag to true
        canShoot = true;
    }

    IEnumerator MoveBurst()
    {
        // Set the canMove flag to false
        canMove = false;

        // Choose a random direction for the burst
        Vector2 burstDirection = Random.insideUnitCircle.normalized;

        // Move in the burst direction for the duration of the burst
        float startTime = Time.time;
        while (Time.time < startTime + burstDuration)
        {
            transform.position += (Vector3)burstDirection * moveSpeed * Time.deltaTime;
            yield return null;
        }

        // Wait for the burst interval before allowing another burst
        yield return new WaitForSeconds(burstInterval);

        // Set the canMove flag to true
        canMove = true;
    }
}
