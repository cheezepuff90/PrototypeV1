using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementCC : MonoBehaviour
{
    public float speed = 5f;
    public float jump = 2f;
    public float gravity = -9.81f;

    private CharacterController cc;
    private Vector3 velocity;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 1. Handle ground state and vertical reset
        if (cc.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Keeps the character firmly snapped to the ground
        }

        // 2. Gather horizontal input
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = (transform.right * moveX + transform.forward * moveZ) * speed;

        // 3. Handle Jumping
        if (Input.GetButtonDown("Jump") && cc.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jump * -2f * gravity);
        }

        // 4. Apply gravity to vertical velocity
        velocity.y += gravity * Time.deltaTime;

        // 5. Combine horizontal and vertical movement into ONE single Move call
        Vector3 finalMovement = move + new Vector3(0, velocity.y, 0);
        cc.Move(finalMovement * Time.deltaTime);
    }
}