using UnityEngine;
using UnityEngine.UI;

public class SummonWall : MonoBehaviour {
    public GameObject wallPrefab;
    public float wallHeight = 2.0f;
    public float wallDistance = 1.0f;
    public KeyCode summonKey = KeyCode.U;
    public float wallLifetime = 2.0f;
    public Text cooldownText;

    private bool isFacingRight;
    private GameObject currentWall;
    private float cooldownTimer;

    void Start() {
        isFacingRight = transform.localScale.x > 0;
        cooldownTimer = 0;
        UpdateCooldownText();
    }

    void Update() {
        // Update cooldown timer
        if (cooldownTimer > 0) {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer < 0) {
                cooldownTimer = 0;
                UpdateCooldownText();
            } else {
                UpdateCooldownText();
                return; // Don't allow wall summoning during cooldown
            }
        }

        if (Input.GetKeyDown(summonKey) && currentWall == null) {
            Vector3 wallPosition = transform.position;
            wallPosition.y += wallHeight / 2;

            if (isFacingRight) {
                wallPosition.x += wallDistance;
            } else {
                wallPosition.x -= wallDistance;
            }

            currentWall = Instantiate(wallPrefab, wallPosition, Quaternion.identity);
            currentWall.transform.localScale = new Vector3(1, wallHeight, 1);

            Destroy(currentWall, wallLifetime);

            // Start cooldown timer
            cooldownTimer = wallLifetime;
            UpdateCooldownText();
        }

        // Update facing direction
        isFacingRight = transform.localScale.x > 0;
    }

    void UpdateCooldownText() {
        if (cooldownText != null) {
            cooldownText.text = "Wall (U): " + Mathf.Ceil(cooldownTimer) + "s";
        }
    }
}
