using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Transform playerTransform;

    [Header("Inputs")]
    private Vector2 moveInput;
    private bool jumpPressed;
    private bool jumpReleased;
    private int facingDirection = 1;

    [Header("Movement Vars")]
    public float maxSpeed = 20;
    public float acceleration;
    public float ground_drag;
    public float air_drag;
    public float jumpForce;
    public float jumpCutMultiplier = .5f;
    public float normalGravity;
    public float jumpGravity;
    public float fallGravity;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isGrounded;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        playerTransform = GetComponent<Transform>();

        rb.gravityScale = normalGravity;
    }

    private void Update()
    {
        Flip();
    }

    private void FixedUpdate()
    {
        ApplyGravity();
        CheckGrounded();
        HandleMovement();
        HandleJump();
    }

    private void HandleMovement()
    {
        if (rb.linearVelocity.x >= maxSpeed)
        {
            rb.linearVelocity = new Vector2(maxSpeed , rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x + acceleration, rb.linearVelocity.y);
        }
    }

    private void HandleJump()
    {
        if (jumpPressed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpPressed = false;
            jumpReleased = false;
        }
        if (jumpReleased)
        {
            if (rb.linearVelocity.y > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
            }
            jumpReleased = false;
        }
    }

    private void ApplyDrag()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x - ground_drag, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x - air_drag, rb.linearVelocity.y);
        }
    }

    private void ApplyGravity()
    {
        if (rb.linearVelocity.y < -0.1f) //Le joueur tombe
        {
            rb.gravityScale = fallGravity;
        }
        else if (rb.linearVelocity.y > 0.1f) //Le joueur saute
        {
            rb.gravityScale = jumpGravity;
        }
        else
        {
            rb.gravityScale = normalGravity; //Joueur au sol
        }

    }
    private void Flip()
    {
        if (moveInput.x > 0.1f)
        {
            facingDirection = 1;
        }
        else if (moveInput.x < -0.1f)
        {
            facingDirection = -1;
        }

        playerTransform.localScale = new Vector3(facingDirection, 1, 1);
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    

    private void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            jumpPressed = true;
            jumpReleased = false;
        }
        else
        {
            jumpReleased = true;
        }
    }

    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
}
