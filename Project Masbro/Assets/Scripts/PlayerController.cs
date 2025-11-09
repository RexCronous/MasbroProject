using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);

    [Header("Head Check")]
    [SerializeField] private Transform headCheckPos;
    [SerializeField] private Vector2 headCheckSize = new Vector2(0.2f, 0.05f);

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;

    [Header("Movement Parameters")]
    // [SerializeField] private float movSpeed = 10.0f;
    // [SerializeField] private float runSpeed = 20.0f;
    // [SerializeField] private float airTime = 0f;
    // [SerializeField] private float normalJumpForce = 5.0f;
    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private int maxJump = 1;
    [SerializeField] private float acceleration = 5.0f;
    [SerializeField] private float deceleration = 8.0f;
    [SerializeField] private float maxVelocityX = 11.0f;
    [SerializeField] private float maxVelocityY = 15f;

    [Header("Animation & Feedback")]
    [SerializeField] private Animator animator;
    public ParticleSystem SmokeFX;

    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;
    [SerializeField] private int jumpCount = 0;
    // private bool runBeforeJump = false;
    [SerializeField] private bool isGrounded = false;
        private bool wasGrounded = false;
    AudioManager audioManager;
    private float nextFootstepTime = 0f;
    private bool lastFacingRight;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        // Ambil Input
        float horizontalInput = Input.GetAxis("Horizontal");
        bool jumpPressed = Input.GetButtonDown("Jump");
        bool inputLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        bool inputRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        
        // Ground Checking
        wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer);
        if (!isGrounded && jumpCount > 0) print("not grounded");

        // bool isRunning = Input.GetAxis("Run") > 0;
        // bool isCrouching = Input.GetKey(KeyCode.C);

        // Double Jump Logic
        if (isGrounded && !wasGrounded)
        {
            jumpCount = 0;
        }

        // Update Velocity
        Vector2 velocity = rb.linearVelocity;

        float targetSpeedX = horizontalInput * maxVelocityX;

        if (inputLeft && inputRight)
            velocity.x = Mathf.Lerp(velocity.x, 0.0f, 6f * Time.deltaTime);
        else if (inputLeft || inputRight)
            velocity.x = Mathf.Lerp(velocity.x, targetSpeedX, acceleration * Time.deltaTime); // acceleration
        else
        {
            if (Mathf.Abs(velocity.x) > 0f && Mathf.Abs(velocity.x) < 0.7f)
                velocity.x = 0f;
            else
                velocity.x = Mathf.Lerp(velocity.x, 0.0f, deceleration * Time.deltaTime); // deceleration
        }

        if (!isGrounded && wasGrounded && jumpCount == 0)
        {
            jumpCount = 1;
        }

        if (jumpPressed) animator.SetBool("isJumping", true);

        if (jumpPressed && (isGrounded || jumpCount <= maxJump))
        {
            SmokeFX.Play();
            if (audioManager != null && audioManager.jump != null)
                audioManager.PlaySfx(audioManager.jump);

            if (!isGrounded)
                animator.SetBool("isDoubleJumping", true);
            else
                animator.SetBool("isDoubleJumping", false);

            velocity.y = jumpForce;
            jumpCount++;
            animator.SetBool("isJumping", true);
        }

        // Batas kecepatan
        velocity.x = Mathf.Clamp(velocity.x, -maxVelocityX, maxVelocityX);
        velocity.y = Mathf.Clamp(velocity.y, -maxVelocityY, maxVelocityY);

        if (velocity.y > 0)
        {
            velocity.y += Physics2D.gravity.y * Time.deltaTime * 1.5f;
        }
        else if (velocity.y < 0)
        {
            velocity.y += Physics2D.gravity.y * Time.deltaTime * 2f;
        }

        rb.linearVelocity = velocity;

        // Head Checking
        if (Physics2D.OverlapBox(headCheckPos.position, headCheckSize, 0, groundLayer) && !isGrounded)
        {
            capsuleCollider.enabled = false;
        }
        else
        {
            capsuleCollider.enabled = true;
        }

        // Flip player direction
        bool facingRight = transform.localScale.x > 0;
        if (horizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // footstep sfx
        bool isMovingNow = isGrounded && Mathf.Abs(rb.linearVelocity.x) > 0.1f;
        if (isMovingNow && Time.time >= nextFootstepTime)
        {
            if (audioManager != null && audioManager.walking != null && audioManager.walking.Length > 0)
            {
                AudioClip step = audioManager.walking[UnityEngine.Random.Range(0, audioManager.walking.Length)];

                audioManager.SetFootstepPitchWalk();
                nextFootstepTime = Time.time + 0.50f;
                audioManager.PlayFootstep(step);

            }
        }

        float velY = rb.linearVelocity.y;

        if (!isGrounded && velY > 0.01f)
        {
            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);
        }
        else if (!isGrounded && velY < -0.01f)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }

        // Animator controller
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocityX));

        // Efek smoke saat bergerak
        if (lastFacingRight != facingRight && Mathf.Abs(rb.linearVelocityX) > 0)
        {
            SmokeFX.Play();
        }

        lastFacingRight = facingRight;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        // Tidak digunakan
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Object"))
        {
            Rigidbody2D boxRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (boxRb != null)
            {
                foreach (ContactPoint2D contact in collision.contacts)
                {
                    if (contact.normal.y > 0.5f)
                    {
                        return;
                    }
                }

                float pushPower = 0.5f;
                Vector2 pushDir = new Vector2(rb.linearVelocity.x, 0);
                boxRb.linearVelocity = new Vector2(pushDir.x * pushPower, boxRb.linearVelocity.y);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
        Gizmos.DrawWireCube(headCheckPos.position, headCheckSize);
    }
}
