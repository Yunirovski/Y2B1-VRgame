using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(AudioSource))]
public class PlateButton : MonoBehaviour
{
    public GameObject Plate;
    AudioSource audioData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        XRSimpleInteractable interactable = GetComponent<XRSimpleInteractable>();
        audioData = GetComponent<AudioSource>();

        if (interactable != null )
        {
            interactable.selectEntered.AddListener(OnButtonPressed);
        }
    }

    void OnButtonPressed(SelectEnterEventArgs args)
    {
        audioData.Play();
        Instantiate(Plate, new Vector3(6.81647968f, 1.04572999f, -8.03367996f), Quaternion.identity);
    }
}
