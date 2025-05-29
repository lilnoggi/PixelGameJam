using UnityEngine;

public class TestingPlayerMovement : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody2D rb;
    public BoxCollider2D groundCheck;
    private Animator anim;

    [Header("Settings")]
    public LayerMask groundMask;
    public float groundSpeed = 3f;
    public float jumpForce = 5f;
    [Range(0f, 1f)] public float groundDecay = 0.2f;
    public float maxFallSpeed = -10f;

    [Header("State")]
    public bool grounded;

    float xInput;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        GetInput();
        Move();

        if (Input.GetButtonDown("Jump") && grounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        CheckGround();
        ApplyFriction();
        ClampFallSpeed();
    }

    void GetInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
    }

    void Move()
    {
        anim.SetFloat("Speed", Mathf.Abs(xInput));
        bool idleState = Mathf.Abs(xInput) < 0.01f;
        anim.SetBool("IsIdle", idleState);
        Debug.Log("IsIdle " + idleState + " xInput: " + xInput);

        rb.linearVelocity = new Vector2(xInput * groundSpeed, rb.linearVelocity.y);

        // Flip sprite
        if (xInput != 0)
        {
            float direction = Mathf.Sign(xInput);
            transform.localScale = new Vector3(direction, 1f, 1f);
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    void CheckGround()
    {
        grounded = Physics2D.OverlapBox(
            groundCheck.bounds.center,
            groundCheck.bounds.size,
            0f,
            groundMask
        );
    }

    void ApplyFriction()
    {
        if (grounded && xInput == 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x * groundDecay, rb.linearVelocity.y);
        }
    }

    void ClampFallSpeed()
    {
        if (rb.linearVelocity.y < maxFallSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, maxFallSpeed);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(groundCheck.bounds.center, groundCheck.bounds.size);
        }
    }
}
