using UnityEngine;

public class WaterSpawner : MonoBehaviour
{
    public GameObject waterParticlePrefab; // reference to the WaterParticle prefab
    public float spawnInterval = 0.5f; // time interval between each particle spawn
    public float particleSpeed = 5f; // speed at which particles move upwards

    private float spawnTimer = 0f; // timer to track time between spawns

    private void Update()
    {
        // increment spawn timer
        spawnTimer += Time.deltaTime;

        // if spawn interval has elapsed, spawn a particle and reset timer
        if (spawnTimer >= spawnInterval)
        {
            SpawnParticle();
            spawnTimer = 0f;
        }
    }

    private void SpawnParticle()
    {
        // instantiate a new WaterParticle prefab at this object's position
        GameObject particle = Instantiate(waterParticlePrefab, transform.position, Quaternion.identity);

        // set particle's initial velocity to move upwards
        Rigidbody2D rb = particle.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.up * particleSpeed;
    }
}
