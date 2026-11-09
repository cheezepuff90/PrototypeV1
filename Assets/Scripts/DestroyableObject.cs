using UnityEngine;

public class DestroyableObject : MonoBehaviour, IInteractable
{
    public void interact()
    {
        Destroy(gameObject);
    }

    public void OnHover()
    {
        
    }

    public void OnHoverExit()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
