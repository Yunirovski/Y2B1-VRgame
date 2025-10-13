using UnityEngine;

public class SimpleDetector : MonoBehaviour
{
    public string tagName = "Goob"; // Tab
    public GameObject customer;  // The customer
    public Transform destination; // Where customer will go

    // When something touches this
    void OnTriggerEnter(Collider other)
    {
        // Is it the right tag?
        if (other.CompareTag(tagName))
        {
            Debug.Log("Order is here!");

            // Customer starts walking
            customer.GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(destination.position);
        }
    }
}