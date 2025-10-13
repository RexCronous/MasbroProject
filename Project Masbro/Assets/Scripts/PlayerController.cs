using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float movSpeed = 10.0f;
    [SerializeField] private float runSpeed = 20.0f;
    // [SerializeField] private float airTime = 0f;
    [SerializeField] private float normalJumpForce = 5.0f;
    [SerializeField] private float runJumpForce = 8.0f;
    [SerializeField] private int maxJump = 1;
    [SerializeField] private float maxVelocityY = 15f;
    [SerializeField] private Animator animator;
    //private int groundContacts = 0;
    private Rigidbody2D rb;
    private int jumpCount = 0;
    private bool runBeforeJump = false;
    private bool isGrounded = false;
    public ParticleSystem SmokeFX;
    //private bool isHit = false;

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

        // Mengecek apakah player sedang di tanah (grounded)
        if (isGrounded)
        {
            // Simpan status apakah player sedang berlari sebelum melakukan lompatan
            runBeforeJump = isRunning;
        }
        
        // Speed modifier
        float currentSpeed = 0f;

        // Menentukan kecepatan berdasarkan kondisi grounded atau tidak
        if (isGrounded)
        {
            // Jika player masih menyentuh tanah:
            // Gunakan kecepatan sesuai input saat ini (running atau normal)
            currentSpeed = isRunning ? runSpeed : movSpeed;
        }
        else
        {
            // Jika player sedang di udara:
            // Gunakan kecepatan berdasarkan status sebelum melompat
            // (tidak bisa mengubah dari jalan ke lari di udara)
            currentSpeed = runBeforeJump ? runSpeed : movSpeed;
        }


        // Update Velocity
        Vector2 velocity = rb.linearVelocity;

        velocity.x = horizontalInput * currentSpeed;

        // Double Jump Logic
        if (isGrounded)
        {
            jumpCount = 0;
        }
        
        if (jumpPressed) animator.SetBool("isJumping", true);

        if (jumpPressed && jumpCount < maxJump)
        {
            SmokeFX.Play();
            if (jumpCount == 0)
            {
                velocity.y = isRunning && (Mathf.Abs(velocity.x) > movSpeed) ? runJumpForce : normalJumpForce;
            }
            else if (jumpCount > 0 && !isGrounded)
            {
                velocity.y = normalJumpForce;
            }
            jumpCount++;
        }

        // Sedikit mengurangi kecepatan horizontal saat lari lalu melompat
        if (!isGrounded && Mathf.Abs(velocity.x) > movSpeed)
        {
            float targetSpeed = Mathf.Sign(velocity.x) * movSpeed;
            velocity.x = Mathf.Lerp(velocity.x, targetSpeed, Time.deltaTime * 50f);
        }

        // Biar tidak jatuh/naik terlalu cepat
        velocity.y = Mathf.Clamp(velocity.y, -maxVelocityY, maxVelocityY);

        rb.linearVelocity = velocity;

        // Ground Checking
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        //Flip player direction
        if (horizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        float velY = rb.linearVelocity.y;

        if (!isGrounded && velY > 0.01f) {
            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);
        }
        else if (!isGrounded && velY < -0.01f) {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);
        }
        else {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }

        //Animator controller
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocityX));

        if (isRunning && !SmokeFX.isPlaying && Mathf.Abs(rb.linearVelocityX) > 0) {
            SmokeFX.Play();
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        // touchingLeftWall = false;
        // touchingRightWall = false;
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
                    // Cek arah normal tabrakan
                    // Normal ke atas (y > 0.5) artinya player di atas box → jangan dorong
                    if (contact.normal.y > 0.5f)
                    {
                        return; // keluar, tidak dorong
                    }
                }

                // Kalau bukan di atas (berarti di samping), baru dorong
                float pushPower = 0.5f; // kecil = berat
                Vector2 pushDir = new Vector2(rb.linearVelocity.x, 0);
                boxRb.linearVelocity = new Vector2(pushDir.x * pushPower, boxRb.linearVelocity.y);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Reaching checkpoint
        if (other.gameObject.CompareTag("Checkpoint"))
        {
            GameManager.Instance.isAtCheckpoint = true;
        }

        // Reaching finish line
        if (other.gameObject.CompareTag("Finish"))
        {
            GameManager.Instance.nextLevel();
        }
    }

    // Buat bentuk visual gameobject di bawah player
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
    }
}