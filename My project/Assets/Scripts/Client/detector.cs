using UnityEngine;

public class OrderDeliveryDetector : MonoBehaviour
{
    [Header("Detection Settings")]
    public string orderTag = "Goob"; // Tag used for delivered orders

    [Header("References")]
    public QueueManager_Human humanQueueManager;  // Reference to Human queue
    public QueueManager_Robot robotQueueManager;  // Reference to Robot queue

    // Called when an object enters the trigger zone
    void OnTriggerEnter(Collider other)
    {
        // Check if it's an order
        if (other.CompareTag(orderTag))
        {
            Debug.Log("Order placed in the delivery area!");

            // Try Human queue first
            CustomerOrderSystem currentCustomer = null;

            if (humanQueueManager != null)
            {
                currentCustomer = humanQueueManager.GetCurrentCustomer();
            }

            // If no Human customer, try Robot queue
            if (currentCustomer == null && robotQueueManager != null)
            {
                currentCustomer = robotQueueManager.GetCurrentCustomer();
            }

            if (currentCustomer != null)
            {
                Debug.Log("Customer found — delivering order.");
                currentCustomer.ReceiveOrder(other.gameObject);
            }
            else
            {
                Debug.Log("No customer at either counter right now.");
            }
        }
    }
}