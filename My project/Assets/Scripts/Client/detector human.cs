using UnityEngine;
using System.Collections.Generic;

public class OrderDeliveryDetector_Human : MonoBehaviour
{
    [Header("Detection Settings")]
    public string[] orderTags = { "goob", "pizza", "burger" };

    [Header("References")]
    public QueueManager_Human humanQueueManager;

    [Header("Feedback")]
    public GameObject correctOrderFX;
    public GameObject wrongOrderFX;

    // Track items already processed
    private HashSet<Collider> processedOrders = new HashSet<Collider>();

    void OnTriggerStay(Collider other)
    {
        // Skip if already processed
        if (processedOrders.Contains(other))
            return;

        // Check if valid order
        bool isValidOrder = System.Array.Exists(orderTags, tag => other.CompareTag(tag));

        if (!isValidOrder)
            return;

        CustomerOrderSystem currentCustomer = humanQueueManager.GetCurrentCustomer();

        if (currentCustomer != null)
        {
            // Check if order timer started
            if (!currentCustomer.orderTimerActive)
                return;

            // Mark as processed
            processedOrders.Add(other);

            // Check if order matches
            if (other.tag == currentCustomer.orderType)
            {
                currentCustomer.ReceiveOrder(other.gameObject);

                if (correctOrderFX != null)
                    Instantiate(correctOrderFX, other.transform.position, Quaternion.identity);
            }
            else
            {
                if (wrongOrderFX != null)
                    Instantiate(wrongOrderFX, other.transform.position, Quaternion.identity);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Clear record when item leaves
        processedOrders.Remove(other);
    }
}