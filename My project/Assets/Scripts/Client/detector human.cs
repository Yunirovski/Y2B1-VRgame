using UnityEngine;

public class OrderDeliveryDetector_Human : MonoBehaviour
{
    [Header("Detection Settings")]
    public string[] orderTags = { "goob", "pizza", "burger" }; // Valid order tags

    [Header("References")]
    public QueueManager_Human humanQueueManager;

    [Header("Feedback")]
    public GameObject correctOrderFX;   // Spawn when order correct
    public GameObject wrongOrderFX;     // Spawn when order wrong

    void OnTriggerEnter(Collider other)
    {
        // Check if it's a valid order
        bool isValidOrder = System.Array.Exists(orderTags, tag => other.CompareTag(tag));

        if (!isValidOrder)
        {
            Debug.Log("Invalid order placed!");
            return;
        }

        Debug.Log($"[HUMAN] Order placed: {other.tag}");

        CustomerOrderSystem currentCustomer = humanQueueManager.GetCurrentCustomer();

        if (currentCustomer != null)
        {
            Debug.Log($"[HUMAN] Customer expects: {currentCustomer.orderType}");

            // Check if order matches
            if (other.tag == currentCustomer.orderType)
            {
                Debug.Log("[HUMAN] ✓ CORRECT ORDER!");
                currentCustomer.ReceiveOrder(other.gameObject);

                if (correctOrderFX != null)
                {
                    Instantiate(correctOrderFX, other.transform.position, Quaternion.identity);
                }
            }
            else
            {
                Debug.Log($"[HUMAN] ✗ WRONG ORDER! Got {other.tag}, expected {currentCustomer.orderType}");

                if (wrongOrderFX != null)
                {
                    Instantiate(wrongOrderFX, other.transform.position, Quaternion.identity);
                }
            }
        }
        else
        {
            Debug.Log("[HUMAN] No customer at counter.");
        }
    }
}