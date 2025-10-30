using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    [Header("Queue Settings")]
    public Transform[] queuePositions; // Queue points (set manually in the scene)
    public Transform counterPosition;  // Counter position
    public float moveDelay = 1f;       // Delay before queue moves after someone leaves

    [Header("Customer Spawning")]
    public GameObject[] customerPrefabs; // Different types of customer prefabs
    public Transform spawnPoint;         // Where customers appear
    public float spawnInterval = 5f;     // Time between new customers
    public int maxQueueSize = 5;        // Max number of customers (0 = unlimited)

    private List<CustomerOrderSystem> queue = new List<CustomerOrderSystem>();
    private float nextSpawnTime;

    void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;

        // If maxQueueSize is 0, set it to unlimited
        if (maxQueueSize == 0)
        {
            maxQueueSize = int.MaxValue;
        }
    }

    void Update()
    {
        // Total queue capacity = counter (1) + waiting spots
        int maxCapacity = queuePositions.Length + 1;

        // Use the smaller value between user-defined and actual capacity
        int effectiveMax = Mathf.Min(maxCapacity, maxQueueSize);

        // Spawn new customers automatically
        if (Time.time >= nextSpawnTime && queue.Count < effectiveMax)
        {
            SpawnCustomer();
            nextSpawnTime = Time.time + spawnInterval;
        }

        // Check if first customer has arrived at counter
        CheckFirstCustomerArrival();
    }

    // Check if the first customer in queue has reached the counter
    void CheckFirstCustomerArrival()
    {
        if (queue.Count > 0 && queue[0] != null && counterPosition != null)
        {
            CustomerOrderSystem firstCustomer = queue[0];

            // If customer is assigned to counter but not marked as at counter
            if (firstCustomer.queuePosition == counterPosition && !firstCustomer.isAtCounter)
            {
                float distance = Vector3.Distance(firstCustomer.transform.position, counterPosition.position);

                if (distance < firstCustomer.stopDistance)
                {
                    firstCustomer.ArrivedAtCounter();
                    Debug.Log($"First customer arrived at counter (distance: {distance:F2})");
                }
            }
        }
    }

    // Create a new customer
    void SpawnCustomer()
    {
        if (customerPrefabs.Length == 0 || spawnPoint == null) return;

        GameObject prefab = customerPrefabs[Random.Range(0, customerPrefabs.Length)];
        GameObject newCustomer = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        CustomerOrderSystem customer = newCustomer.GetComponent<CustomerOrderSystem>();
        if (customer != null)
        {
            AddCustomerToQueue(customer);
            Debug.Log($"New customer spawned. Total in queue: {queue.Count}");
        }
    }

    // Add a customer to the queue
    public void AddCustomerToQueue(CustomerOrderSystem customer)
    {
        queue.Add(customer);
        UpdateQueue();
    }

    // Called when a customer leaves
    public void CustomerLeft(CustomerOrderSystem customer)
    {
        if (queue.Contains(customer))
        {
            Debug.Log($"Customer left the queue. Remaining: {queue.Count - 1}");
            queue.Remove(customer);

            // Move everyone forward
            UpdateQueue();
        }
    }

    // Update queue positions for all customers
    void UpdateQueue()
    {
        Debug.Log($"=== Updating queue. Count: {queue.Count} ===");

        for (int i = 0; i < queue.Count; i++)
        {
            if (queue[i] == null) continue;

            if (i == 0)
            {
                // First customer goes to the counter
                queue[i].queuePosition = counterPosition;
                Debug.Log($"Customer {i}: assigned to counter");

                // Don't immediately call ArrivedAtCounter here
                // Let the Update() loop check distance continuously
            }
            else if (i - 1 < queuePositions.Length)
            {
                // Others go to queue positions
                queue[i].queuePosition = queuePositions[i - 1];
                Debug.Log($"Customer {i}: assigned to {queuePositions[i - 1].name}");
            }
            else
            {
                // If there are too many customers, place them at the last spot
                Debug.LogWarning($"Customer {i}: exceeds queue capacity! Assigning to last position.");
                if (queuePositions.Length > 0)
                {
                    queue[i].queuePosition = queuePositions[queuePositions.Length - 1];
                }
            }
        }
    }

    // Get the first customer (the one at the counter)
    public CustomerOrderSystem GetCurrentCustomer()
    {
        if (queue.Count > 0 && queue[0] != null)
        {
            return queue[0];
        }
        return null;
    }

    // Draw helper gizmos in the editor
    void OnDrawGizmos()
    {
        if (queuePositions == null) return;

        Gizmos.color = Color.green;
        for (int i = 0; i < queuePositions.Length; i++)
        {
            if (queuePositions[i] != null)
            {
                Gizmos.DrawWireSphere(queuePositions[i].position, 0.3f);

                // Show labels in the editor
#if UNITY_EDITOR
                UnityEditor.Handles.Label(
                    queuePositions[i].position + Vector3.up * 0.5f,
                    $"Queue {i + 1}"
                );
#endif
            }
        }

        if (counterPosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(counterPosition.position, 0.5f);

#if UNITY_EDITOR
            UnityEditor.Handles.Label(
                counterPosition.position + Vector3.up * 0.5f,
                "Counter"
            );
#endif
        }
    }
}