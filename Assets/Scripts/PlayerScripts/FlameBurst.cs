using UnityEngine;

public class FlameBurst : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem flameParticle;

    [SerializeField]
    private float maxFireBurstLength = 1.0f;

    [SerializeField]
    private float flameForceMagnitude = 1f;

    private float currentFireBurstLength = 0.0f;

    private Rigidbody2D playerRigidbody;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            flameParticle.transform.position = transform.position;
            flameParticle.Play();
        }
        else if (Input.GetKey(KeyCode.I) && currentFireBurstLength < maxFireBurstLength)
        {
            currentFireBurstLength += Time.deltaTime;
        }
        else if (Input.GetKeyUp(KeyCode.I))
        {
            flameParticle.Stop();
            currentFireBurstLength = 0.0f;
        }
        
        // Get the mouse position in world space
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0.0f;
        
        // Calculate the direction vector from the particle system's position to the mouse position
        Vector3 direction = mousePos - flameParticle.transform.position;
        
        // Calculate the rotation needed to point the particle system in the direction of the mouse pointer
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.back);
        
        // Set the rotation of the particle system to the calculated rotation
        flameParticle.transform.rotation = rotation;

        // Apply a force to the player in the opposite direction to the flame burst
        if (currentFireBurstLength > 0.0f)
        {
            Vector2 forceDirection = -direction.normalized;
            Vector2 force = forceDirection * flameForceMagnitude;
            playerRigidbody.AddForce(force);
        }
    }
}
