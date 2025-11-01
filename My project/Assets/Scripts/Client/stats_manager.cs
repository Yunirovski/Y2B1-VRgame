using UnityEngine;
using TMPro;

public class GameStatsManager : MonoBehaviour
{
    [Header("UI Display")]
    public TextMeshProUGUI humanOrdersCompletedText;   // Display human completed orders
    public TextMeshProUGUI robotOrdersCompletedText;   // Display robot completed orders
    public TextMeshProUGUI customersTimedOutText;      // Display total timed out customers
    public TextMeshProUGUI timerText_Human;            // Display Human customer timer
    public TextMeshProUGUI timerText_Robot;            // Display Robot customer timer

    // Global statistics (static so they persist)
    public static int humanOrdersCompleted = 0;
    public static int robotOrdersCompleted = 0;
    public static int totalCustomersTimedOut = 0;

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
        if (humanOrdersCompletedText != null)
        {
            humanOrdersCompletedText.text = "Human Orders: " + humanOrdersCompleted;
        }

        if (robotOrdersCompletedText != null)
        {
            robotOrdersCompletedText.text = "Robot Orders: " + robotOrdersCompleted;
        }

        if (customersTimedOutText != null)
        {
            customersTimedOutText.text = "Timed Out: " + totalCustomersTimedOut;
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

    // Call this to add a timed out customer
    public static void AddTimedOutCustomer()
    {
        totalCustomersTimedOut++;
    }

    // Reset stats (optional, for restarting game)
    public static void ResetStats()
    {
        humanOrdersCompleted = 0;
        robotOrdersCompleted = 0;
        totalCustomersTimedOut = 0;
    }
}