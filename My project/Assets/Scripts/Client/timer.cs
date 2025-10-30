using UnityEngine;

public class CustomerTimer : MonoBehaviour
{
    public float waitTime = 30f;  // Wait time in seconds

    private float timer;
    private CustomerOrderSystem customer;
    private bool counting = false;
    private bool alreadyCounted = false;  // Prevent double counting

    void Start()
    {
        customer = GetComponent<CustomerOrderSystem>();
        timer = waitTime;
    }

    void Update()
    {
        // Start counting when at counter
        if (customer.isAtCounter && !alreadyCounted)
        {
            if (!counting)
            {
                counting = true;
                timer = waitTime;  // Reset to 30 seconds
            }

            timer -= Time.deltaTime;  // Count down
            GameStatsManager.UpdateTimer(timer);  // Show on UI

            // Check if received order (SUCCESS - count immediately)
            if (customer.hasReceivedOrder)
            {
                GameStatsManager.AddCompletedOrder();
                alreadyCounted = true;
                counting = false;
                return;  // Stop here, don't check timeout
            }

            // Time's up (TIMEOUT)
            if (timer <= 0)
            {
                GameStatsManager.AddTimedOutCustomer();
                customer.hasReceivedOrder = true;  // Force leave
                alreadyCounted = true;
                counting = false;
            }
        }
    }
}