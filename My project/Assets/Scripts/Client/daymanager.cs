using UnityEngine;
using TMPro;

public class DayManager : MonoBehaviour
{
    [Header("Day Settings")]
    public int currentDay = 1;
    public TextMeshProUGUI dayText;  // Display current day

    [Header("Day 1 - Only Robots")]
    public int day1_RobotsTarget = 10;
    public int day1_HumansTarget = 0;
    public int day1_MaxCustomers = 5;

    [Header("Day 2 - Robots + Humans")]
    public int day2_RobotsTarget = 8;
    public int day2_HumansTarget = 5;
    public int day2_MaxCustomers = 8;

    [Header("Day 3 - Robots + Humans")]
    public int day3_RobotsTarget = 5;
    public int day3_HumansTarget = 10;
    public int day3_MaxCustomers = 8;

    [Header("References")]
    public QueueManager_Human humanQueueManager;
    public QueueManager_Robot robotQueueManager;

    // Current day targets
    private int humanOrdersTarget;
    private int robotOrdersTarget;
    private int maxCustomersPerQueue;

    void Start()
    {
        SetupDay(currentDay);
    }

    void Update()
    {
        // Update day display
        if (dayText != null)
        {
            dayText.text = "Day " + currentDay;
        }

        // Check if day is complete
        int humanOrdersCompleted = GameStatsManager.humanOrdersCompleted;
        int robotOrdersCompleted = GameStatsManager.robotOrdersCompleted;

        bool humanTargetMet = humanOrdersCompleted >= humanOrdersTarget;
        bool robotTargetMet = robotOrdersCompleted >= robotOrdersTarget;
        bool queuesEmpty = humanQueueManager.queue.Count == 0 && robotQueueManager.queue.Count == 0;

        if (humanTargetMet && robotTargetMet && queuesEmpty)
        {
            NextDay();
        }
    }

    void SetupDay(int day)
    {
        switch (day)
        {
            case 1:
                humanOrdersTarget = day1_HumansTarget;
                robotOrdersTarget = day1_RobotsTarget;
                maxCustomersPerQueue = day1_MaxCustomers;
                break;
            case 2:
                humanOrdersTarget = day2_HumansTarget;
                robotOrdersTarget = day2_RobotsTarget;
                maxCustomersPerQueue = day2_MaxCustomers;
                break;
            case 3:
                humanOrdersTarget = day3_HumansTarget;
                robotOrdersTarget = day3_RobotsTarget;
                maxCustomersPerQueue = day3_MaxCustomers;
                break;
            default:
                Debug.LogError("Day " + day + " not configured!");
                return;
        }

        // Set queue managers
        humanQueueManager.maxQueueSize = maxCustomersPerQueue;
        humanQueueManager.orderTargetCount = humanOrdersTarget;

        robotQueueManager.maxQueueSize = maxCustomersPerQueue;
        robotQueueManager.orderTargetCount = robotOrdersTarget;

        Debug.Log($"=== DAY {day} START ===");
        Debug.Log($"Targets - Humans: {humanOrdersTarget}, Robots: {robotOrdersTarget}");
        Debug.Log($"Max queue size: {maxCustomersPerQueue}");
    }

    void NextDay()
    {
        currentDay++;
        Debug.Log($"!!! DAY {currentDay - 1} COMPLETE !!!");
        Debug.Log($"Human Orders: {GameStatsManager.humanOrdersCompleted}, Robot Orders: {GameStatsManager.robotOrdersCompleted}");

        // Clear all remaining customers
        ClearAllCustomers();

        SetupDay(currentDay);
    }

    void ClearAllCustomers()
    {
        foreach (var customer in humanQueueManager.queue)
        {
            if (customer != null)
                Destroy(customer.gameObject);
        }
        humanQueueManager.queue.Clear();

        foreach (var customer in robotQueueManager.queue)
        {
            if (customer != null)
                Destroy(customer.gameObject);
        }
        robotQueueManager.queue.Clear();

        Debug.Log("All remaining customers cleared.");
    }
}