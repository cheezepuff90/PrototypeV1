using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoorScript
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Collider))]
    public class Door : MonoBehaviour, IInteractable
    {
        public bool open;
        public float smooth = 1.0f;
        float DoorOpenAngle = -90.0f;
        float DoorCloseAngle = 0.0f;
        public AudioSource asource;
        public AudioClip openDoor, closeDoor;

        [Header("The Hinge Pivot")]
        [SerializeField] private Transform hingeParent; // Drag the parent DoorHinge here (or leave empty to auto-use parent)

        void Start () 
        {
            asource = GetComponent<AudioSource>();
            
            // If you forgot to assign it, automatically use the parent object as the hinge
            if (hingeParent == null)
            {
                hingeParent = transform.parent;
            }
        }
        
        void Update () 
        {
            if (hingeParent == null) return;

            // Rotates the Hinge Parent instead of the mesh itself
            if (open)
            {
                var target = Quaternion.Euler (0, DoorOpenAngle, 0);
                hingeParent.localRotation = Quaternion.Slerp(hingeParent.localRotation, target, Time.deltaTime * 5 * smooth);
            }
            else
            {
                var target1 = Quaternion.Euler (0, DoorCloseAngle, 0);
                hingeParent.localRotation = Quaternion.Slerp(hingeParent.localRotation, target1, Time.deltaTime * 5 * smooth);
            }  
        }

        public void OpenDoor()
        {
            open = !open;
            asource.clip = open ? openDoor : closeDoor;
            asource.Play();
        }

        public void interact()
        {
            OpenDoor();
        }

        public void OnHover() { }
        public void OnHoverExit() { }
    }
}