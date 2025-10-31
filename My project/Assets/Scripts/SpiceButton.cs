using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SpiceButton : MonoBehaviour
{
    public GameObject SpicePlate;
    private bool unga = false;
    private GameObject plateog;
    Vector3 PlatePos;
    Quaternion PlateRot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        XRSimpleInteractable interactable = GetComponent<XRSimpleInteractable>();

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
        if (other.CompareTag("GoobMicro"))
        {
            Debug.Log("OBJECT ENTER");
            unga = true;
            plateog = other.gameObject;
            PlatePos = other.transform.position;
            PlateRot = other.transform.rotation;
        }
    }

    void OnButtonPressed(SelectEnterEventArgs args)
    {
        if (unga)
        {
            Destroy(plateog);
            Instantiate(SpicePlate, PlatePos, PlateRot);
            unga = false;
        }
    }
}
