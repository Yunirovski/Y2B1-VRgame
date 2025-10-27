using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
public class MicrowaveButton : MonoBehaviour
{
    public GameObject MicroPlate;
    private bool unga = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        XRSimpleInteractable interactable = GetComponent<XRSimpleInteractable>();

        if (interactable != null)
        {
            interactable.selectEntered.AddListener(OnButtonPressed);
        }
    }

    void OnButtonPressed(SelectEnterEventArgs args)
    {
        if (unga == true)
        {
            Instantiate(MicroPlate, new Vector3(6.81647968f, 1.04572999f, -8.03367996f), Quaternion.identity);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == GameObject.FindWithTag("Goob"))
        {
            unga = true;
        }
        else
        {
            unga = false;
        }
    }
}
