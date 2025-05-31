using UnityEngine;

/// <summary>
/// Controls 2D player movement: walk, jump, ground detection, and interaction.
/// Must be attatched to the Player root GameObject (has the Rigidbody2D component).
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Horizontal movement speed.")]
    [SerializeField] private float moveSpeed = 5f;
    [Tooltip("Force applied when jumping.")]
    [SerializeField] private float jumpForce = 8f;
    [Tooltip("Maximum downward fall speed.")]
    [SerializeField] private float maxFallSpeed = -10f;
    [Tooltip("Friction factor applied when grounded and idle.")]
    [SerializeField, Range(0f, 1f)] private float frictionFactor = 0.2f;

    [Header("Ground Check Settings")]
    [Tooltip("Collider used to check if the player is grounded.")]
    [SerializeField] private BoxCollider2D groundCheckCollider;
    [Tooltip("Layer(s) considered as ground.")]
    [SerializeField] private LayerMask groundLayer;

    [Header("Interaction")]
    [Tooltip("Radius around interactPoint to detect interactable objects.")]
    [SerializeField] private float interactRange = 1f;
    [Tooltip("Layers considered interactable.")]
    [SerializeField] private LayerMask interactLayer;
    [Tooltip("Point from where interactions are detected.")]
    [SerializeField] private Transform interactPoint;

    // Components
    private Rigidbody2D rb;
    private Animator anim;

    // Input values
    private float xInput;

    // State flags
    private bool isGrounded;
    private bool isInteracting;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // Only allow movement input if not interacting
        if (!isInteracting)
        {
            HandleInput();
            HandleJump();
        }

        HandleInteraction(); // Always listen for interaction input
        UpdateAnimations(); // Update animator parameters every frame
    }

    void FixedUpdate()
    {
        CheckGround();
        ApplyMovement();
        ApplyFriction();
        ClampFallSpeed();
    }

    /// <summary>Reads horizontal input axis ("Horizontal").</summary>
    void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
    }

    /// <summary>Applies jump force if jump button is pressed and player is grounded.</summary>
    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    /// <summary>Applies horizontal movement based on input.
    /// Also flips the player sprite to face movement direction.
    /// </summary>
    void ApplyMovement()
    {
        rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);

        if (xInput != 0)
        {
            float direction = Mathf.Sign(xInput);
            transform.localScale = new Vector3(direction, 1f, 1f);
        }
    }

    /// <summary>
    /// Checks if player is touching the ground using the groundCheckCollider.
    /// </summary>
    void CheckGround()
    {
        isGrounded = Physics2D.OverlapBox(
            groundCheckCollider.bounds.center,
            groundCheckCollider.bounds.size,
            0f,
            groundLayer
        );
    }

    /// <summary>Applies friction when grounded and no horizontal input to slow player down smoothly.</summary>
    void ApplyFriction()
    {
        if (isGrounded && Mathf.Approximately(xInput, 0f))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x * frictionFactor, rb.linearVelocity.y);
        }
    }

    /// <summary>Limits the maximum falling speed so player doesn't fall too fast.</summary>
    void ClampFallSpeed()
    {
        if (rb.linearVelocity.y < maxFallSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, maxFallSpeed);
        }
    }

    /// <summary>Updates animator parameters for movement and grounded state.</summary>
    void UpdateAnimations()
    {
        if (anim == null) return;

        anim.SetFloat("Speed", Mathf.Abs(xInput));
        anim.SetBool("Grounded", isGrounded);
    }

    /// <summary>Draws ground check box in the editor for visualisation.</summary>
    void OnDrawGizmosSelected()
    {
        if (groundCheckCollider != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(groundCheckCollider.bounds.center, groundCheckCollider.bounds.size);
        }
    }

    /// <summary>
    /// Handles interaction input and triggers interaction coroutine.
    /// </summary>
    void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isInteracting)
        {
            // Check for interactable object within range
            Collider2D target = Physics2D.OverlapCircle(interactPoint.position, interactRange, interactLayer);
            if (target != null && target.TryGetComponent(out IInteractable interactable))
            {
                StartCoroutine(PerformInteraction(interactable));
            }
        }
    }

    /// <summary>
    /// Coroutine to perform interaction:
    /// disables movement, plays interaction animation, invokes interactable's Interact,
    /// waits before enabling movement again.
    /// </summary>
    System.Collections.IEnumerator PerformInteraction(IInteractable interactable)
    {
        isInteracting = true;
        rb.linearVelocity = Vector2.zero; // Stop player movement
        anim.SetTrigger("Interact"); // Trigger interact animation

        yield return new WaitForSeconds(0.5f); // Wait for anim duration

        interactable.Interact(); // Call object's interaction method

        // Extra wait time before player can move again
        float extraWait = 0.3f;
        yield return new WaitForSeconds(extraWait);

        isInteracting = false; // Player moves again
    }
}
