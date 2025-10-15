using UnityEngine;

public class OrderDeliveryDetector : MonoBehaviour
{
    [Header("Detection Settings")]
    public string orderTag = "CompletedOrder"; // Tag used for delivered orders

    [Header("References")]
    public QueueManager queueManager; // Reference to the QueueManager

    // Called when an object enters the trigger zone
    void OnTriggerEnter(Collider other)
    {
        // Check if it's an order
        if (other.CompareTag(orderTag))
        {
            Debug.Log("Order placed in the delivery area!");

            // Get the current customer at the counter (first in queue)
            CustomerOrderSystem currentCustomer = queueManager.GetCurrentCustomer();

            if (currentCustomer != null)
            {
                Debug.Log("Customer found — delivering order.");

                // Give the order to the customer (triggers their leaving process)
                currentCustomer.ReceiveOrder(other.gameObject);
            }
            else
            {
                Debug.Log("No customer at the counter right now.");
            }
        }
    }
}
