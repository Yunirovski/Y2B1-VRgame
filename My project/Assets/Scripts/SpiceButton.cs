using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(AudioSource))]
public class SpiceButton : MonoBehaviour
{
    public GameObject SpicePlate;
    private bool unga = false;
    private GameObject plateog;
    Vector3 PlatePos;
    Quaternion PlateRot;
    AudioSource audioData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        XRSimpleInteractable interactable = GetComponent<XRSimpleInteractable>();
        audioData = GetComponent<AudioSource>();

        if (interactable != null)
        {
            interactable.selectEntered.AddListener(OnButtonPressed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("cooked"))
        {
            unga = true;
            plateog = other.gameObject;
            PlatePos = other.transform.position;
            PlateRot = other.transform.rotation;
        }
    }

    void OnButtonPressed(SelectEnterEventArgs args)
    {
        audioData.Play();
        if (unga)
        {
            Destroy(plateog);
            Instantiate(SpicePlate, PlatePos, PlateRot);
            unga = false;
        }
    }
}
