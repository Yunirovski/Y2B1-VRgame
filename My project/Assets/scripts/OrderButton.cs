using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class OrderCompleteButton : MonoBehaviour
{
    public Customer customer;

    void Start()
    {
        GetComponent<XRSimpleInteractable>().selectEntered.AddListener(OnButtonPress);
    }

    void OnButtonPress(SelectEnterEventArgs args)
    {
        if (customer != null)
        {
            customer.CompleteOrder();
        }
    }
}