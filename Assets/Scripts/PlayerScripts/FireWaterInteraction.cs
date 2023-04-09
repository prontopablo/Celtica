using UnityEngine;

public class FireWaterInteraction : MonoBehaviour
{
    public GameObject steamPrefab; // The steam particle prefab
    public float steamDuration = 5f; // The duration that the steam particle should last

    private void OnParticleCollision(GameObject other)
    {
        // Check if the other gameobject is a WaterParticle
        if (other.CompareTag("WaterParticle"))
        {
            // Randomly determine if the water particle should be deleted
            if (Random.value < 0.01f)
            {
                // Spawn the steam particle at the position of the water particle
                GameObject steam = Instantiate(steamPrefab, other.transform.position, Quaternion.identity);

                // Delete the water particle game object
                Destroy(other.gameObject);

                // Delete the steam particle after the specified duration
                Destroy(steam, steamDuration);
            }
        }
    }
}