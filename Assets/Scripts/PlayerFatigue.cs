using UnityEngine;

public class PlayerFatigue : MonoBehaviour
{
    [Header("Fatigue Settings")]
    public float maxFatigue = 100f;
    public float currentFatigue = 0f;
    public float fatigueIncreaseRate = 2f; // Fatigue gained per second

    [Header("Caffeine Protection")]
    public float protectionTimer = 0f; // While > 0, fatigue stops increasing

    [Header("Exhaustion Penalty")]
    public float normalSpeed = 5f;
    public float exhaustedSpeed = 2f;
    private PlayerMovementCC playerMovement;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovementCC>();
        if (playerMovement != null)
        {
            normalSpeed = playerMovement.speed;
        }
    }

    void Update()
    {
        // 1. Check if caffeine protection is active
        if (protectionTimer > 0f)
        {
            protectionTimer -= Time.deltaTime;
        }
        else
        {
            // 2. Increase fatigue over time when unprotected
            currentFatigue += fatigueIncreaseRate * Time.deltaTime;
            currentFatigue = Mathf.Clamp(currentFatigue, 0f, maxFatigue);
        }

        // 3. Apply slowdown if completely exhausted
        HandleExhaustion();
    }

    void HandleExhaustion()
    {
        if (playerMovement == null) return;

        if (currentFatigue >= maxFatigue)
        {
            playerMovement.speed = exhaustedSpeed;
        }
        else
        {
            playerMovement.speed = normalSpeed;
        }
    }

    // Called by CoffeeInteract when the player takes a sip
    public void ApplyCoffeeEffect(float fatigueReduction, float protectionDuration)
    {
        currentFatigue -= fatigueReduction;
        currentFatigue = Mathf.Clamp(currentFatigue, 0f, maxFatigue);

        // Add or refresh caffeine shield
        protectionTimer = protectionDuration;

        Debug.Log($"Fatigue reduced to {Mathf.Round(currentFatigue)}! Protected for {protectionDuration}s.");
    }
}
