using UnityEngine;

public class CustomerMove : MonoBehaviour
{
    public Transform whereToGo; // Where the customer walks to
    public float speed = 1f;    // Walk speed

    void Update()
    {
        // Walk to the target
        if (whereToGo != null)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                whereToGo.position,
                speed * Time.deltaTime
            );
        }
    }
}