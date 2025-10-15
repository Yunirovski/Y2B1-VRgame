using UnityEngine;
using UnityEngine.AI;

public class CustomerOrderSystem : MonoBehaviour
{
    [Header("Order Item")]
    public GameObject heldItem; // Item in hand (defines order type)
    public Transform itemHoldPosition; // Where the item is held

    [Header("Order Type")]
    public string orderType; // Automatically set from held item's tag

    [Header("Movement")]
    public Transform queuePosition; // Position in the queue (assigned by QueueManager)
    public Transform exitPosition;  // Exit position
    public float stopDistance = 0.5f;

    [Header("State")]
    public bool hasReceivedOrder = false; // True if order is received
    public bool isAtCounter = false; // True if customer is at counter

    private NavMeshAgent agent;
    private CustomerState currentState;

    private enum CustomerState
    {
        MovingToQueue,
        WaitingInQueue,
        AtCounter,
        Leaving
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = CustomerState.MovingToQueue;

        // Set order type from held item tag
        if (heldItem != null)
        {
            orderType = heldItem.tag;
            if (itemHoldPosition != null)
            {
                heldItem.transform.SetParent(itemHoldPosition);
                heldItem.transform.localPosition = Vector3.zero;
                heldItem.transform.localRotation = Quaternion.identity;
            }
        }

        // Move to queue position
        if (queuePosition != null)
        {
            agent.SetDestination(queuePosition.position);
        }
    }

    void Update()
    {
        switch (currentState)
        {
            case CustomerState.MovingToQueue:
                CheckArrivalAtQueue();
                break;

            case CustomerState.WaitingInQueue:
                // Keep adjusting position in queue
                if (queuePosition != null)
                {
                    float distance = Vector3.Distance(transform.position, queuePosition.position);
                    if (distance > stopDistance)
                    {
                        agent.SetDestination(queuePosition.position);
                    }
                }
                break;

            case CustomerState.AtCounter:
                // Wait for order
                if (hasReceivedOrder)
                {
                    StartLeaving();
                }
                break;

            case CustomerState.Leaving:
                CheckArrivalAtExit();
                break;
        }
    }

    void CheckArrivalAtQueue()
    {
        if (queuePosition == null) return;

        if (Vector3.Distance(transform.position, queuePosition.position) < stopDistance)
        {
            currentState = CustomerState.WaitingInQueue;
        }
    }

    // Called when customer reaches the counter
    public void ArrivedAtCounter()
    {
        currentState = CustomerState.AtCounter;
        isAtCounter = true;
        Debug.Log($"Customer arrived at counter. Order: {orderType}");
    }

    // Called when player gives an item
    public void ReceiveOrder(GameObject orderItem)
    {
        Debug.Log($"Received item: {orderItem.tag}, Expected: {orderType}");

        // Check if correct item
        if (orderItem.tag == orderType)
        {
            Debug.Log("✓ Correct order. Customer leaving.");

            if (heldItem != null)
                Destroy(heldItem);

            orderItem.transform.SetParent(itemHoldPosition);
            orderItem.transform.localPosition = Vector3.zero;
            orderItem.transform.localRotation = Quaternion.identity;

            hasReceivedOrder = true;
            StartLeaving();
        }
        else
        {
            Debug.Log($"✗ Wrong order! Wanted {orderType}");
        }
    }

    void StartLeaving()
    {
        Debug.Log("Customer leaving.");
        currentState = CustomerState.Leaving;
        isAtCounter = false;

        if (exitPosition != null)
        {
            agent.SetDestination(exitPosition.position);
        }
        else
        {
            Debug.LogError("⚠ Exit position not set!");
        }

        // Notify QueueManager
        QueueManager queueManager = FindObjectOfType<QueueManager>();
        if (queueManager != null)
        {
            queueManager.CustomerLeft(this);
        }
    }

    void CheckArrivalAtExit()
    {
        if (exitPosition == null) return;

        if (Vector3.Distance(transform.position, exitPosition.position) < stopDistance)
        {
            Destroy(gameObject);
        }
    }

    // Debug: draw line to queue position
    void OnDrawGizmos()
    {
        if (queuePosition != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, queuePosition.position);
        }
    }
}
