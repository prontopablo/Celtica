using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryObject : MonoBehaviour
{
    public float carryDistance = 1f;
    public LayerMask objectLayers;
    public float pickupRange = 1.5f;
    public float throwForce = 10f;
    
    private Rigidbody2D rb;
    private Collider2D coll;
    private GameObject carriedObject;
    private bool isCarrying = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }
//
void Update()
{
    // Check if the player wants to pick up an object
    if (Input.GetMouseButtonDown(0) && !isCarrying)
    {
        // Cast a ray from the mouse position to see if there is an object to pick up
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0f, objectLayers);

        if (hit.collider != null && Vector2.Distance(transform.position, hit.collider.transform.position) <= pickupRange && hit.collider.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            // Pick up the object and set the player as its parent
            carriedObject = hit.collider.gameObject;
            carriedObject.transform.SetParent(transform);
            isCarrying = true;
        }
    }
    
    // Check if the player wants to throw the object
    else if (Input.GetMouseButtonDown(1) && isCarrying)
    {
        // Throw the object in the direction of the mouse pointer and remove the player as its parent
        carriedObject.transform.SetParent(null);
        carriedObject.GetComponent<Rigidbody2D>().velocity = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized * throwForce;
        carriedObject = null;
        isCarrying = false;
    }

    // Check if the player wants to drop the object
    else if (Input.GetMouseButtonDown(0) && isCarrying)
    {
        // Drop the object and remove the player as its parent
        carriedObject.transform.SetParent(null);
        carriedObject = null;
        isCarrying = false;
    }

    // Move the object in front of the player if they are carrying one
    if (isCarrying)
    {
        carriedObject.transform.position = transform.position + (Vector3.right * carryDistance * transform.localScale.x);
    }
}
}
