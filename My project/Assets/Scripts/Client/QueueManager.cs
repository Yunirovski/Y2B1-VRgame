using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    [Header("Queue Settings")]
    public Transform[] queuePositions; // Queue position points (set manually in scene)
    public Transform counterPosition;  // Counter position
    public float moveDelay = 1f;       // Delay before queue moves after someone leaves

    [Header("Customer Spawning")]
    public GameObject[] customerPrefabs; // Different customer prefabs
    public Transform spawnPoint;         // Spawn position
    public float spawnInterval = 5f;     // Time between spawns

    private List<CustomerOrderSystem> queue = new List<CustomerOrderSystem>();
    private float nextSpawnTime;

    void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;
    }

    void Update()
    {
        // Spawn customers automatically
        if (Time.time >= nextSpawnTime && queue.Count < queuePositions.Length)
        {
            SpawnCustomer();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    // Spawn a new customer
    void SpawnCustomer()
    {
        if (customerPrefabs.Length == 0 || spawnPoint == null) return;

        GameObject prefab = customerPrefabs[Random.Range(0, customerPrefabs.Length)];
        GameObject newCustomer = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        CustomerOrderSystem customer = newCustomer.GetComponent<CustomerOrderSystem>();
        if (customer != null)
        {
            AddCustomerToQueue(customer);
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

            // Move others forward
            UpdateQueue();
        }
    }

    // Update all queue positions
    void UpdateQueue()
    {
        Debug.Log($"=== Updating queue. Count: {queue.Count} ===");

        for (int i = 0; i < queue.Count; i++)
        {
            if (queue[i] == null) continue;

            if (i == 0)
            {
                // First customer → counter
                queue[i].queuePosition = counterPosition;
                Debug.Log($"Customer {i}: assigned to counter");

                // Check if already arrived
                float distance = Vector3.Distance(queue[i].transform.position, counterPosition.position);
                if (distance < queue[i].stopDistance && !queue[i].isAtCounter)
                {
                    queue[i].ArrivedAtCounter();
                    Debug.Log($"Customer {i}: arrived at counter");
                }
            }
            else if (i - 1 < queuePositions.Length)
            {
                // Others → queue positions
                queue[i].queuePosition = queuePositions[i - 1];
                Debug.Log($"Customer {i}: assigned to {queuePositions[i - 1].name}");
            }
            else
            {
                Debug.LogWarning($"Customer {i}: not enough queue positions!");
            }
        }
    }

    // Get the customer currently at the counter
    public CustomerOrderSystem GetCurrentCustomer()
    {
        if (queue.Count > 0 && queue[0] != null)
        {
            return queue[0];
        }
        return null;
    }

    // Draw queue positions in editor
    void OnDrawGizmos()
    {
        if (queuePositions == null) return;

        Gizmos.color = Color.green;
        foreach (Transform pos in queuePositions)
        {
            if (pos != null)
                Gizmos.DrawWireSphere(pos.position, 0.3f);
        }

        if (counterPosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(counterPosition.position, 0.5f);
        }
    }
}
