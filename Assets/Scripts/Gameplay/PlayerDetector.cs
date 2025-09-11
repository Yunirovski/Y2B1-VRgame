using UnityEngine;

// Attach to detectable objects
public class DetectableObject : MonoBehaviour
{
    [Header("Target Object")]
    public GameObject targetObject; // The object to show/hide

    private SpriteRenderer sr;
    private bool playerInside = false; // Preventing repeated triggering

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        // Hide target object at start
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !playerInside)
        {
            playerInside = true;
            Debug.Log("Player entered " + gameObject.name);

            // Change color
            if (sr != null) sr.color = Color.red;

            // Show target object
            if (targetObject != null)
            {
                targetObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && playerInside)
        {
            playerInside = false;
            Debug.Log("Player left " + gameObject.name);

            // Change color back
            if (sr != null) sr.color = Color.white;

            // Hide target object
            if (targetObject != null)
            {
                targetObject.SetActive(false);
            }
        }
    }
}