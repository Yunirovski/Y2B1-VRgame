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
        if (joint.angle > 57.0) {
            Vector3 dw = transform.TransformDirection(Vector3.down);

            if (Physics.Raycast(transform.position, dw, 10))
            {
                Debug.Log("ollison");
            }
        }
    }
}
