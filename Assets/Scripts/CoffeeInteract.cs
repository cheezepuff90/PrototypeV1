using Unity.Mathematics;
using UnityEngine;

public class CoffeeInteract : MonoBehaviour, IInteractable
{
    private Rigidbody rb;
    private bool isHeld = false;
    private Transform holdPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(isHeld && holdPoint != null)
        {
            //Smoothly brings the interactable object towards user
            transform.position = Vector3.Lerp(transform.position, holdPoint.position, Time.deltaTime * 15f);
            transform.rotation = Quaternion.Lerp(transform.rotation, holdPoint.rotation, Time.deltaTime * 15f);

            if(Input.GetKeyDown(KeyCode.Q)) Drop();
        }
    }
    public void interact()
    {
        if (!isHeld) Pickup();
    }

    public void OnHover()
    {
        
    }

    public void OnHoverExit()
    {
        
    }

    void Pickup()
    {
        //Used for finding gameobject with holdpoint to grab its transfor(position and rotation)
        GameObject holdObj = GameObject.FindWithTag("HoldPoint");
        if(holdObj != null) holdPoint = holdObj.transform;
        else 
        {
            Debug.Log("Create Gameobject tagged 'HoldPoint' on your camera!");
            return;
        }

        isHeld = true;
        //This line disables physics for object
        rb.isKinematic = true;
        GetComponent<Collider>().enabled = false;
    }

    void Drop()
    {
        isHeld = false;
        holdPoint = null;

        //re-enables physics to drop on floor
        rb.isKinematic = false;
        GetComponent<Collider>().enabled = true;
    }
    
    
}
