using System.Collections;
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
    [SerializeField] private bool dashPressed;
    //[SerializeField] private bool slidePressed;
    //[SerializeField] private bool slideReleased;
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
    }

    private void Update()
    {
        Flip();
        //HandleSlide();
    }

    private void FixedUpdate()
    {
        if (isDashing)
            return;

        ApplyGravity();
        CheckGrounded();
        HandleMovement();        
        HandleJump();
        HandleDash();
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
    
    private IEnumerator HandleDash()
    {
        if (dashPressed && canDash)
        {
            canDash = false;
            isDashing = true;
            float originalGravity = rb.gravityScale;
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
            tr.emitting = true;
            yield return new WaitForSeconds(dashingTime);
            tr.emitting = false;
            rb.gravityScale = originalGravity;
            isDashing = false;
            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
        }
    }

/*
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
*/
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

    private void OnDash(InputValue value)
    {
        if (value.isPressed)
        {
            dashPressed = true;
        }
        else
        {
            dashPressed = false;
        }
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
