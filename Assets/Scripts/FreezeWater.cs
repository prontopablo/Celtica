using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FreezeWater : MonoBehaviour
{
    public GameObject waterParticlePrefab;
    public float freezeRadius = 2.0f;
    public float freezeDuration = 3.0f;
    public float cooldown = 4.0f;
    public Text cooldownText;

    private bool canFreeze = true;
    private float cooldownTimer = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (!canFreeze)
        {
            cooldownTimer -= Time.deltaTime;
            cooldownText.text = "Cooldown: " + cooldownTimer.ToString("F1");
            if (cooldownTimer <= 0.0f)
            {
                canFreeze = true;
                cooldownText.text = "Ready to Freeze";
            }
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            FreezeNearbyWater();
            canFreeze = false;
            cooldownTimer = cooldown;
        }
        cooldownText.text = "Freeze (Y): " + cooldownTimer.ToString("F0") + "s";
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

            // Change the color of the WaterParticle
            SpriteRenderer spriteRenderer = collider.GetComponent<SpriteRenderer>();
            spriteRenderer.color = Color.cyan;

            // Start a coroutine to unfreeze the Rigidbody2D after the freezeDuration
            StartCoroutine(UnfreezeWater(rb, spriteRenderer, freezeDuration));
        }
    }
}

    IEnumerator UnfreezeWater(Rigidbody2D rb, SpriteRenderer spriteRenderer, float delay)
    {
        Vector2 initialPosition = rb.position;
        Vector2 initialVelocity = rb.velocity;

        yield return new WaitForSeconds(delay);

        rb.constraints = RigidbodyConstraints2D.None;
        rb.position = initialPosition;
        rb.velocity = initialVelocity;

        spriteRenderer.color = Color.blue; // Change the color back to its original value
    }

    IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        canFreeze = true;
    }

    // Draw a gizmo in the editor to show the freezeRadius
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, freezeRadius);
    }
}