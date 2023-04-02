using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float digDistance = 1f;
    public float crouchHeightModifier = 0.5f;
    public float crouchSpeed = 2f;
    public float jumpTime = 0.25f; // Time to reach the apex of the jump
    public float variableJumpHeightMultiplier = 0.5f; // How much to multiply jump force by when jump button is released early
    public KeyCode crouchKey = KeyCode.C;
    public LayerMask groundLayers;

    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
    private Vector2 standingColliderSize;
    private Vector2 standingColliderOffset;

    private float horizontalMove = 0f;
    private bool isGrounded = false;
    private bool isJumping = false;
    private bool isCrouching = false;
    private float jumpTimeCounter;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        standingColliderSize = coll.size;
        standingColliderOffset = coll.offset;
    }

    void Update()
    {
        // Check if the player is on the ground
        isGrounded = Physics2D.IsTouchingLayers(coll, groundLayers);

        // Handle player movement
        horizontalMove = Input.GetAxisRaw("Horizontal") * (Input.GetKey(crouchKey) ? crouchSpeed : moveSpeed);

        // Flip the player sprite if moving left
        if (horizontalMove < 0f)
        {
            transform.localScale = new Vector2(-1f, 1f);
        }
        else if (horizontalMove > 0f)
        {
            transform.localScale = new Vector2(1f, 1f);
        }

        // Handle player jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

            if (Input.GetButton("Jump") && isJumping)
        {
        if (jumpTimeCounter > 0)
            {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTimeCounter -= Time.deltaTime;
            }
        }

    if (Input.GetButtonUp("Jump"))
    {
        isJumping = false;
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
    }

        // Handle player digging
        if (Input.GetButtonDown("Dig"))
        {
            Debug.Log("Digging!");
            Dig();
        }

        // Handle player crouching
        isCrouching = Input.GetKey(KeyCode.C);

        if (isCrouching)
        {
            coll.size = new Vector2(coll.size.x, standingColliderSize.y * crouchHeightModifier);
            coll.offset = new Vector2(coll.offset.x, standingColliderOffset.y - ((standingColliderSize.y - coll.size.y) / 2f));
        }
        else
        {
            coll.size = standingColliderSize;
            coll.offset = standingColliderOffset;
        }

        // Print the player speed
        //float speedInKph = rb.velocity.magnitude * 3.6f; // Convert from m/s to km/h
        //string speedString = speedInKph.ToString("0.00") + " km/h";
        //Debug.Log("Player speed: " + speedString);
    }

    void FixedUpdate()
    {
        // Move the player
        rb.velocity = new Vector2(horizontalMove, rb.velocity.y);

        // Handle player jumping
        if (isJumping)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isJumping = false;
        }
    }

    void Dig()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, digDistance, groundLayers);

        if (hit.collider != null)
        {
            Destroy(hit.collider.gameObject);
        }
    }
}
