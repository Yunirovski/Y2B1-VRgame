using UnityEngine;

public class CustomerTimer : MonoBehaviour
{
    public float waitTime = 10f;  // Wait time in seconds

    private float timer;
    private CustomerOrderSystem customer;
    private bool timerRunning = false;
    private bool counted = false;
    private bool wasAtCounter = false;

    void Start()
    {
        customer = GetComponent<CustomerOrderSystem>();
        timer = waitTime;
        Debug.Log("=== NEW CUSTOMER SPAWNED ===");
    }

    void Update()
    {
        if (customer == null)
        {
            Debug.LogError("Customer is NULL!");
            return;
        }

        if (counted)
        {
            return;  // Already counted this customer
        }

        // Debug current state
        if (Time.frameCount % 60 == 0)  // Every 60 frames
        {
            Debug.Log($"State: isAtCounter={customer.isAtCounter}, hasReceivedOrder={customer.hasReceivedOrder}, timerRunning={timerRunning}, timer={timer:F1}");
        }

        // Detect when customer arrives at counter
        if (customer.isAtCounter && !wasAtCounter)
        {
            wasAtCounter = true;
            timerRunning = true;
            timer = waitTime;
            Debug.Log(">>> TIMER STARTED: " + waitTime + "s");
        }

        // Timer is running
        if (timerRunning)
        {
            timer -= Time.deltaTime;

            // 根据顾客类型更新对应的 timer
            if (customer.customerType == CustomerOrderSystem.CustomerType.Human)
                GameStatsManager.UpdateTimer_Human(timer);
            else
                GameStatsManager.UpdateTimer_Robot(timer);

            // Check if order received
            if (customer.hasReceivedOrder)
            {
                Debug.Log($"!!! hasReceivedOrder is TRUE, timer={timer:F1}");

                if (timer > 0)
                {
                    Debug.Log(">>> CALLING AddCompletedOrder()");
                    GameStatsManager.AddCompletedOrder();
                    counted = true;
                    timerRunning = false;
                    Debug.Log($"✓✓✓ ORDER COMPLETED! Total now: {GameStatsManager.totalOrdersCompleted}");
                }
                else
                {
                    Debug.Log(">>> Order received but timer already at 0");
                }
                return;
            }

            // Time ran out
            if (timer <= 0)
            {
                Debug.Log(">>> TIMER EXPIRED - Customer timed out");
                GameStatsManager.AddTimedOutCustomer();
                customer.hasReceivedOrder = true;
                counted = true;
                timerRunning = false;
                Debug.Log($"✗✗✗ TIMED OUT! Total now: {GameStatsManager.totalCustomersTimedOut}");
            }
        }
    }
}