using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

//AstarPath.Scan()

public class WifeController : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform target;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.8f;
    public float jumpModifier = 0.3f;
    public float jumpCheckOffset = 0.1f;
    public float fallSpeedMultiplier = 3f; // Multiplier for the player's fall speed  
    public float maxFallSpeed = 15f;
    public LayerMask groundLayers;

    [Header("CustomBehaviour")]
    public bool followEnabled = true;
    public bool jumpEnabled = true;
    public bool directionLookEnabled = true;

    private Path path;
    private int currentWaypoint = 0;
    Seeker seeker;
    Rigidbody2D rb;
    CapsuleCollider2D coll;

    public void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    private void FixedUpdate()
    {
        if (TargetInDistance() && followEnabled)
        {
            PathFollow();
        }

        // Apply fall speed multiplier
        if (rb.velocity.y < 0)
        {
            //fall faster
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallSpeedMultiplier - 1) * Time.deltaTime;
            //caps max fall speed
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
        }
    }

    private void UpdatePath()
    {
        if(followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void PathFollow()
    {
        if (path == null)
        {
            return;
        }

        //Reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        //Direction Calculation
        Vector2 direction  = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        //Jump
        if (jumpEnabled && IsGrounded())
        {
            if (direction.y > jumpNodeHeightRequirement)
            {
                rb.AddForce(Vector2.up * speed * jumpModifier);
            }
        }

        //Movement
        rb.AddForce(force);

        //Next Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }


       //Direction Graphics Handling
        if(directionLookEnabled)
        {
            if(rb.velocity.x > 0.01f)
            {
                transform.localScale = new Vector2(1f, 1f);
            }
            else if (rb.velocity.x < -0.01f) // Changed from > to <
            {
                transform.localScale = new Vector2(-1f, 1f);
            }
        }

    }

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;  
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    // Check if we're on the ground
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, groundLayers);
    }
}