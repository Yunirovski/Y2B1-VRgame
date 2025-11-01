using UnityEngine;
using TMPro;

public class GameStatsManager : MonoBehaviour
{
    [Header("UI Display - Current Day")]
    public TextMeshProUGUI humanOrdersText_Today;      // Display human orders TODAY
    public TextMeshProUGUI robotOrdersText_Today;      // Display robot orders TODAY
    public TextMeshProUGUI timedOutText_Today;         // Display timed out customers TODAY
    public TextMeshProUGUI timerText_Human;            // Display Human customer timer
    public TextMeshProUGUI timerText_Robot;            // Display Robot customer timer

    [Header("UI Display - Progress")]
    public TextMeshProUGUI customerProgressText;       // Display "Customers: 5/10"

    [Header("UI Display - Total")]
    public TextMeshProUGUI humanOrdersText_Total;      // Display human orders TOTAL
    public TextMeshProUGUI robotOrdersText_Total;      // Display robot orders TOTAL
    public TextMeshProUGUI timedOutText_Total;         // Display timed out customers TOTAL

    [Header("UI Display - Day Breakdown")]
    public TextMeshProUGUI day1StatsText;              // Day 1 stats
    public TextMeshProUGUI day2StatsText;              // Day 2 stats
    public TextMeshProUGUI day3StatsText;              // Day 3 stats

    // Today's statistics (static)
    public static int humanOrdersCompleted_Today = 0;
    public static int robotOrdersCompleted_Today = 0;
    public static int totalCustomersTimedOut_Today = 0;
    public static int totalCustomersServed_Today = 0;  // Total customers spawned today

    // Total statistics across all days (static)
    public static int humanOrdersCompleted_Total = 0;
    public static int robotOrdersCompleted_Total = 0;
    public static int totalCustomersTimedOut_Total = 0;

    // Day-by-day breakdown (static)
    public static int day1_HumanOrders = 0;
    public static int day1_RobotOrders = 0;
    public static int day1_TimedOut = 0;
    public static int day1_Total = 0;

    public static int day2_HumanOrders = 0;
    public static int day2_RobotOrders = 0;
    public static int day2_TimedOut = 0;
    public static int day2_Total = 0;

    public static int day3_HumanOrders = 0;
    public static int day3_RobotOrders = 0;
    public static int day3_TimedOut = 0;
    public static int day3_Total = 0;

    private static TextMeshProUGUI staticTimerText_Human;
    private static TextMeshProUGUI staticTimerText_Robot;

    private static int currentDay = 1;
    private static int totalCustomersLimit_Today = 10; // How many customers max for today

    void Start()
    {
        staticTimerText_Human = timerText_Human;
        staticTimerText_Robot = timerText_Robot;
    }

    void Update()
    {
        // Update today's UI
        if (humanOrdersText_Today != null)
        {
            humanOrdersText_Today.text = "Human (Today): " + humanOrdersCompleted_Today;
        }

        if (robotOrdersText_Today != null)
        {
            robotOrdersText_Today.text = "Robot (Today): " + robotOrdersCompleted_Today;
        }

        if (timedOutText_Today != null)
        {
            timedOutText_Today.text = "Timed Out (Today): " + totalCustomersTimedOut_Today;
        }

        // Update progress
        if (customerProgressText != null)
        {
            customerProgressText.text = $"Customers: {totalCustomersServed_Today}/{totalCustomersLimit_Today}";
        }

        // Update total UI
        if (humanOrdersText_Total != null)
        {
            humanOrdersText_Total.text = "Human (Total): " + humanOrdersCompleted_Total;
        }

        if (robotOrdersText_Total != null)
        {
            robotOrdersText_Total.text = "Robot (Total): " + robotOrdersCompleted_Total;
        }

        if (timedOutText_Total != null)
        {
            timedOutText_Total.text = "Timed Out (Total): " + totalCustomersTimedOut_Total;
        }

        // Update day breakdown UI
        if (day1StatsText != null)
        {
            day1StatsText.text = $"Day 1: Human {day1_HumanOrders} | Robot {day1_RobotOrders} | Timed Out {day1_TimedOut} (Total: {day1_Total})";
        }

        if (day2StatsText != null)
        {
            day2StatsText.text = $"Day 2: Human {day2_HumanOrders} | Robot {day2_RobotOrders} | Timed Out {day2_TimedOut} (Total: {day2_Total})";
        }

        if (day3StatsText != null)
        {
            day3StatsText.text = $"Day 3: Human {day3_HumanOrders} | Robot {day3_RobotOrders} | Timed Out {day3_TimedOut} (Total: {day3_Total})";
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

    // Check if we've reached the customer limit for today
    public static bool HasReachedCustomerLimit()
    {
        return totalCustomersServed_Today >= totalCustomersLimit_Today;
    }

    // Get remaining customer slots for today
    public static int GetRemainingCustomerSlots()
    {
        return Mathf.Max(0, totalCustomersLimit_Today - totalCustomersServed_Today);
    }

    // Register a new customer spawned
    public static void RegisterCustomerSpawned()
    {
        totalCustomersServed_Today++;
        Debug.Log($"Customer spawned. Progress: {totalCustomersServed_Today}/{totalCustomersLimit_Today}");
    }

    // Call this when a human order is completed
    public static void AddHumanOrder()
    {
        humanOrdersCompleted_Today++;
        humanOrdersCompleted_Total++;

        // Add to the current day's breakdown
        switch (currentDay)
        {
            case 1:
                day1_HumanOrders++;
                break;
            case 2:
                day2_HumanOrders++;
                break;
            case 3:
                day3_HumanOrders++;
                break;
        }
    }

    // Call this when a robot order is completed
    public static void AddRobotOrder()
    {
        robotOrdersCompleted_Today++;
        robotOrdersCompleted_Total++;

        // Add to the current day's breakdown
        switch (currentDay)
        {
            case 1:
                day1_RobotOrders++;
                break;
            case 2:
                day2_RobotOrders++;
                break;
            case 3:
                day3_RobotOrders++;
                break;
        }
    }

    // Call this when a customer times out
    public static void AddTimedOutCustomer()
    {
        totalCustomersTimedOut_Today++;
        totalCustomersTimedOut_Total++;

        // Add to the current day's breakdown
        switch (currentDay)
        {
            case 1:
                day1_TimedOut++;
                break;
            case 2:
                day2_TimedOut++;
                break;
            case 3:
                day3_TimedOut++;
                break;
        }
    }

    // Call this when moving to a new day (from DayManager)
    public static void StartNewDay(int dayNumber, int customerLimit)
    {
        currentDay = dayNumber;
        totalCustomersLimit_Today = customerLimit;

        // Reset TODAY's statistics but keep TOTAL statistics
        humanOrdersCompleted_Today = 0;
        robotOrdersCompleted_Today = 0;
        totalCustomersTimedOut_Today = 0;
        totalCustomersServed_Today = 0;

        Debug.Log($"=== Started Day {dayNumber} ===");
        Debug.Log($"Customer Limit: {customerLimit}");
        Debug.Log($"Total so far - Human: {humanOrdersCompleted_Total}, Robot: {robotOrdersCompleted_Total}, Timed Out: {totalCustomersTimedOut_Total}");
    }

    // Get all stats as a formatted string
    public static string GetAllStats()
    {
        string stats = "=== FINAL STATS ===\n";
        stats += $"Day 1: Human {day1_HumanOrders} | Robot {day1_RobotOrders} | Timed Out {day1_TimedOut} | Total Served: {day1_Total}\n";
        stats += $"Day 2: Human {day2_HumanOrders} | Robot {day2_RobotOrders} | Timed Out {day2_TimedOut} | Total Served: {day2_Total}\n";
        stats += $"Day 3: Human {day3_HumanOrders} | Robot {day3_RobotOrders} | Timed Out {day3_TimedOut} | Total Served: {day3_Total}\n";
        stats += $"\nTOTAL: Human {humanOrdersCompleted_Total} | Robot {robotOrdersCompleted_Total} | Timed Out {totalCustomersTimedOut_Total}";
        return stats;
    }
}