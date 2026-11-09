using UnityEngine;

public interface IInteractable
{
    void interact(); // Keep lowercase to match implementation or capitalize both
    void OnHover();
    void OnHoverExit();
}

public class InteractionSystem : MonoBehaviour
{
    public Camera cam;
    public float distance = 3f; // Added a default distance
    public Color hoverColor = Color.green;

    public IInteractable current;
    public Renderer r;
    private Color originalColor;

    void Update()
    {
        RaycastHit hit;
        
        // Raycast hits an interactable object
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, distance) && hit.collider.TryGetComponent(out IInteractable i))
        {
            if (current != i)
            {
                Clear();
                current = i;
                current.OnHover(); // Added parentheses

                r = hit.collider.GetComponent<Renderer>();
                if (r)
                {
                    originalColor = r.material.color;
                    r.material.color = hoverColor;
                }
            }
        }
        else
        {
            // If we aren't looking at anything interactable, clear current
            Clear();
        }

        // Press 'E' to interact (added a null check so it doesn't crash)
        if (Input.GetKeyDown(KeyCode.E) && current != null)
        {
            current.interact(); // Matches the lowercase 'interact' in your interface
        }
    }

    void Clear()
    {
        if (current != null) 
        {
            current.OnHoverExit();
        }
        
        if (r) 
        {
            r.material.color = originalColor; // Fixed 'ref' typo to 'r'
        }
        
        current = null;
        r = null; // Added missing semicolon
    }
}