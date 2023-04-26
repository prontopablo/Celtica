using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float fallSpeedMultiplier = 3f; // Multiplier for the player's fall speed  
    public float maxFallSpeed = 15f;
    public LayerMask groundLayers;

    private Vector2 movementInputValue;
    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
    private float coyoteTime = 0.2f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
    }

    private void OnMovement(InputValue value)
    {
        movementInputValue = value.Get<Vector2>();
        Debug.Log(movementInputValue);
    }
 
    void Update()
    {
            if (IsGrounded())
            {
                coyoteTime = 0.1f; // Reset coyoteTime when player is on the ground
            }
            else
            {
                coyoteTime -= Time.deltaTime; // Subtract coyoteTime every frame player is not on the ground
            }

            // Flip the player sprite depending on horizontal movement
            if (movementInputValue.x < 0)
            {
                transform.localScale = new Vector2(-1f, 1f);
            }
            else if (movementInputValue.x > 0)
            {
                transform.localScale = new Vector2(1f, 1f);
            }

            // Print the player speed
            //float speedInKph = rb.velocity.magnitude * 3.6f; // Convert from m/s to km/h
            //string speedString = speedInKph.ToString("0.00") + " km/h";
            //Debug.Log("Player speed: " + speedString);
    }

    void FixedUpdate()
    {
            MoveLogic();
            // Apply fall speed multiplier
            if (rb.velocity.y < 0)
            {
                //fall faster
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallSpeedMultiplier - 1) * Time.deltaTime;
                //caps max fall speed
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
            }
    }    

    private void OnButtonRegular()
    {
        Debug.Log("Button pressed");
    }
    
    private void OnButtonHold()
    {
        Debug.Log("Button held");
    }

    private void MoveLogic()
    {
        Vector2 horizontalMovement = new Vector2(movementInputValue.x, 0f);
        float horizontalVelocity = horizontalMovement.x * moveSpeed * Time.fixedDeltaTime;

        if (movementInputValue.y > 0.5) // treat up movement as a jump
        {
            if(IsGrounded() || coyoteTime > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }
        else
        {
            rb.velocity = new Vector2(horizontalVelocity, rb.velocity.y); // horizontal movement
        }
    }

    // Check if the player is on the ground
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, groundLayers);
    }
}
