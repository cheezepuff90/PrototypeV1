using UnityEngine;

public class CoffeeMachine : MonoBehaviour, IInteractable
{
    [Header("Brew Settings")]
    [SerializeField] private float brewTime = 2.5f;
    private bool isBrewing = false;
    private float brewTimer = 0f;
    private CoffeeInteract targetMug;

    void Update()
    {
        if (isBrewing)
        {
            brewTimer += Time.deltaTime;
            Debug.Log($"Brewing coffee... {Mathf.Round((brewTimer / brewTime) * 100f)}%");

            if (brewTimer >= brewTime)
            {
                FinishBrewing();
            }
        }
    }

    public void interact()
    {
        if (isBrewing)
        {
            Debug.Log("Machine is already brewing...");
            return;
        }

        // 1. Find the HoldPoint on the player
        GameObject holdObj = GameObject.FindWithTag("HoldPoint");
        if (holdObj == null) return;

        // 2. Check all CoffeeInteract objects to see which one is currently held
        CoffeeInteract[] allMugs = FindObjectsByType<CoffeeInteract>(FindObjectsSortMode.None);
        CoffeeInteract heldMug = null;

        foreach (CoffeeInteract mug in allMugs)
        {
            if (mug.IsHeld())
            {
                heldMug = mug;
                break;
            }
        }

        // 3. Validate that the player is holding a mug that isn't already full
        if (heldMug == null)
        {
            Debug.Log("You need to hold a coffee mug to use the machine!");
            return;
        }

        targetMug = heldMug;
        StartBrewing();
    }

    void StartBrewing()
    {
        isBrewing = true;
        brewTimer = 0f;
        Debug.Log("Started brewing coffee...");
    }

    void FinishBrewing()
    {
        isBrewing = false;

        if (targetMug != null)
        {
            targetMug.Refill();
            targetMug = null;
        }
    }

    public void OnHover() { }
    public void OnHoverExit() { }
}
