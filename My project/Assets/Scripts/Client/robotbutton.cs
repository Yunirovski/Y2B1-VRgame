using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(AudioSource))]
public class RobotAcceptOrderButton : MonoBehaviour
{
    [Header("References")]
    public QueueManager_Robot robotQueueManager;

    AudioSource audioData;
    void Start()
    {
        XRSimpleInteractable interactable = GetComponent<XRSimpleInteractable>();
        audioData = GetComponent<AudioSource>();

        if (interactable != null)
        {
            interactable.selectEntered.AddListener(OnButtonPressed);
        }
    }

    void OnButtonPressed(SelectEnterEventArgs args)
    {
        Debug.Log("Robot Accept Order button pressed!");
        audioData.Play();

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