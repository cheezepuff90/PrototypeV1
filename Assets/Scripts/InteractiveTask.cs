using UnityEngine;

public class InteractiveTask : MonoBehaviour, IInteractable
{
    [Header("Task Settings")]
    [SerializeField] private float maxProgress = 100f;
    private float currentProgress = 0f;
    private bool isDoingTask = false;

    [Header("Vulnerability & Positioning")]
    [SerializeField] private Transform playerTransform; // Drag your Player object here
    [SerializeField] private float taskHeightOffset = -0.5f; // Lowers player position to look vulnerable/crouched
    private Vector3 originalPlayerScale;

    void Start()
    {
        // If you didn't assign it manually, find the player by tag
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null) playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (isDoingTask)
        {
            // Fill the progress bar over time (e.g., holding down the action)
            currentProgress += Time.deltaTime * 20f; // Adjust 20f to make it faster/slower

            // Check if task is completed
            if (currentProgress >= maxProgress)
            {
                currentProgress = maxProgress;
                CompleteTask();
            }

            // Abort task if player presses Escape or moves away
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                AbortTask();
            }

            // Debug log to test the progress bar filling up
            Debug.Log($"Task Progress: {Mathf.Round(currentProgress)}%");
        }
    }

    public void interact()
    {
        if (!isDoingTask && currentProgress < maxProgress)
        {
            StartTask();
        }
        else if (isDoingTask)
        {
            AbortTask();
        }
    }

    void StartTask()
    {
        isDoingTask = true;
        
        // Put player in a vulnerable/lowered position
        if (playerTransform != null)
        {
            // Simple vulnerability cue: drop player position slightly
            playerTransform.position += new Vector3(0, taskHeightOffset, 0);
            
            // Optional: Disable player movement script here so they can't run away while vulnerable
            playerTransform.GetComponent<PlayerMovementCC>().enabled = false;
        }

        Debug.Log("Started task... Vulnerable state active.");
    }

    void AbortTask()
    {
        isDoingTask = false;

        // Stand the player back up
        if (playerTransform != null)
        {
            playerTransform.position -= new Vector3(0, taskHeightOffset, 0);
            
            // Re-enable player movement
            playerTransform.GetComponent<PlayerMovementCC>().enabled = true;
        }

        Debug.Log($"Task aborted! Progress saved at {Mathf.Round(currentProgress)}%. Standing back up.");
    }

    void CompleteTask()
    {
        isDoingTask = false;

        // Stand back up on completion
        if (playerTransform != null)
        {
            playerTransform.position -= new Vector3(0, taskHeightOffset, 0);
            playerTransform.GetComponent<PlayerMovementCC>().enabled = true;
        }

        Debug.Log("Task Completed!");
        // Add reward, open door, or trigger event here
    }

    public void OnHover() { }
    public void OnHoverExit() { }
}