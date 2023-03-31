using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthMechanic : MonoBehaviour
{
    public float maxDistance = 10f; // maximum distance at which the enemy can "see" the player
    public float moveSpeed = 5f; // speed at which the enemy moves towards the player
    public LayerMask playerLayer; // layer on which the player is placed
    
    private Transform player; // reference to the player's transform component
    private bool playerSeen; // flag indicating whether the player has been seen

    void Start()
    {
        // find the player object in the scene using its layer
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // cast a ray from the enemy to the player
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.position - transform.position, maxDistance, playerLayer);

        // if the ray hit the player, follow the player
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            if (!playerSeen)
            {
                Debug.Log("Player seen");
                playerSeen = true;
            }
            // move towards the player
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            playerSeen = false;
        }
    }
}

