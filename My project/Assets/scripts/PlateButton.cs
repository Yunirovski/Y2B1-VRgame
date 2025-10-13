using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
public class PlateButton : MonoBehaviour
{
    public GameObject Plate;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        XRSimpleInteractable interactable = GetComponent<XRSimpleInteractable>();

        if (interactable != null )
        {
            interactable.selectEntered.AddListener(OnButtonPressed);
        }
    }

    void OnButtonPressed(SelectEnterEventArgs args)
    {
        Instantiate(Plate, new Vector3(1.48399997f, 1.02999997f, -5.70928907f), Quaternion.identity);
    }
}
