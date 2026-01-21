using System;
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
    [SerializeField] private Animator anim;

    [Header("Inputs")]
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private bool jumpPressed;
    [SerializeField] private bool jumpReleased;
    [SerializeField] private int facingDirection = 1;

    [Header("Movement Vars")]
    public float speed = 14f;
    public float jumpForce;
    public float jumpCutMultiplier = .5f;
    public float normalGravity;
    public float jumpGravity;
    public float fallGravity;

    [Header("Dash Settings")]
    public int dashingPower = 24;
    public float dashingTime = 0.2f;
    public float dashCooldown = 1f;
    private int maxDash = 1;
    private int dashCount;
    private bool canDash = true;
    private bool isDashing;

    [Header("Wall Slide Settings")]
    public float wallSlideSpeed = 5f;

    [Header("Wall Jump Settings")]
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(14f, 18f);

    
    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isGrounded;

    [Header("Wall Check")]
    public Transform wallCheck;
    public float wallCheckRadius;
    public LayerMask wallLayer;
    private bool isWallSliding;

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
        DashCounterUpdate();
        HandleWallSlide();
        HandleWallJump();
        HandleAnimations();
        CheckGrounded();


        if (!isWallJumping)
            Flip();
    }

    private void FixedUpdate()
    {
        if (!isWallJumping)
            HandleMovement();
        ApplyGravity();
        WallCheck();
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
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        float dashDirection;
        if (moveInput.x != 0f)
            dashDirection = moveInput.x;
        else
            dashDirection = facingDirection;

        tr.emitting = true;

        float timer = 0f;
        while (timer < dashingTime)
        {
            RaycastHit2D hit = Physics2D.Raycast(rb.position, new Vector2(dashDirection, rb.position.y), wallCheckRadius, wallLayer);
            if (hit.collider != null)
            {
                break;
            }
            rb.linearVelocity = new Vector2(dashDirection * dashingPower, 0f);
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        tr.emitting = false;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = normalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }

    private void HandleWallSlide()
    {
        if (!isGrounded && isWallSliding && moveInput.x != 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -wallSlideSpeed));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void HandleWallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -facingDirection;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else if(!isGrounded)
        {
            wallJumpingCounter -= Time.deltaTime;
        }
        else
        {
            //Debug.Log("else atteint");
            wallJumpingCounter = 0f;
        }

        if (jumpPressed && wallJumpingCounter > 0)
        {
            isWallJumping = true;
            jumpPressed = false;
            rb.linearVelocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
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

    private void HandleAnimations()
    {
        anim.SetTrigger("Dash");

        anim.SetBool("isIdle", Mathf.Abs(moveInput.x) < .1f && isGrounded);
        anim.SetBool("isRunning", Mathf.Abs(moveInput.x) > .1f && isGrounded);

        anim.SetFloat("yVelocity", rb.linearVelocity.y);

        anim.SetBool("isJumping", rb.linearVelocity.y > .1f);
        anim.SetBool("isGrounded", isGrounded);


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
        if (moveInput.x > 0.1f)
        {
            moveInput.x = 1f;
        }
        else if (moveInput.x < -0.1f)
        {
            moveInput.x = -1f;
        }
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
            jumpReleased = true;        }
        
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

    private void WallCheck()
    {
        isWallSliding = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
    }
}
