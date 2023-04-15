using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WifeFlameBurst : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem flameParticle;

    [SerializeField]
    private float maxFireBurstLength = 1.0f;

    [SerializeField]
    private float flameForceMagnitude = 1f;

    private Rigidbody2D wifeRigidbody;

    private void Awake()
    {
        wifeRigidbody = GetComponent<Rigidbody2D>();
    }

    public void ShootAtTarget(Transform target, float shootDuration)
    {
        if (flameParticle != null && target != null)
        {
            StartCoroutine(ShootCoroutine(target, shootDuration));
        }
    }

    private IEnumerator ShootCoroutine(Transform target, float shootDuration)
    {
        // Point the particle system towards the target
        Vector3 direction = target.position - flameParticle.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.back);
        flameParticle.transform.rotation = rotation;

        // Start the particle system
        flameParticle.Play();

        // Apply a force to the wife in the opposite direction to the flame burst
        float currentFireBurstLength = 0.0f;
        while (currentFireBurstLength < maxFireBurstLength && shootDuration > 0.0f)
        {
            Vector2 forceDirection = -direction.normalized;
            Vector2 force = forceDirection * flameForceMagnitude;
            wifeRigidbody.AddForce(force);
            currentFireBurstLength += Time.deltaTime;
            shootDuration -= Time.deltaTime;
            yield return null;
        }

        // Stop the particle system and reset the fire burst length
        flameParticle.Stop();
    }
}
