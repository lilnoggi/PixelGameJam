using UnityEngine;

/// <summary>
/// Handles 2D platformer player movement: walk, jump, and ground check.
/// Must be attatched to the Player root GameObject.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float maxFallSpeed = -10f;
    [SerializeField, Range(0f, 1f)] private float frictionFactor = 0.2f;

    [Header("Ground Check Settings")]
    [SerializeField] private BoxCollider2D groundCheckCollider;
    [SerializeField] private LayerMask groundLayer;

    // Components
    private Rigidbody2D rb;
    private Animator anim;

    // Input
    private float xInput;

    // State
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        HandleInput();
        HandleJump();
        UpdateAnimations();
    }

    void FixedUpdate()
    {
        CheckGround();
        ApplyMovement();
        ApplyFriction();
        ClampFallSpeed();
    }

    /// <summary>Stores horizontal input.</summary>
    void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
    }

    /// <summary>Applies jump force if grounded.</summary>
    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    /// <summary>Applies horizontal movement.</summary>
    void ApplyMovement()
    {
        rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);

        // Flip sprite based on direction
        if (xInput != 0)
        {
            float direction = Mathf.Sign(xInput);
            transform.localScale = new Vector3(direction, 1f, 1f);
        }
    }

    /// <summary>Checks if player is touching the ground.</summary>
    void CheckGround()
    {
        isGrounded = Physics2D.OverlapBox(
            groundCheckCollider.bounds.center,
            groundCheckCollider.bounds.size,
            0f,
            groundLayer
        );
    }

    /// <summary>Applies horizontal friction if grounded and not moving.</summary>
    void ApplyFriction()
    {
        if (isGrounded && Mathf.Approximately(xInput, 0f))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x * frictionFactor, rb.linearVelocity.y);
        }
    }

    /// <summary>Limits downward fall speed.</summary>
    void ClampFallSpeed()
    {
        if (rb.linearVelocity.y < maxFallSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, maxFallSpeed);
        }
    }

    /// <summary>Updates animation parameters.</summary>
    void UpdateAnimations()
    {
        if (anim == null) return;

        anim.SetFloat("Speed", Mathf.Abs(xInput));
        anim.SetBool("Grounded", isGrounded);
    }

    /// <summary>Draws ground check box in Scene view.</summary>
    void OnDrawGizmosSelected()
    {
        if (groundCheckCollider != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(groundCheckCollider.bounds.center, groundCheckCollider.bounds.size);
        }
    }
}
