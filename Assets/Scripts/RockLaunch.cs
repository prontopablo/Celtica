using UnityEngine;

public class RockLaunch : MonoBehaviour
{
    public float launchSpeed = 10f;
    public float launchAngle = 45f;
    public KeyCode launchKey = KeyCode.LeftShift;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(launchKey))
        {
            Launch();
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
    }
}
