using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    HingeJoint joint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        joint = GetComponent<HingeJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        if (joint.angle > 60.0) {
            Debug.Log("Lever is at " + joint.angle);
        }
    }
}
