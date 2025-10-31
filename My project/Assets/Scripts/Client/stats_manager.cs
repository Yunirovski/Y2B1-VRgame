using UnityEngine;
using TMPro;

public class GameStatsManager : MonoBehaviour
{
    [Header("UI Display")]
    public TextMeshProUGUI ordersCompletedText;   // Display total completed orders
    public TextMeshProUGUI customersTimedOutText; // Display total timed out customers
    public TextMeshProUGUI timerText_Human;       // Display Human customer timer
    public TextMeshProUGUI timerText_Robot;       // Display Robot customer timer
    public TextMeshProUGUI totalCustomersText;    // Display total customers to spawn

    [Header("References")]
    public QueueManager_Human humanQueueManager;  // Human queue reference
    public QueueManager_Robot robotQueueManager;  // Robot queue reference

    // Global statistics
    public static int totalOrdersCompleted = 0;
    public static int totalCustomersTimedOut = 0;
    public static int customersSpawnedCount = 10;

    private static TextMeshProUGUI staticTimerText_Human;
    private static TextMeshProUGUI staticTimerText_Robot;

    void Start()
    {
        staticTimerText_Human = timerText_Human;
        staticTimerText_Robot = timerText_Robot;
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
        if (totalCustomersText != null)
        {
            int humanRemaining = humanQueueManager != null ? humanQueueManager.totalCustomersToSpawn - humanQueueManager.customersSpawned : 0;
            int robotRemaining = robotQueueManager != null ? robotQueueManager.totalCustomersToSpawn - robotQueueManager.customersSpawned : 0;
            totalCustomersText.text = "Human: " + humanRemaining + " | Robot: " + robotRemaining;
        }
    }

    // Update Human timer display
    public static void UpdateTimer_Human(float timeRemaining)
    {
        if (staticTimerText_Human != null)
        {
            staticTimerText_Human.text = Mathf.Ceil(timeRemaining).ToString() + "s";
        }
    }

    // Update Robot timer display
    public static void UpdateTimer_Robot(float timeRemaining)
    {
        if (staticTimerText_Robot != null)
        {
            staticTimerText_Robot.text = Mathf.Ceil(timeRemaining).ToString() + "s";
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