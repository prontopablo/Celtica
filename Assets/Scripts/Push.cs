using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Push : MonoBehaviour
{
    public float forceMagnitude = 10f;
    public float range = 5f;
    public float angle = 30f;
    public float cooldown = 4f;
    private Rigidbody2D rb;
    private bool canPush = true;
    private float cooldownTimer = 0f;
    public Text cooldownText;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (canPush && Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(PushDelay());
        }

        if (!canPush)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                canPush = true;
                cooldownTimer = 0;
            }
        }

        cooldownText.text = "Push (R): " + cooldownTimer.ToString("F0") + "s";
    }

    private IEnumerator PushDelay()
    {
        canPush = false;
        cooldownTimer = cooldown;
        Bend();
        yield return new WaitForSeconds(cooldown);
    }

    private void Bend()
    {
        Vector2 playerPosition = rb.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - playerPosition).normalized;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(playerPosition, range);

        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();

            if (collider.gameObject != gameObject && rb != null)
            {
                Debug.Log("colliders found");
                Vector2 targetPosition = collider.transform.position;
                Vector2 targetDirection = (targetPosition - playerPosition).normalized;
                float angleDifference = Vector2.Angle(direction, targetDirection);

                // Only apply force to objects in front of the player
                if (angleDifference < angle / 2f)
                {
                    Debug.Log("Object within angle");
                    Vector2 force = direction * forceMagnitude;
                    rb.AddForce(force, ForceMode2D.Impulse);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
