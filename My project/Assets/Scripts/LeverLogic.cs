using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

[RequireComponent(typeof(AudioSource))]
public class NewMonoBehaviourScript : MonoBehaviour
{
    HingeJoint joint;
    public GameObject GoobEmpty;
    public GameObject GoobFull;
    AudioSource audioData;

    private bool Used;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   

        joint = GetComponentInChildren<HingeJoint>();
        audioData = GetComponent<AudioSource>();
    }

    public UnityEvent OnEnterTrigger;

    private void OnTriggerStay(Collider other)
    {
        if (joint.angle > 55)
        {
            if (!Used)
            {
                if (other.CompareTag("Empty"))
                {
                    audioData.Play();
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
