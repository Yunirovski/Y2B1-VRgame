using System.Collections.Generic;
using UnityEngine;

public class QueueManager_Human : MonoBehaviour
{
    [Header("Queue Settings")]
    public Transform[] queuePositions; // Queue spots in the scene
    public Transform counterPosition;  // Counter spot

    [Header("Customer Spawning")]
    public GameObject[] customerPrefabs; // Customer types
    public Transform spawnPoint;         // Where customers appear
    public float spawnInterval = 5f;     // Time between spawns
    public int maxQueueSize = 5;         // Max customers in queue at once

    // Internal variables
    public List<CustomerOrderSystem> queue = new List<CustomerOrderSystem>();
    private float nextSpawnTime;
    private int humanSpawnPercentage = 50;  // What percentage of customers should be human

    void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;

        if (maxQueueSize == 0)
        {
            maxQueueSize = int.MaxValue;
        }
    }

    void Update()
    {
        int maxCapacity = queuePositions.Length + 1;
        int effectiveMax = Mathf.Min(maxCapacity, maxQueueSize);

        // Check if we can still spawn customers
        bool hasReachedLimit = GameStatsManager.HasReachedCustomerLimit();

        if (!hasReachedLimit && Time.time >= nextSpawnTime && queue.Count < effectiveMax)
        {
            SpawnCustomer();
            nextSpawnTime = Time.time + spawnInterval;
        }

        CheckFirstCustomerArrival();
    }

    void CheckFirstCustomerArrival()
    {
        if (queue.Count > 0 && queue[0] != null && counterPosition != null)
        {
            CustomerOrderSystem firstCustomer = queue[0];

            if (firstCustomer.queuePosition == counterPosition && !firstCustomer.isAtCounter)
            {
                float distance = Vector3.Distance(firstCustomer.transform.position, counterPosition.position);

                if (distance < firstCustomer.stopDistance)
                {
                    firstCustomer.ArrivedAtCounter();
                    Debug.Log($"Human customer arrived at counter (distance: {distance:F2})");
                }
            }
        }
    }

    // Set the percentage of customers that should be human
    public void SetHumanSpawnPercentage(int percentage)
    {
        humanSpawnPercentage = Mathf.Clamp(percentage, 0, 100);
        Debug.Log($"Human spawn percentage set to {humanSpawnPercentage}%");
    }

    // Spawn customers (could be human or robot based on percentage)
    void SpawnCustomer()
    {
        if (customerPrefabs.Length == 0 || spawnPoint == null) return;

        // Decide if this should be human or robot based on percentage
        int randomChance = Random.Range(0, 100);
        bool shouldBeHuman = randomChance < humanSpawnPercentage;

        // Keep spawning until we get the right type
        CustomerOrderSystem customer = null;
        int attempts = 0;
        const int maxAttempts = 20;

        while ((customer == null || customer.customerType != GetDesiredType(shouldBeHuman)) && attempts < maxAttempts)
        {
            GameObject prefab = customerPrefabs[Random.Range(0, customerPrefabs.Length)];
            GameObject newCustomer = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

            customer = newCustomer.GetComponent<CustomerOrderSystem>();

            if (customer == null || customer.customerType != GetDesiredType(shouldBeHuman))
            {
                if (newCustomer != null)
                    Destroy(newCustomer);
                customer = null;
            }

            attempts++;
        }

        if (customer != null)
        {
            AddCustomerToQueue(customer);
            GameStatsManager.RegisterCustomerSpawned();
            Debug.Log($"Customer spawned ({(shouldBeHuman ? "Human" : "Robot")}). Queue: {queue.Count}");
        }
    }

    // Helper to get the desired customer type
    private CustomerOrderSystem.CustomerType GetDesiredType(bool wantHuman)
    {
        return wantHuman ? CustomerOrderSystem.CustomerType.Human : CustomerOrderSystem.CustomerType.Robot;
    }

    public void AddCustomerToQueue(CustomerOrderSystem customer)
    {
        queue.Add(customer);
        UpdateQueue();
    }

    public void CustomerLeft(CustomerOrderSystem customer)
    {
        if (queue.Contains(customer))
        {
            Debug.Log($"Customer left. Remaining: {queue.Count - 1}");
            queue.Remove(customer);
            UpdateQueue();
        }
    }

    void UpdateQueue()
    {
        Debug.Log($"=== Updating queue. Count: {queue.Count} ===");

        for (int i = 0; i < queue.Count; i++)
        {
            if (queue[i] == null) continue;

            if (i == 0)
            {
                queue[i].queuePosition = counterPosition;
            }
            else if (i - 1 < queuePositions.Length)
            {
                queue[i].queuePosition = queuePositions[i - 1];
            }
            else
            {
                if (queuePositions.Length > 0)
                {
                    queue[i].queuePosition = queuePositions[queuePositions.Length - 1];
                }
            }
        }
    }

    public CustomerOrderSystem GetCurrentCustomer()
    {
        if (queue.Count > 0 && queue[0] != null)
        {
            return queue[0];
        }
        return null;
    }

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