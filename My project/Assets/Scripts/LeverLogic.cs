using UnityEngine;
using UnityEngine.Events;

public class NewMonoBehaviourScript : MonoBehaviour
{
    HingeJoint joint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        joint = GetComponent<HingeJoint>();
        
    }

    public UnityEvent OnEnterTrigger;

    private void OnTriggerStay(Collider other)
    {
        if (joint.angle > 55)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
