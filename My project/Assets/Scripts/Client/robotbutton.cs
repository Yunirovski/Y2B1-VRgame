using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class RobotAcceptOrderButton : MonoBehaviour
{
    [Header("References")]
    public QueueManager_Robot robotQueueManager;

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
        Debug.Log("Robot Accept Order button pressed!");

        CustomerOrderSystem currentCustomer = robotQueueManager.GetCurrentCustomer();

        if (currentCustomer != null && currentCustomer.isAtCounter && !currentCustomer.hasReceivedOrder)
        {
            Debug.Log($"Accepting robot customer order: {currentCustomer.orderType}");
            currentCustomer.StartOrderTimer();
        }
        else
        {
            Debug.LogWarning("No valid robot customer at counter!");
        }
    }
}