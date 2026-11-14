using UnityEngine;

public class InteractiveTask : MonoBehaviour, IInteractable
{
    [Header("Task Settings")]
    [SerializeField] private float maxProgress = 100f;
    [SerializeField] private float normalFillSpeed = 20f;
    [SerializeField] private float exhaustedFillSpeed = 8f; // Slower progress when exhausted
    private float currentProgress = 0f;
    private bool isDoingTask = false;

    [Header("Vulnerability & Positioning")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float taskHeightOffset = -0.5f;
    private PlayerFatigue playerFatigue;

    void Start()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null) playerTransform = player.transform;
        }

        if (playerTransform != null)
        {
            playerFatigue = playerTransform.GetComponent<PlayerFatigue>();
        }
    }

    void Update()
    {
        if (isDoingTask)
        {
            // Check if player is exhausted to determine how fast the task progresses
            float currentSpeed = normalFillSpeed;
            if (playerFatigue != null && playerFatigue.IsExhausted)
            {
                currentSpeed = exhaustedFillSpeed;
            }

            currentProgress += Time.deltaTime * currentSpeed;

            if (currentProgress >= maxProgress)
            {
                currentProgress = maxProgress;
                CompleteTask();
                return;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                AbortTask();
            }

            Debug.Log($"Task Progress: {Mathf.Round(currentProgress)}% | Speed: {currentSpeed}");
        }
    }

    public void interact()
    {
        if (!isDoingTask && currentProgress < maxProgress)
        {
            StartTask();
        }
    }

    void StartTask()
    {
        isDoingTask = true;

        if (playerFatigue != null) playerFatigue.isWorking = true;

        if (playerTransform != null)
        {
            playerTransform.position += new Vector3(0, taskHeightOffset, 0);
            playerTransform.GetComponent<PlayerMovementCC>().enabled = false;
        }

        Debug.Log("Started task... Working drains fatigue faster!");
    }

    void AbortTask()
    {
        isDoingTask = false;

        if (playerFatigue != null) playerFatigue.isWorking = false;

        if (playerTransform != null)
        {
            playerTransform.position -= new Vector3(0, taskHeightOffset, 0);
            playerTransform.GetComponent<PlayerMovementCC>().enabled = true;
        }

        Debug.Log($"Task aborted! Progress saved at {Mathf.Round(currentProgress)}%.");
    }

    void CompleteTask()
    {
        isDoingTask = false;

        if (playerFatigue != null) playerFatigue.isWorking = false;

        if (playerTransform != null)
        {
            playerTransform.position -= new Vector3(0, taskHeightOffset, 0);
            playerTransform.GetComponent<PlayerMovementCC>().enabled = true;
        }

        Debug.Log("Task Completed!");
    }

    public void OnHover() { }
    public void OnHoverExit() { }
}
