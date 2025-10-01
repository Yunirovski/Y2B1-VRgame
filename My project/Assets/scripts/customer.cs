using UnityEngine;

public class customer : MonoBehaviour
{
    public GameObject destination1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(destination1.transform.position);    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
