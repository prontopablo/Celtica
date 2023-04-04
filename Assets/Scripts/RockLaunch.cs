using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockLaunch : MonoBehaviour
{
    public float launchSpeed = 10f;
    public float launchAngle = 45f;
    public KeyCode launchKey = KeyCode.LeftShift;
    public GameObject rockPrefab;
    public float growTime = 1f;

    private Rigidbody2D rb;
    private bool launched = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(launchKey) && !launched)
        {
            Launch();
            launched = true;
        }
    }

    private void Launch()
    {
        Vector2 direction;
        if (transform.right.x > 0) // facing right
        {
            float radians = launchAngle * Mathf.Deg2Rad;
            float x = Mathf.Cos(radians);
            float y = Mathf.Sin(radians);
            direction = new Vector2(x, y).normalized + Vector2.up;
        }
        else // facing left
        {
            float radians = launchAngle * Mathf.Deg2Rad;
            float x = -Mathf.Cos(radians);
            float y = Mathf.Sin(radians);
            direction = new Vector2(x, y).normalized + Vector2.up;
        }

        rb.velocity = direction * launchSpeed;

        // Spawn rock prefab
        Vector3 spawnPosition = transform.position - Vector3.up * 0.5f; // slightly below player's position
        GameObject rock = Instantiate(rockPrefab, spawnPosition, Quaternion.identity);

        // Disable rock rendering and collider at first
        Renderer rockRenderer = rock.GetComponent<Renderer>();
        Collider2D rockCollider = rock.GetComponent<Collider2D>();
        rockRenderer.enabled = false;
        rockCollider.enabled = false;

        // Start growing the rock from the ground up
        StartCoroutine(GrowRock(rock, growTime));
    }

    private IEnumerator GrowRock(GameObject rock, float time)
    {
        // Start growing from the ground up
        float startY = rock.transform.position.y;
        float endY = startY + rock.transform.localScale.y / 2;
        float t = 0f;
        while (t <= 1f)
        {
            t += Time.deltaTime / time;
            float currentY = Mathf.Lerp(startY, endY, t);
            rock.transform.position = new Vector3(rock.transform.position.x, currentY, rock.transform.position.z);
            yield return null;
        }

        // Enable rock rendering and collider once growth is complete
        Renderer rockRenderer = rock.GetComponent<Renderer>();
        Collider2D rockCollider = rock.GetComponent<Collider2D>();
        rockRenderer.enabled = true;
        rockCollider.enabled = true;
    }
}
