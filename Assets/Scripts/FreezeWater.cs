using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeWater : MonoBehaviour
{
    public GameObject waterParticlePrefab;
    public float freezeRadius = 2.0f;
    public float freezeDuration = 3.0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            FreezeNearbyWater();
        }
    }

    void FreezeNearbyWater()
    {
        // Find all WaterParticle instances within the freezeRadius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, freezeRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("WaterParticle"))
            {
                // Freeze the WaterParticle's Rigidbody2D
                Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
                rb.constraints = RigidbodyConstraints2D.FreezeAll;

                // Start a coroutine to unfreeze the Rigidbody2D after the freezeDuration
                StartCoroutine(UnfreezeWater(rb, freezeDuration));
            }
        }
    }

    IEnumerator UnfreezeWater(Rigidbody2D rb, float delay)
    {
        yield return new WaitForSeconds(delay);
        rb.constraints = RigidbodyConstraints2D.None;
    }

    // Draw a gizmo in the editor to show the freezeRadius
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, freezeRadius);
    }
}