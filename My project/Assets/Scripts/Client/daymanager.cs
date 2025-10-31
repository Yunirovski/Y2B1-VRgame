using UnityEngine;
using TMPro;

public class DayManager : MonoBehaviour
{
    [Header("Day Settings")]
    public int currentDay = 1;
    public TextMeshProUGUI dayText;  // Display current day

    [Header("Day 1 - Only Robots")]
    public int day1_Robots = 10;
    public int day1_Humans = 0;

    [Header("Day 2 - Robots + Humans")]
    public int day2_Robots = 8;
    public int day2_Humans = 5;

    [Header("Day 3 - Robots + Humans")]
    public int day3_Robots = 5;
    public int day3_Humans = 10;

    [Header("References")]
    public QueueManager_Human humanQueueManager;
    public QueueManager_Robot robotQueueManager;

    private int humanRemaining;
    private int robotRemaining;

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
        humanRemaining = humanQueueManager.totalCustomersToSpawn - humanQueueManager.customersSpawned;
        robotRemaining = robotQueueManager.totalCustomersToSpawn - robotQueueManager.customersSpawned;

        // If all customers spawned and queues are empty
        if (humanRemaining <= 0 && robotRemaining <= 0 &&
            humanQueueManager.queue.Count == 0 && robotQueueManager.queue.Count == 0)
        {
            NextDay();
        }
    }

    // Setup customer counts for specific day
    void SetupDay(int day)
    {
        switch (day)
        {
            case 1:
                humanQueueManager.totalCustomersToSpawn = day1_Humans;
                robotQueueManager.totalCustomersToSpawn = day1_Robots;
                break;
            case 2:
                humanQueueManager.totalCustomersToSpawn = day2_Humans;
                robotQueueManager.totalCustomersToSpawn = day2_Robots;
                break;
            case 3:
                humanQueueManager.totalCustomersToSpawn = day3_Humans;
                robotQueueManager.totalCustomersToSpawn = day3_Robots;
                break;
        }

        // Reset spawn counters
        humanQueueManager.customersSpawned = 0;
        robotQueueManager.customersSpawned = 0;

        Debug.Log($"=== DAY {day} START ===");
        Debug.Log($"Humans: {humanQueueManager.totalCustomersToSpawn}, Robots: {robotQueueManager.totalCustomersToSpawn}");
    }

    // Move to next day
    void NextDay()
    {
        currentDay++;
        Debug.Log($"!!! DAY {currentDay - 1} COMPLETE !!!");
        SetupDay(currentDay);
    }
}