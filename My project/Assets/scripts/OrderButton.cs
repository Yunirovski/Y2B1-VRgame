using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class OrderCompleteButton : MonoBehaviour
{
    public Customer customer; // Drag your customer object here

    void Start()
    {
        // Get the XR Simple Interactable component
        XRSimpleInteractable interactable = GetComponent<XRSimpleInteractable>();

        if (interactable != null)
        {
            // Add listener for select (poke) event
            interactable.selectEntered.AddListener(OnButtonPressed);
        }
    }

    void OnButtonPressed(SelectEnterEventArgs args)
    {
        if (customer != null)
        {
            customer.CompleteOrder();
            Debug.Log("Order completed!");
        }
    }
}