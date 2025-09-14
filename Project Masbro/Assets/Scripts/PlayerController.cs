using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movSpeed = 10.0f;
    [SerializeField] private float runSpeed = 20.0f;
    [SerializeField] private float airSpeed = 4.0f;
    [SerializeField] private float normalJumpForce = 5.0f;
    [SerializeField] private float runJumpForce = 8.0f;

    public BoxCollider2D standingCollider;
    public BoxCollider2D crouchingCollider;
    private int groundContacts = 0;
    private bool isGrounded = false;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Ambil Input
        float horizontalInput = Input.GetAxis("Horizontal");
        bool isRunning = Input.GetAxis("Run") > 0;
        bool isCrouching = Input.GetKey(KeyCode.C);
        bool jumpPressed = Input.GetButtonDown("Jump");

        // Speed Modifier
        float currentSpeed = isRunning ? runSpeed : movSpeed;

        // Update Velocity
        Vector2 velocity = rb.linearVelocity;
        velocity.x = horizontalInput * currentSpeed;

        if (jumpPressed && isGrounded && isRunning && (velocity.x > movSpeed || velocity.x < -movSpeed))
        {
            velocity.y = runJumpForce;
        }
        else if (jumpPressed && isGrounded)
        {
            velocity.y = normalJumpForce;
        }

        rb.linearVelocity = velocity;

        // Mekanik Crouching
        // Mengaktifkan collider "berdiri" saat tidak crouch, dan collider "crouch" saat crouching
        standingCollider.enabled = !isCrouching;
        crouchingCollider.enabled = isCrouching;
    }



    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Object"))
        {
            groundContacts++;
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Object"))
        {
            groundContacts--;
            if (groundContacts <= 0)
            {
                isGrounded = false;
                groundContacts = 0; // just in case
            }
        }
    }
}
