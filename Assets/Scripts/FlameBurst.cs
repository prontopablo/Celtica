using UnityEngine;

public class FlameBurst : MonoBehaviour
{
    public ParticleSystem flamePrefab;
    public float duration = 0.5f;

void Update()
{
    if (Input.GetKeyDown(KeyCode.I))
    {
        // Instantiate the flamePrefab particle system
        ParticleSystem flame = Instantiate(flamePrefab, transform.position, Quaternion.identity);

        // Set the flame particle system's rotation to match that of the prefab
        flame.transform.rotation = flamePrefab.transform.rotation;
        Destroy(flame.gameObject, duration);
    }
}

}
