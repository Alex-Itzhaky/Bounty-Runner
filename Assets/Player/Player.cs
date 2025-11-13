using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Transform playerTransform;

    [Header("Inputs")]
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private bool jumpPressed;
    [SerializeField] private bool jumpReleased;
    [SerializeField] private int facingDirection = 1;

    [Header("Movement Vars")]
    public float speed = 20;
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

    [Header("Slide Settings")]
    public float slideDuration = .6f;
    public float slideSpeed = 12;
    [SerializeField] private bool isSliding;
    [SerializeField] private float slideTimer;


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
        HandleSlide();
    }

    private void FixedUpdate()
    {
        ApplyGravity();
        CheckGrounded();

        if (!isSliding)
            HandleMovement();
        
        HandleJump();
    }

    private void HandleMovement()
    {
        rb.linearVelocity = new Vector2(moveInput.x * speed , rb.linearVelocity.y);
        
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

    private void HandleSlide()
    {
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            rb.linearVelocity = new Vector2(slideSpeed * facingDirection, rb.linearVelocity.y);
            if (slideTimer <= 0)
            {
                isSliding = false;
            }
        }
        if(isGrounded && !isSliding)
        {
            isSliding = true;
            slideTimer = slideDuration;
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
