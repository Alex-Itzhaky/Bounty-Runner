using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private TrailRenderer tr;

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

    [Header("Dash Settings")]
    private bool canDash = true;
    private bool isDashing;
    public int dashingPower = 24;
    public float dashingTime = 0.2f;
    public float dashCooldown = 1f;
    private int maxDash = 1;
    private int dashCount;

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
        rb.gravityScale = normalGravity;
        dashCount = maxDash;
    }

    private void Update()
    {
        Flip();
        DashCounterUpdate();
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
        if (isDashing)
            return;
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
    
    private IEnumerator HandleDash()
    {
        dashCount--;
        Debug.Log(dashCount);
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        Vector2 dashDirection;
        if (moveInput.sqrMagnitude > 0.01f)
            dashDirection = moveInput.normalized;
        else
            dashDirection = transform.forward;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(dashDirection * dashingPower, ForceMode2D.Impulse);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);

        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }

    private void ApplyGravity()
    {
        if (isDashing)
            return;
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
        if (isDashing)
            return;
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

    private void DashCounterUpdate()
    {
        if (isGrounded)
        {
            dashCount = maxDash;
        }
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    

    void OnJump(InputValue value)
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

    void OnDash(InputValue value)
    {
        if (!canDash || isDashing || dashCount <= 0)
            return;
        StartCoroutine(HandleDash());
    }
    /*
    private void OnSlide(InputValue value)
    {
        if (value.isPressed)
        {
            slidePressed = true;
            slideReleased = false;
        }
        else
        {
            slideReleased = true;
        }
    }
    */

    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
}
