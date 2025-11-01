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
    public int day1_Quota = 7;

    [Header("Day 2 - Robots + Humans")]
    public int day2_TotalCustomers = 15;      // Total customers to spawn
    public int day2_HumanPercentage = 33;     // 33% humans, 67% robots
    public int day2_MaxQueueSize = 8;
    public int day2_Quota = 10;

    [Header("Day 3 - Robots + Humans")]
    public int day3_TotalCustomers = 20;      // Total customers to spawn
    public int day3_HumanPercentage = 60;     // 60% humans, 40% robots
    public int day3_MaxQueueSize = 8;
    public int day3_quota = 15;

    [Header("References")]
    public QueueManager_Human humanQueueManager;
    public QueueManager_Robot robotQueueManager;

    [Header("Environment")]
    public GameObject corpse;

    // Current day settings
    private int totalCustomersForDay;
    private int humanPercentageForDay;
    private int maxQueueSizeForDay;
    private int quotaForDay;

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
        int served = GameStatsManager.GetCustomersServed();

        if (served >= quotaForDay)
        {
            Invoke(nameof(NextDay), 2f);
        }

        if (allCustomersSpawned && queuesEmpty && currentDay < 3)
        {
            Debug.Log($"Day {currentDay} Complete! All {totalCustomersForDay} customers processed.");
            Invoke(nameof(NextDay), 2f);
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
                quotaForDay = day1_Quota;
                break;
            case 2:
                totalCustomersForDay = day2_TotalCustomers;
                humanPercentageForDay = day2_HumanPercentage;
                maxQueueSizeForDay = day2_MaxQueueSize;
                quotaForDay = day2_Quota;
                break;
            case 3:
                totalCustomersForDay = day3_TotalCustomers;
                humanPercentageForDay = day3_HumanPercentage;
                maxQueueSizeForDay = day3_MaxQueueSize;
                quotaForDay = day3_quota;
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
                Instantiate(corpse, new Vector3(3.75200009f, 0.558000028f, -5.26900005f), new Quaternion(0, 0, 0.707106829f, 0.707106829f));
                Instantiate(corpse, new Vector3(4.75200009f, 0.558000028f, -5.79900005f), new Quaternion(0, 0, 0.707106829f, 0.707106829f));

                break;
            case 3:
                GameStatsManager.StartNewDay(3, day3_TotalCustomers);
                Instantiate(corpse, new Vector3(8.7840004f, 0.586660028f, 1.41900003f), new Quaternion(0, 0, 0.707106829f, 0.707106829f));
                Instantiate(corpse, new Vector3(4.60200024f, 0.586660028f, 1.41900003f), new Quaternion(0, 0, 0.707106829f, 0.707106829f));
                Instantiate(corpse, new Vector3(10.8330002f, 0.586660028f, 1.41900003f), new Quaternion(0, 0, 0.707106829f, 0.707106829f));
                Instantiate(corpse, new Vector3(9.98200035f, 0.586660028f, 0.127000004f), new Quaternion(0, 0, 0.707106829f, 0.707106829f));
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