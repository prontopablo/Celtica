using UnityEngine;

public class Door : MonoBehaviour
{
    public float doorOpenHeight = 2f; // height to open the door
    public float doorOpenSpeed = 2f; // speed at which the door opens and closes
    
    private Vector3 originalPosition;
    private bool isDoorOpen = false;
    
    void Start()
    {
        originalPosition = transform.localPosition;
    }
    
    void OnMouseDown()
    {
        if (!isDoorOpen)
        {
            // open the door
            transform.localPosition += new Vector3(0f, doorOpenHeight, 0f);
            isDoorOpen = true;
        }
        else
        {
            // close the door
            transform.localPosition = originalPosition;
            isDoorOpen = false;
        }
    }
    
    void Update()
    {
        if (isDoorOpen && transform.localPosition.y < originalPosition.y + doorOpenHeight)
        {
            // continue opening the door until it reaches the desired height
            transform.localPosition += new Vector3(0f, doorOpenSpeed * Time.deltaTime, 0f);
        }
        else if (!isDoorOpen && transform.localPosition.y > originalPosition.y)
        {
            // continue closing the door until it reaches its original position
            transform.localPosition -= new Vector3(0f, doorOpenSpeed * Time.deltaTime, 0f);
        }
    }
}
