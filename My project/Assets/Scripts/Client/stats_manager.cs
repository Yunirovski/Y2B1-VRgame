using UnityEngine;
using TMPro;

public class GameStatsManager : MonoBehaviour
{
    [Header("UI Display")]
    public TextMeshProUGUI ordersCompletedText;   // Display total completed orders
    public TextMeshProUGUI customersTimedOutText; // Display total timed out customers
    public TextMeshProUGUI timerText;             // Display current customer timer
    public TextMeshProUGUI totalCustomersText;  // Display total customers to spawn

    [Header("References")]
    public QueueManager queueManager;             // Drag your QueueManager here!

    // Global statistics
    public static int totalOrdersCompleted = 0;
    public static int totalCustomersTimedOut = 0;
    public static int customersSpawnedCount = 10;

    private static TextMeshProUGUI staticTimerText;

    void Start()
    {
        staticTimerText = timerText;
    }

    void Update()
    {
        // Update UI text
        if (ordersCompletedText != null)
        {
            ordersCompletedText.text = "Orders Completed: " + totalOrdersCompleted;
        }

        if (customersTimedOutText != null)
        {
            customersTimedOutText.text = "Timed Out: " + totalCustomersTimedOut;
        }
        if (totalCustomersText != null && queueManager != null)
        {
            int remaining = queueManager.totalCustomersToSpawn - queueManager.customersSpawned;
            totalCustomersText.text = "Remaining: " + remaining;
        }
    }

    // Update timer display
    public static void UpdateTimer(float timeRemaining)
    {
        if (staticTimerText != null)
        {
            staticTimerText.text = Mathf.Ceil(timeRemaining).ToString() + "s";
        }
    }

    // Call this to add a completed order
    public static void AddCompletedOrder()
    {
        totalOrdersCompleted++;
    }

    // Call this to add a timed out customer
    public static void AddTimedOutCustomer()
    {
        totalCustomersTimedOut++;
    }

    // Reset stats (optional, for restarting game)
    public static void ResetStats()
    {
        totalOrdersCompleted = 0;
        totalCustomersTimedOut = 0;
    }
}