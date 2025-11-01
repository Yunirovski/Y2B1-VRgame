using System.Collections.Generic;
using UnityEngine;

public class QueueManager_Robot : MonoBehaviour
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

    [Header("Order Target")]
    public int orderTargetCount = 10;   // How many orders to complete (set by DayManager)

    // Internal variables
    public List<CustomerOrderSystem> queue = new List<CustomerOrderSystem>();
    private float nextSpawnTime;

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

        // Check if we should still spawn customers
        int ordersCompleted = GameStatsManager.robotOrdersCompleted;
        bool shouldStillSpawn = ordersCompleted < orderTargetCount;

        if (Time.time >= nextSpawnTime && queue.Count < effectiveMax && shouldStillSpawn)
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
                    Debug.Log($"Robot customer arrived at counter (distance: {distance:F2})");
                }
            }
        }
    }

    // Spawn Robot customers only
    void SpawnCustomer()
    {
        if (customerPrefabs.Length == 0 || spawnPoint == null) return;

        // Keep spawning until we get a Robot type customer
        CustomerOrderSystem customer = null;

        while (customer == null || customer.customerType != CustomerOrderSystem.CustomerType.Robot)
        {
            GameObject prefab = customerPrefabs[Random.Range(0, customerPrefabs.Length)];
            GameObject newCustomer = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

            customer = newCustomer.GetComponent<CustomerOrderSystem>();

            if (customer == null || customer.customerType != CustomerOrderSystem.CustomerType.Robot)
            {
                if (newCustomer != null)
                    Destroy(newCustomer);
                customer = null;
            }
        }

        // Now we have a confirmed Robot customer
        AddCustomerToQueue(customer);
        Debug.Log($"Robot customer spawned. Target: {orderTargetCount}, Completed: {GameStatsManager.robotOrdersCompleted}");
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
            Debug.Log($"Robot customer left. Remaining: {queue.Count - 1}");
            queue.Remove(customer);
            UpdateQueue();
        }
    }

    void UpdateQueue()
    {
        Debug.Log($"=== Updating Robot queue. Count: {queue.Count} ===");

        for (int i = 0; i < queue.Count; i++)
        {
            if (queue[i] == null) continue;

            if (i == 0)
            {
                queue[i].queuePosition = counterPosition;
                Debug.Log($"Robot Customer {i}: assigned to counter");
            }
            else if (i - 1 < queuePositions.Length)
            {
                queue[i].queuePosition = queuePositions[i - 1];
                Debug.Log($"Robot Customer {i}: assigned to {queuePositions[i - 1].name}");
            }
            else
            {
                Debug.LogWarning($"Robot Customer {i}: too many! Using last position.");
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

        Gizmos.color = Color.blue;
        for (int i = 0; i < queuePositions.Length; i++)
        {
            if (queuePositions[i] != null)
            {
                Gizmos.DrawWireSphere(queuePositions[i].position, 0.3f);

#if UNITY_EDITOR
                UnityEditor.Handles.Label(
                    queuePositions[i].position + Vector3.up * 0.5f,
                    $"Robot Queue {i + 1}"
                );
#endif
            }
        }

        if (counterPosition != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(counterPosition.position, 0.5f);

#if UNITY_EDITOR
            UnityEditor.Handles.Label(
                counterPosition.position + Vector3.up * 0.5f,
                "Robot Counter"
            );
#endif
        }
    }
}