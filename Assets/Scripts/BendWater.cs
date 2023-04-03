using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BendWater : MonoBehaviour
{
    public GameObject waterParticlePrefab;
    public float moveRadius = 2.0f;
    public float moveSpeed = 5.0f;

    private bool isMoving = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            isMoving = true;
            MoveNearbyWaterParticles();
        }
        else if (Input.GetKeyUp(KeyCode.G))
        {
            isMoving = false;
        }

        if (isMoving)
        {
            MoveNearbyWaterParticles();
        }
    }

    void MoveNearbyWaterParticles()
    {
        // Find all WaterParticle instances within the moveRadius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, moveRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("WaterParticle"))
            {
                // Calculate the direction towards the mouse pointer
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 direction = (mousePos - collider.transform.position).normalized;

                // Move the WaterParticle's Rigidbody2D in the direction towards the mouse pointer
                Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
                rb.velocity = direction * moveSpeed;
            }
        }
    }

    // Draw a gizmo in the editor to show the moveRadius
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, moveRadius);
    }
}