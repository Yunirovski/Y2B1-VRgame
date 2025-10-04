using UnityEngine;

public class Customer : MonoBehaviour
{
    public GameObject destination1; // First destination (e.g. table)
    public GameObject destination2; // Second destination (e.g. exit)
    public float arrivalDistance = 0.5f; // Distance to consider "arrived"

    private UnityEngine.AI.NavMeshAgent agent;
    private bool reachedDest1 = false;
    private bool orderDone = false;
    private bool movedToDest2 = false;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.SetDestination(destination1.transform.position);
    }

    void Update()
    {
        // Check if reached destination 1
        if (!reachedDest1)
        {
            if (Vector3.Distance(transform.position, destination1.transform.position) <= arrivalDistance)
            {
                reachedDest1 = true;
            }
        }

        // Move to destination 2 after order is done
        if (orderDone && reachedDest1 && !movedToDest2)
        {
            agent.SetDestination(destination2.transform.position);
            movedToDest2 = true;
        }
    }

    // Call this when order is complete
    public void CompleteOrder()
    {
        orderDone = true;
    }
}