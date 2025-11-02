using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(AudioSource))]
public class MicrowaveButton : MonoBehaviour
{
    public GameObject MicroPlate;
    private bool Funga = false;
    private GameObject plateog;
    Vector3 PlatePos;
    Quaternion PlateRot;
    AudioSource audioData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        XRSimpleInteractable interactable = GetComponentInChildren<XRSimpleInteractable>();
        audioData = GetComponent<AudioSource>();

        if (interactable != null)
        {
            interactable.selectEntered.AddListener(OnButtonPressed);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goob"))
            {
            Funga = true;
            plateog = other.gameObject;
            PlatePos = other.transform.position;
            PlateRot = other.transform.rotation;
        }
    }

    void OnButtonPressed(SelectEnterEventArgs args)
    {
        audioData.Play();
        if (Funga == true)
        {
            Destroy(plateog);
            Instantiate(MicroPlate, PlatePos, PlateRot);
            Funga = false;
        }
    }
    
}
