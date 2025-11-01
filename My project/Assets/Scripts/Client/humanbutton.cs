using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class HumanAcceptOrderButton : MonoBehaviour
{
    [Header("References")]
    public QueueManager_Human humanQueueManager;

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
        Debug.Log("Human Accept Order button pressed!");

        CustomerOrderSystem currentCustomer = humanQueueManager.GetCurrentCustomer();

        if (currentCustomer != null && currentCustomer.isAtCounter && !currentCustomer.hasReceivedOrder)
        {
            Debug.Log($"Accepting human customer order: {currentCustomer.orderType}");
            currentCustomer.StartOrderTimer();
        }
        else
        {
            Debug.LogWarning("No valid human customer at counter!");
        }
    }
}