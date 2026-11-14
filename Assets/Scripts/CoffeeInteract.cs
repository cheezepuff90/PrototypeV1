using UnityEngine;

public class CoffeeInteract : MonoBehaviour, IInteractable
{
    [Header("Coffee Settings")]
    [SerializeField] private int maxSips = 3;

    [Header("Coffee Effects")]
    [SerializeField] private float fatigueReductionPerSip = 25f;
    [SerializeField] private float protectionSecondsPerSip = 10f;

    private PlayerFatigue playerFatigue;
    private int currentSips;
    private Rigidbody rb;
    private bool isHeld = false;
    private Transform holdPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSips = maxSips;

        // 1. Create the 'player' variable first
        GameObject player = GameObject.FindWithTag("Player");

        // 2. Then check if it exists and grab PlayerFatigue
        if (player != null)
        {
            playerFatigue = player.GetComponent<PlayerFatigue>();
        }
    }

    void Update()
    {
        if (isHeld && holdPoint != null)
        {
            // Smoothly brings the interactable object towards user
            transform.position = Vector3.Lerp(transform.position, holdPoint.position, Time.deltaTime * 15f);
            transform.rotation = Quaternion.Lerp(transform.rotation, holdPoint.rotation, Time.deltaTime * 15f);

            if (Input.GetKeyDown(KeyCode.Q)) Drop();

            if (Input.GetMouseButtonDown(0)) Drink();
        }
    }

    void Drink()
    {
        if (currentSips > 0)
        {
            currentSips--;

            // Actually reduce fatigue and start protection timer
            if (playerFatigue != null)
            {
                playerFatigue.ApplyCoffeeEffect(fatigueReductionPerSip, protectionSecondsPerSip);
            }

            if (currentSips <= 0)
            {
                Debug.Log("Coffee mug empty");
            }
            else
            {
                Debug.Log($"Fatigue Restored! Sips remaining: {currentSips}");
            }
        }
        else
        {
            Debug.Log("Mug is already empty. Press Q to drop it");
        }
    }

    public void interact()
    {
        if (!isHeld) Pickup();
    }

    void Pickup()
    {
        // Used for finding gameobject with holdpoint to grab its transform (position and rotation)
        GameObject holdObj = GameObject.FindWithTag("HoldPoint");
        if (holdObj != null) holdPoint = holdObj.transform;
        else
        {
            Debug.Log("Create Gameobject tagged 'HoldPoint' on your camera!");
            return;
        }

        isHeld = true;
        // This line disables physics for object
        rb.isKinematic = true;
        GetComponent<Collider>().enabled = false;
    }

    void Drop()
    {
        isHeld = false;
        holdPoint = null;

        // Re-enables physics to drop on floor
        rb.isKinematic = false;
        GetComponent<Collider>().enabled = true;
    }

    public void OnHover() { }
    public void OnHoverExit() { }
}
