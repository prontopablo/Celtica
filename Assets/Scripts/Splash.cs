using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour
{
    public GameObject waterPrefab;
    public float waterSpeed = 10f;
    public float waterInterval = 0.1f; // The time interval between each water particle
    public float waterLifetime = 2f; // The lifetime of each water particle
    public float waterSpread = 0.001f; // The amount of spread for each water particle

    private float timer = 0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.B))
        {
            // Update the timer
            timer += Time.deltaTime;

            // If enough time has passed, shoot a new water stream
            if (timer >= waterInterval)
            {
                // Calculate the direction from the water gun to the mouse position
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 direction = (mousePosition - transform.position).normalized;

                // Calculate the spread direction perpendicular to the water stream
                Vector3 spreadDirection = new Vector3(-direction.y, direction.x, 0f);

                // Instantiate multiple water particles with randomized offset between them
                for (int i = 0; i < 3; i++)
                {
                    // Calculate the randomized offset for each water particle
                    float spreadAmount = Random.Range(-waterSpread, waterSpread);
                    Vector3 offset = spreadAmount * spreadDirection;

                    // Create a new water particle with offset from the water gun position
                    GameObject water = Instantiate(waterPrefab, transform.position + offset, Quaternion.identity);

                    // Set the velocity of the water particle to shoot towards the mouse position
                    Rigidbody2D waterRigidbody = water.GetComponent<Rigidbody2D>();
                    waterRigidbody.velocity = (direction + offset.normalized) * waterSpeed;

                    // Destroy the water particle after the specified lifetime
                    Destroy(water, waterLifetime);
                }

                // Reset the timer
                timer = 0f;
            }
        }
        else
        {
            // Reset the timer if the player stops shooting
            timer = 0f;
        }
    }
}

