using UnityEngine;

public class Customer : MonoBehaviour
{
    public GameObject dest1;
    public GameObject dest2;
    public float stopDist = 2f;

    UnityEngine.AI.NavMeshAgent agent;
    bool atDest1 = false;
    bool orderDone = false;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.SetDestination(dest1.transform.position);
    }

    void Update()
    {
        // Check distance to dest1
        if (!atDest1)
        {
            float dist = Vector3.Distance(transform.position, dest1.transform.position);
            if (dist < stopDist)
            {
                atDest1 = true;
            }
        }

        // Go to dest2 when order done
        if (orderDone && atDest1)
        {
            agent.SetDestination(dest2.transform.position);
            orderDone = false; // Prevent repeat
        }
    }

    public void CompleteOrder()
    {
        orderDone = true;
    }
}