using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    [Header("Queue Settings")]
    public Transform[] queuePositions; // Queue spots in the scene
    public Transform counterPosition;  // Counter spot
    public float moveDelay = 1f;       // Delay before queue moves

    [Header("Customer Spawning")]
    public GameObject[] customerPrefabs; // Customer types
    public Transform spawnPoint;         // Where customers appear
    public float spawnInterval = 5f;     // Time between spawns
    public int maxQueueSize = 5;         // Max customers at once

    [Header("Limited Spawning")]
    public bool unlimitedMode = false;   // Turn on for unlimited customers
    public int totalCustomersToSpawn = 10; // How many customers in total

    // Internal variables
    private List<CustomerOrderSystem> queue = new List<CustomerOrderSystem>();
    private float nextSpawnTime;
    public int customersSpawned = 0; // Counter for spawned customers

    void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;

        // If maxQueueSize is 0, make it unlimited
        if (maxQueueSize == 0)
        {
            maxQueueSize = int.MaxValue;
        }
    }

    void Update()
    {
        // Total spots = counter (1) + waiting spots
        int maxCapacity = queuePositions.Length + 1;

        // Use the smaller number
        int effectiveMax = Mathf.Min(maxCapacity, maxQueueSize);

        // Check if we can spawn more customers
        bool canSpawn = unlimitedMode || customersSpawned < totalCustomersToSpawn;

        // Spawn new customer if:
        // 1. Time is right
        // 2. Queue is not full
        // 3. We haven't reached the limit
        if (Time.time >= nextSpawnTime && queue.Count < effectiveMax && canSpawn)
        {
            SpawnCustomer();
            nextSpawnTime = Time.time + spawnInterval;
        }

        // Check if first customer reached counter
        CheckFirstCustomerArrival();
    }

    // Check if first customer arrived at counter
    void CheckFirstCustomerArrival()
    {
        if (queue.Count > 0 && queue[0] != null && counterPosition != null)
        {
            CustomerOrderSystem firstCustomer = queue[0];

            // If customer is going to counter but not marked as arrived
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

        // Pick random customer type
        GameObject prefab = customerPrefabs[Random.Range(0, customerPrefabs.Length)];
        GameObject newCustomer = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        CustomerOrderSystem customer = newCustomer.GetComponent<CustomerOrderSystem>();
        if (customer != null)
        {
            AddCustomerToQueue(customer);
            customersSpawned++; // Increase counter
            Debug.Log($"New customer spawned. Total: {customersSpawned}/{totalCustomersToSpawn}");
        }
    }

    // Add customer to queue
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
            Debug.Log($"Customer left. Remaining: {queue.Count - 1}");
            queue.Remove(customer);

            // Move everyone forward
            UpdateQueue();
        }
    }

    // Update positions for all customers
    void UpdateQueue()
    {
        Debug.Log($"=== Updating queue. Count: {queue.Count} ===");

        for (int i = 0; i < queue.Count; i++)
        {
            if (queue[i] == null) continue;

            if (i == 0)
            {
                // First customer goes to counter
                queue[i].queuePosition = counterPosition;
                Debug.Log($"Customer {i}: assigned to counter");
            }
            else if (i - 1 < queuePositions.Length)
            {
                // Others go to queue spots
                queue[i].queuePosition = queuePositions[i - 1];
                Debug.Log($"Customer {i}: assigned to {queuePositions[i - 1].name}");
            }
            else
            {
                // Too many customers - put at last spot
                Debug.LogWarning($"Customer {i}: too many! Using last position.");
                if (queuePositions.Length > 0)
                {
                    queue[i].queuePosition = queuePositions[queuePositions.Length - 1];
                }
            }
        }
    }

    // Get the customer at the counter
    public CustomerOrderSystem GetCurrentCustomer()
    {
        if (queue.Count > 0 && queue[0] != null)
        {
            return queue[0];
        }
        return null;
    }

    // Draw helper circles in editor
    void OnDrawGizmos()
    {
        if (queuePositions == null) return;

        Gizmos.color = Color.green;
        for (int i = 0; i < queuePositions.Length; i++)
        {
            if (queuePositions[i] != null)
            {
                Gizmos.DrawWireSphere(queuePositions[i].position, 0.3f);

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