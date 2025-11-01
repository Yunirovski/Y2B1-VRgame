using UnityEngine;
using UnityEngine.AI;

public class CustomerOrderSystem : MonoBehaviour
{
    [Header("Order Item")]
    public GameObject heldItem;              // Item the customer is holding (defines order type)
    public Transform itemHoldPosition;       // Where the item sits in the customer's hand

    [Header("Order Type")]
    public string orderType;                 // Order type (taken from heldItem.tag)

    public CustomerType customerType;

    public enum CustomerType
    {
        Human,
        Robot
    }

    [Header("Movement")]
    public Transform queuePosition;          // Assigned by QueueManager
    public Transform exitPosition;           // Where the customer leaves
    public float stopDistance = 0.5f;        // Distance to consider "arrived"
    public float positionCheckInterval = 0.2f; // How often we check if the target moved

    [Header("Timing")]
    public float waitTimeBeforeTimeout = 15f; // Seconds before customer leaves unhappy

    [Header("State")]
    public bool hasReceivedOrder = false;    // Did the customer get the item?
    public bool isAtCounter = false;         // Is the customer at the counter?
    public bool orderTimerActive = false;    // Is timer running? (made public for detector access)

    private NavMeshAgent agent;
    private CustomerState currentState;
    private float nextPositionCheck;
    private Vector3 lastTargetPosition;      // Last target position we set on the agent
    private Transform lastQueuePosition;     // Track last assigned queue position

    private float orderTimer = 0f;           // Timer for waiting at counter
    private bool orderCompleted = false;     // Has order been completed?

    private enum CustomerState
    {
        MovingToQueue,   // Walking to their queue spot (or counter if first)
        WaitingInQueue,  // Standing in the queue (following reassignments)
        AtCounter,       // At the counter, waiting for item
        Leaving          // Walking to exit
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Basic safety: we need a NavMeshAgent to move
        if (agent == null)
        {
            Debug.LogError("CustomerOrderSystem: Missing NavMeshAgent component.");
            enabled = false;
            return;
        }

        if (!agent.enabled) agent.enabled = true;
        agent.isStopped = false;

        currentState = CustomerState.MovingToQueue;
        nextPositionCheck = Time.time;
        lastTargetPosition = Vector3.zero;
        lastQueuePosition = null;

        // Set order type from the held item's tag and attach the item to the hand
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

        // Start moving to the initial queue position (if assigned)
        if (queuePosition != null)
        {
            lastTargetPosition = queuePosition.position;
            lastQueuePosition = queuePosition;
            agent.SetDestination(queuePosition.position);
        }
    }

    void Update()
    {
        // Check if queue position changed - if so, start moving again
        if (queuePosition != lastQueuePosition)
        {
            lastQueuePosition = queuePosition;
            currentState = CustomerState.MovingToQueue;
            Debug.Log($"Queue position changed! Back to moving. New target: {queuePosition.name}");
        }

        switch (currentState)
        {
            case CustomerState.MovingToQueue:
                CheckArrivalAtQueue();
                UpdateDestinationIfNeeded();
                break;

            case CustomerState.WaitingInQueue:
                UpdateDestinationIfNeeded();
                break;

            case CustomerState.AtCounter:
                UpdateOrderTimer();
                break;

            case CustomerState.Leaving:
                CheckArrivalAtExit();
                break;
        }
    }

    // Check if the queue target Transform moved and update the agent's destination
    void UpdateDestinationIfNeeded()
    {
        if (queuePosition == null || agent == null) return;

        if (Time.time >= nextPositionCheck)
        {
            Vector3 currentTargetPosition = queuePosition.position;

            // If the Transform's position changed since last time, update NavMeshAgent
            if (Vector3.Distance(lastTargetPosition, currentTargetPosition) > 0.01f)
            {
                Debug.Log($"Queue position moved -> updating destination from {lastTargetPosition} to {currentTargetPosition}");
                agent.isStopped = false;
                agent.SetDestination(currentTargetPosition);
                lastTargetPosition = currentTargetPosition;
            }

            nextPositionCheck = Time.time + positionCheckInterval;
        }
    }

    // When walking to the queue, check if we've reached the current target
    void CheckArrivalAtQueue()
    {
        if (queuePosition == null) return;

        if (Vector3.Distance(transform.position, queuePosition.position) < stopDistance)
        {
            currentState = CustomerState.WaitingInQueue;
            Debug.Log("Customer arrived at queue position.");
        }
    }

    // Called by QueueManager when this customer should be at the counter
    public void ArrivedAtCounter()
    {
        currentState = CustomerState.AtCounter;
        isAtCounter = true;
        Debug.Log($"Customer arrived at counter. Order: {orderType}");
    }

    // Called when player clicks accept order button
    public void StartOrderTimer()
    {
        if (currentState == CustomerState.AtCounter && !orderTimerActive)
        {
            orderTimer = waitTimeBeforeTimeout;
            orderTimerActive = true;
            Debug.Log($"Order timer started: {waitTimeBeforeTimeout}s for {orderType}");
        }
    }

    // Update the timer while waiting for order
    void UpdateOrderTimer()
    {
        if (!orderTimerActive || orderCompleted) return;

        orderTimer -= Time.deltaTime;

        // Update UI with remaining time
        if (customerType == CustomerType.Human)
            GameStatsManager.UpdateTimer_Human(orderTimer);
        else
            GameStatsManager.UpdateTimer_Robot(orderTimer);

        // Check if order received
        if (hasReceivedOrder)
        {
            if (orderTimer > 0)
            {
                // Order completed on time - INCREMENT STATS HERE
                if (customerType == CustomerType.Human)
                    GameStatsManager.AddHumanOrder();
                else
                    GameStatsManager.AddRobotOrder();

                orderCompleted = true;
                orderTimerActive = false;
                Debug.Log($"✓✓✓ ORDER COMPLETED! Type: {orderType}");

                // NOW leave after counting
                StartLeaving();
            }
            return;
        }

        // Time ran out
        if (orderTimer <= 0)
        {
            Debug.Log(">>> TIMER EXPIRED - Customer timed out and is leaving unhappy");
            GameStatsManager.AddTimedOutCustomer();
            orderCompleted = true;
            orderTimerActive = false;

            // Customer leaves when timer expires
            StartLeaving();
        }
    }

    // Player gives an item to the customer
    public void ReceiveOrder(GameObject orderItem)
    {
        if (orderItem == null)
        {
            Debug.LogWarning("ReceiveOrder called with null item.");
            return;
        }

        Debug.Log($"Received item: {orderItem.tag}, expected: {orderType}");

        // Check if the item matches the expected order
        if (orderItem.tag == orderType)
        {
            Debug.Log("✓ Correct order. Customer will leave.");

            if (heldItem != null)
                Destroy(heldItem);

            if (itemHoldPosition != null)
            {
                orderItem.transform.SetParent(itemHoldPosition);
                orderItem.transform.localPosition = Vector3.zero;
                orderItem.transform.localRotation = Quaternion.identity;
            }

            // Mark that order was received
            // UpdateOrderTimer() will handle the rest (counting and leaving)
            hasReceivedOrder = true;
        }
        else
        {
            Debug.Log($"✗ Wrong order. Expected {orderType}.");
        }
    }

    // Begin leaving the store
    void StartLeaving()
    {
        Debug.Log("Customer leaving.");
        currentState = CustomerState.Leaving;
        isAtCounter = false;

        if (agent == null) return;

        if (exitPosition != null)
        {
            agent.isStopped = false;
            agent.SetDestination(exitPosition.position);
        }
        else
        {
            Debug.LogError("Exit position is not set!");
        }

        // Notify QueueManager so others can move forward
        QueueManager_Human humanQueue = FindFirstObjectByType<QueueManager_Human>();
        if (humanQueue != null)
        {
            humanQueue.CustomerLeft(this);
        }

        QueueManager_Robot robotQueue = FindFirstObjectByType<QueueManager_Robot>();
        if (robotQueue != null)
        {
            robotQueue.CustomerLeft(this);
        }
    }

    // When leaving, check if we've reached the exit
    void CheckArrivalAtExit()
    {
        if (exitPosition == null) return;

        if (Vector3.Distance(transform.position, exitPosition.position) < stopDistance)
        {
            Destroy(gameObject);
        }
    }

    // Debug drawing in the Scene view
    void OnDrawGizmos()
    {
        if (queuePosition != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, queuePosition.position);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(queuePosition.position, 0.3f);
        }
    }
}