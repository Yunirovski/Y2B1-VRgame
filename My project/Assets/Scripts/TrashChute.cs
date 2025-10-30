using UnityEngine;
using UnityEngine.Events;

public class TrashChute : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    public UnityEvent OnEnterTrigger;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject == GameObject.FindWithTag("Player"))
        {
            Destroy(other.gameObject);
        }
    }
}