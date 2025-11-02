using UnityEngine;

public class TVScreenManager : MonoBehaviour
{
    [Header("Screen Objects")]
    public GameObject goobScreen;
    public GameObject burgerScreen;
    public GameObject pizzaScreen;
    public GameObject blueScreen;

    [Header("Queue Manager")]
    public MonoBehaviour queueManager;

    private bool orderActive = false;

    void Start()
    {
        ShowBlueScreen();
    }

    void Update()
    {
        CustomerOrderSystem customer = GetCurrentCustomer();

        if (customer != null && customer.isAtCounter)
        {
            // Order timer started - show order screen
            if (customer.orderTimerActive && !orderActive)
            {
                orderActive = true;
                ShowOrderScreen(customer.orderType);
            }
            // Order ended - back to blue screen
            else if (!customer.orderTimerActive && orderActive)
            {
                orderActive = false;
                ShowBlueScreen();
            }
        }
        else
        {
            // No customer at counter
            if (orderActive)
            {
                orderActive = false;
                ShowBlueScreen();
            }
        }
    }

    void ShowOrderScreen(string orderType)
    {
        HideAllScreens();

        switch (orderType.ToLower())
        {
            case "cooked":
                goobScreen.SetActive(true);
                break;
            case "burger":
                burgerScreen.SetActive(true);
                break;
            case "pizza":
                pizzaScreen.SetActive(true);
                break;
            default:
                ShowBlueScreen();
                break;
        }
    }

    void ShowBlueScreen()
    {
        HideAllScreens();
        blueScreen.SetActive(true);
    }

    void HideAllScreens()
    {
        goobScreen.SetActive(false);
        burgerScreen.SetActive(false);
        pizzaScreen.SetActive(false);
        blueScreen.SetActive(false);
    }

    // Get current customer at counter
    CustomerOrderSystem GetCurrentCustomer()
    {
        if (queueManager is QueueManager_Human humanQueue)
            return humanQueue.GetCurrentCustomer();

        if (queueManager is QueueManager_Robot robotQueue)
            return robotQueue.GetCurrentCustomer();

        return null;
    }
}