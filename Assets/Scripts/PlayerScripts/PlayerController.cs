using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, PlayerInput.IPlayerControlsActions
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float fallSpeedMultiplier = 3f; // Multiplier for the player's fall speed  
    public float maxFallSpeed = 15f;
    public float lowJumpMultiplier = 0.5f; // Multiplier for when the jump button is released early
    public LayerMask groundLayers;

    private Vector2 movementInputValue;
    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
    private float coyoteTime = 0.2f;

    private PlayerInput playerInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        playerInput = new PlayerInput();
    }

    void OnEnable()
    {
        playerInput.PlayerControls.SetCallbacks(this);
        playerInput.PlayerControls.Enable();
    }

    void OnDisable()
    {
        playerInput.PlayerControls.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInputValue = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && (IsGrounded() || coyoteTime > 0f))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        else if (context.canceled)
        {
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * lowJumpMultiplier);
            }
        }
    }

    public void OnAbility1(InputAction.CallbackContext context)
    { 
        // TODO
    }

    public void OnAbility2(InputAction.CallbackContext context)
    {
        // TODO
    }

    public void OnAbility3(InputAction.CallbackContext context)
    {
        // TODO
    }

    public void OnAbility4(InputAction.CallbackContext context)
    {
        // TODO
    }

    public void OnAbility5(InputAction.CallbackContext context)
    {
        // TODO
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

    private void MoveLogic()
    {
        Vector2 horizontalMovement = new Vector2(movementInputValue.x, 0f);
        float horizontalVelocity = horizontalMovement.x * moveSpeed;

        rb.velocity = new Vector2(horizontalVelocity, rb.velocity.y); // horizontal movement
    }

    // Check if the player is on the ground
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, groundLayers);
    }
}
