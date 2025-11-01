using UnityEngine;
using TMPro;

public class DayManager : MonoBehaviour
{
    [Header("Day Settings")]
    public int currentDay = 1;
    public TextMeshProUGUI dayText;  // Display current day

    [Header("Day 1 - Only Robots")]
    public int day1_TotalCustomers = 10;      // Total customers to spawn
    public int day1_HumanPercentage = 0;      // 0% = Only robots
    public int day1_MaxQueueSize = 5;

    [Header("Day 2 - Robots + Humans")]
    public int day2_TotalCustomers = 15;      // Total customers to spawn
    public int day2_HumanPercentage = 33;     // 33% humans, 67% robots
    public int day2_MaxQueueSize = 8;

    [Header("Day 3 - Robots + Humans")]
    public int day3_TotalCustomers = 20;      // Total customers to spawn
    public int day3_HumanPercentage = 60;     // 60% humans, 40% robots
    public int day3_MaxQueueSize = 8;

    [Header("References")]
    public QueueManager_Human humanQueueManager;
    public QueueManager_Robot robotQueueManager;

    // Current day settings
    private int totalCustomersForDay;
    private int humanPercentageForDay;
    private int maxQueueSizeForDay;

    void Start()
    {
        // Initialize day 1
        GameStatsManager.StartNewDay(1, day1_TotalCustomers);
        SetupDay(1);
    }

    void Update()
    {
        // Update day display
        if (dayText != null)
        {
            dayText.text = "Day " + currentDay;
        }

        // Check if all customers have spawned AND all queues are empty
        bool allCustomersSpawned = GameStatsManager.HasReachedCustomerLimit();
        bool queuesEmpty = humanQueueManager.queue.Count == 0 && robotQueueManager.queue.Count == 0;

        if (allCustomersSpawned && queuesEmpty && currentDay < 3)
        {
            Debug.Log($"Day {currentDay} Complete! All {totalCustomersForDay} customers processed.");
            NextDay();
        }
    }

    void SetupDay(int day)
    {
        currentDay = day;

        switch (day)
        {
            case 1:
                totalCustomersForDay = day1_TotalCustomers;
                humanPercentageForDay = day1_HumanPercentage;
                maxQueueSizeForDay = day1_MaxQueueSize;
                break;
            case 2:
                totalCustomersForDay = day2_TotalCustomers;
                humanPercentageForDay = day2_HumanPercentage;
                maxQueueSizeForDay = day2_MaxQueueSize;
                break;
            case 3:
                totalCustomersForDay = day3_TotalCustomers;
                humanPercentageForDay = day3_HumanPercentage;
                maxQueueSizeForDay = day3_MaxQueueSize;
                break;
            default:
                Debug.LogError("Day " + day + " not configured!");
                return;
        }

        // Set queue managers
        humanQueueManager.maxQueueSize = maxQueueSizeForDay;
        robotQueueManager.maxQueueSize = maxQueueSizeForDay;

        // Tell queue managers the human percentage for this day
        humanQueueManager.SetHumanSpawnPercentage(humanPercentageForDay);
        robotQueueManager.SetRobotSpawnPercentage(100 - humanPercentageForDay);

        Debug.Log($"=== DAY {day} START ===");
        Debug.Log($"Total Customers: {totalCustomersForDay}");
        Debug.Log($"Human Percentage: {humanPercentageForDay}%");
        Debug.Log($"Max Queue Size: {maxQueueSizeForDay}");
    }

    // Call this to manually advance to the next day
    public void NextDay()
    {
        if (currentDay >= 3)
        {
            Debug.Log("Game Complete!");
            Debug.Log(GameStatsManager.GetAllStats());
            return;
        }

        currentDay++;
        Debug.Log($"!!! Advancing to DAY {currentDay} !!!");

        // Clear all remaining customers
        ClearAllCustomers();

        // Start new day
        switch (currentDay)
        {
            case 1:
                GameStatsManager.StartNewDay(1, day1_TotalCustomers);
                break;
            case 2:
                GameStatsManager.StartNewDay(2, day2_TotalCustomers);
                break;
            case 3:
                GameStatsManager.StartNewDay(3, day3_TotalCustomers);
                break;
        }

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