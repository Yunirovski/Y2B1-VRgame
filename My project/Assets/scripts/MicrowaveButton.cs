using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
public class MicrowaveButton : MonoBehaviour
{
    public GameObject MicroPlate;
    public bool unga = false;
    private GameObject plateog;
    Vector3 PlatePos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        XRSimpleInteractable interactable = GetComponentInChildren<XRSimpleInteractable>();

        if (interactable != null)
        {
            interactable.selectEntered.AddListener(OnButtonPressed);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameObject.FindWithTag("Goob"))
        {
            unga = true;
            plateog = other.gameObject;
            PlatePos = other.transform.position;
        }
    }

    void OnButtonPressed(SelectEnterEventArgs args)
    {
        if (unga == true)
        {
            Destroy(plateog);
            Instantiate(MicroPlate, PlatePos, Quaternion.identity);
            unga = false;
        }
    }
    
}
