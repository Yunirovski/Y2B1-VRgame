using UnityEngine;

public class OrderDeliveryDetector : MonoBehaviour
{
    [Header("Detection Settings")]
    public string orderTag = "Goob"; // Tag for order items

    [Header("References")]
    public QueueManager queueManager;
    public Transform targetPosition; // Where item moves to

    [Header("Movement Settings")]
    public float moveSpeed = 2f; // How fast item moves

    private GameObject movingItem; // The item currently moving
    private bool isMoving = false; // Is an item moving right now?

    void Update()
    {
        // Move item to target
        if (isMoving && movingItem != null)
        {
            movingItem.transform.position = Vector3.MoveTowards(
                movingItem.transform.position,
                targetPosition.position,
                moveSpeed * Time.deltaTime
            );

            // Check if arrived
            float distance = Vector3.Distance(movingItem.transform.position, targetPosition.position);
            if (distance < 0.1f)
            {
                DeliverOrder();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if it's an order and nothing is moving
        if (other.CompareTag(orderTag) && !isMoving)
        {
            movingItem = other.gameObject;
            isMoving = true;

            // Disable physics
            Rigidbody rb = movingItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }
        }
    }

    void DeliverOrder()
    {
        // Get current customer
        CustomerOrderSystem customer = queueManager.GetCurrentCustomer();

        if (customer != null)
        {
            customer.ReceiveOrder(movingItem);
        }

        // Destroy item and reset
        Destroy(movingItem);
        movingItem = null;
        isMoving = false;
    }
}