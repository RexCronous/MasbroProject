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
    [SerializeField] private int maxJump = 1;

    public GameObject spawnPoint;
    public BoxCollider2D standingCollider;
    public BoxCollider2D crouchingCollider;
    public BoxCollider2D leftCollider;
    public BoxCollider2D rightCollider;
    private int groundContacts = 0;
    private bool isGrounded = false;
    private Rigidbody2D rb;
    private int jumpCount = 0;
    private bool touchingLeftWall = false;
    private bool touchingRightWall = false;

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

        if (touchingLeftWall && horizontalInput < 0)
        {
            velocity.x = 0; // prevent moving left into wall
        }
        else if (touchingRightWall && horizontalInput > 0)
        {
            velocity.x = 0; // prevent moving right into wall
        }
        else
        {
            velocity.x = horizontalInput * currentSpeed;
        }

        // Double Jump Logic
        if (isGrounded)
        {
            jumpCount = 0;
        }

        if (jumpPressed && jumpCount < maxJump)
        {
            if (isRunning && isGrounded && (velocity.x > movSpeed || velocity.x < -movSpeed))
            {
                velocity.y = runJumpForce;
            }
            else
            {
                velocity.y = normalJumpForce;
            }
            jumpCount++;
        }

        rb.linearVelocity = velocity;

        // if (rb.linearVelocityY == 0)
        // {
        //     rb.linearVelocityX = 0;
        // }

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
                groundContacts = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            // Check which side the wall is
            if (other.bounds.center.x < transform.position.x)
                touchingLeftWall = true;
            else
                touchingRightWall = true;
        }

        if (other.gameObject.CompareTag("Death"))
        {
            Destroy(gameObject);
            SpawnSystem.instance.Respawn();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            touchingLeftWall = false;
            touchingRightWall = false;
        }
    }
}