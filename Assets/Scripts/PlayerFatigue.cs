using UnityEngine;

public class PlayerFatigue : MonoBehaviour
{
    [Header("Fatigue Settings")]
    public float maxFatigue = 100f;
    public float currentFatigue = 0f;
    public float passiveFatigueRate = 1.5f; // Fatigue gained per second normally
    public float workingFatigueRate = 5f;   // Faster drain while sitting at a task

    [HideInInspector] public bool isWorking = false;

    [Header("Caffeine Protection")]
    public float protectionTimer = 0f;

    [Header("Exhaustion Penalty")]
    public float normalSpeed = 5f;
    public float exhaustedSpeed = 2f;
    private PlayerMovementCC playerMovement;

    // Easy check for other scripts to see if player is exhausted
    public bool IsExhausted => currentFatigue >= maxFatigue;

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
        if (protectionTimer > 0f)
        {
            protectionTimer -= Time.deltaTime;
        }
        else
        {
            // Choose drain rate based on whether the player is currently doing a task
            float activeRate = isWorking ? workingFatigueRate : passiveFatigueRate;
            currentFatigue += activeRate * Time.deltaTime;
            currentFatigue = Mathf.Clamp(currentFatigue, 0f, maxFatigue);
        }

        HandleExhaustion();
    }

    void HandleExhaustion()
    {
        if (playerMovement == null) return;

        playerMovement.speed = IsExhausted ? exhaustedSpeed : normalSpeed;
    }

    public void ApplyCoffeeEffect(float fatigueReduction, float protectionDuration)
    {
        currentFatigue -= fatigueReduction;
        currentFatigue = Mathf.Clamp(currentFatigue, 0f, maxFatigue);
        protectionTimer = protectionDuration;

        Debug.Log($"Fatigue reduced to {Mathf.Round(currentFatigue)}! Protected for {protectionDuration}s.");
    }
}
