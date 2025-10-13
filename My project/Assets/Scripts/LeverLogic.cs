using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class NewMonoBehaviourScript : MonoBehaviour
{
    HingeJoint joint;
    public GameObject GoobEmpty;
    public GameObject GoobFull;

    private bool Used;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   

        joint = GetComponentInChildren<HingeJoint>();
    }

    public UnityEvent OnEnterTrigger;

    private void OnTriggerStay(Collider other)
    {
        if (joint.angle > 55)
        {
            if (!Used)
            {
                if (other.gameObject == GameObject.FindWithTag("Empty")) {
                    Destroy(other.gameObject);
                    Instantiate(GoobFull, other.transform.position, Quaternion.identity);
                    Used = true;
                }
            }
        }
        else
        {
            Used = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
}
