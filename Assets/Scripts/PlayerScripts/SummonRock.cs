using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonRock : MonoBehaviour
{
    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
    private Vector2 standingColliderSize;
    private Vector2 standingColliderOffset;

    public GameObject rockPrefab; // Reference to the rock prefab that will be summoned
    public float summonDelay = 4f; // Delay in seconds between rock summons
    public LayerMask groundLayers; // Layer mask for the ground layer
    public float rockDistance = 1f; // Distance in front of player to summon rock
    public Text cooldownText; // Declare a public Text variable to hold the UI text element
    public float breakForce = 3f; // Force required for the rock to break
    public float rockSpeed = 40f;
    private float lastSummonTime; // Time when the last rock was summoned
    private bool summoningRock = false; // Flag for whether the rock is being summoned
    private float summonStartTime; // Time when the rock summoning started
    private Vector3 summonStartPos; // Starting position of the rock summoning
    private GameObject summonedRock; // Reference to the summoned rock object

    private void Start()
    {
        // Initialize the last summon time to be earlier than the current time by the value of summonDelay
        lastSummonTime = -summonDelay;
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        standingColliderSize = coll.size;
        standingColliderOffset = coll.offset;
    }

    private void Update()
    {
        // Check if the player presses the summon button (in this case, the T key)
        if (Input.GetKeyDown(KeyCode.T))
        {
            // Check if enough time has passed since the last summon
            if (Time.time - lastSummonTime >= summonDelay)
            {
                // Check if the player is touching the ground layer
                if (IsGrounded() && !summoningRock)
                {
                    // Start summoning the rock
                    summoningRock = true;
                    summonStartTime = Time.time;
                    Vector3 playerPos = transform.position;
                    Vector3 summonDirection = transform.up;
                    summonStartPos = playerPos + transform.right * rockDistance * (transform.localScale.x < 0 ? -1 : 1);
                    summonedRock = Instantiate(rockPrefab, summonStartPos, Quaternion.identity);
                    summonedRock.transform.localScale = Vector3.zero;

                    // Update the last summon time
                    lastSummonTime = Time.time;
                }
            }
    }

    // Calculate the remaining cooldown time
    float remainingCooldownTime = Mathf.Max(0f, summonDelay - (Time.time - lastSummonTime));

    // Update the UI text element with the remaining cooldown time
    cooldownText.text = "Rock (T): " + remainingCooldownTime.ToString("F0") + "s";

    // If the rock is being summoned, move it up from the ground and then keep it at the player's eye level for a second
    if (summoningRock)
    {
        float summonProgress = (Time.time - summonStartTime) / 0.5f;
        if (summonProgress < 1f)
        {
            float summonHeight = Mathf.Lerp(0f, 1.5f, summonProgress);
            Vector3 summonPos = summonStartPos + Vector3.up * summonHeight;
            summonedRock.transform.position = summonPos;

            float summonScale = Mathf.Lerp(0f, 1f, summonProgress);
            summonedRock.transform.localScale = new Vector3(summonScale, summonScale, summonScale);
        }
        else
        {
            // Shoot the rock towards the mouse pointer
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 shootDirection = (mousePos - summonedRock.transform.position).normalized;
            summonedRock.GetComponent<Rigidbody2D>().velocity = shootDirection * rockSpeed;

            summonedRock.transform.localScale = Vector3.one;
            summonedRock.SetActive(true);
            summonedRock.transform.position = summonStartPos + Vector3.up * 1.5f;
            summoningRock = false;
        }
    }
}
    // Check if the player is on the ground
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, groundLayers);
    }
}
