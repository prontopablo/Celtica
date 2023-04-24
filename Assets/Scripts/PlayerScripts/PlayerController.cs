using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float jumpTime = 0.25f; // Time to reach the apex of the jump
    public float variableJumpHeightMultiplier = 0.25f; // How much to multiply jump force by when jump button is released early
    public float fallSpeedMultiplier = 3f; // Multiplier for the player's fall speed  
    public float maxFallSpeed = 15f;
    public LayerMask groundLayers;


    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
    private Vector2 standingColliderSize;
    private Vector2 standingColliderOffset;
    private PlayerControls playerControls;
    private InputAction movement;

    private float horizontalMove = 0f;
    private float coyoteTime = 0.2f;
    private bool isJumping = false;
    private float jumpTimeCounter;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        standingColliderSize = coll.size;
        standingColliderOffset = coll.offset;
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        movement = playerControls.Player.Movement;
        movement.Enable();

        playerControls.Player.Jump.performed += DoJump;
        playerControls.Player.Jump.Enable();
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
        if (IsGrounded() || coyoteTime > 0f)
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void OnDisable()
    {
        movement.Disable();
        playerControls.Player.Jump.Disable();
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

            // Handle player movement
            horizontalMove = movement.ReadValue<Vector2>().x * moveSpeed;

            // Flip the player sprite depending on mouse position
            if (Input.mousePosition.x < Screen.width / 2)
            {
                transform.localScale = new Vector2(-1f, 1f);
            }
            else if ((Input.mousePosition.x >= Screen.width / 2))
            {
                transform.localScale = new Vector2(1f, 1f);
            }

            if (playerControls.Player.Jump.triggered && isJumping)
            {
                if (jumpTimeCounter > 0)
                    {
                        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                        jumpTimeCounter -= Time.deltaTime;
                    }
            }

            if (playerControls.Player.Jump.ReadValue<float>() > 0)
            {
                isJumping = false;
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
            }

            // Print the player speed
            //float speedInKph = rb.velocity.magnitude * 3.6f; // Convert from m/s to km/h
            //string speedString = speedInKph.ToString("0.00") + " km/h";
            //Debug.Log("Player speed: " + speedString);
    }

    void FixedUpdate()
    {
           Debug.Log("Movement values: " + movement.ReadValue<Vector2>());
           
            // Move the player
            rb.velocity = new Vector2(horizontalMove, rb.velocity.y);

            // Handle player jumping
            if (isJumping)
            {
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                isJumping = false;
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
    // Check if the player is on the ground
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, groundLayers);
    }
}
