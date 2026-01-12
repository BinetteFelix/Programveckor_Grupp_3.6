using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    //Variables
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private InputAction moveAction, jumpAction, runAction;

    private Vector2 moveValue;
    private bool jumpValue;
    private bool runValue;
    private bool isFalling = false;

    private bool isWallSliding;
    private bool isFacingRight = false;

    private float maxSpeed = 7f;
    private float jumpPower = 10f;
    private float maxFallSpeed = 10f;
    private float wallSlidingSpeed = 2f;

    private float fallSpeedIncreaseAtJumpApex = 2f;
    private float LastPressedJumpTime;
    private float LastOnGroundTime;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.05f;
    private Vector2 wallJumpingPower = new Vector2(10f, 4f);

    private float moveDisabledAfterWallJumpTime = 0.5f;
    private Animator animator;

    
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] Transform wallCheck;

    void Start()
    {
        moveAction = InputManager.instance.moveAction;
        jumpAction = InputManager.instance.jumpAction;
        runAction = InputManager.instance.runAction;

        rb = this.GetComponent<Rigidbody2D>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        animator = this.GetComponent<Animator>();
    }

    void Update()
    {
        LastPressedJumpTime -= Time.deltaTime;
        // Get the values from the actions;
        moveValue = moveAction.ReadValue<Vector2>();
        jumpValue = jumpAction.IsPressed();
        runValue = runAction.IsPressed();
        WallSlide();
        WallJump();
        if (!isWallJumping)
        {
            Flip();
        }
        if (LastOnGroundTime < 0 && rb.linearVelocityY < -1.5)
        {
            jumpAction.Disable();
        }
        else
            jumpAction.Enable();
    }

    private void FixedUpdate()
    {
        if (!isWallJumping)
        {
            Move(1);
            Jump();
        }
        
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.02f, groundLayer);
        // Maybe use raycast instead (or maybe that's more for enemy AI)

       /* if (Physics2D.Raycast(transform.position, Vector2.down, 0.63f, groundLayer))
        {
            return true;
        }
        */
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.02f, wallLayer);
    }



    void Flip()
    {
        if(isFacingRight && moveValue.x < 0f || !isFacingRight && moveValue.x > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 wallCheckLocalPos = wallCheck.transform.localPosition;
            wallCheckLocalPos.x *= -1f;
            wallCheck.transform.localPosition = wallCheckLocalPos;
           // transform.localScale = localScale;
        }
    }



    private void Move(float lerpAmount)
    {
        
        //Make the player move faster depending on how close they are to the max speed
        float targetSpeed = moveValue.x * maxSpeed;
        targetSpeed = Mathf.Lerp(rb.linearVelocityX, targetSpeed, lerpAmount);
        float speedDif = targetSpeed - rb.linearVelocityX;
        float movement = speedDif * 3f;
        //Increase speed if running
        if (runValue == false)
        {
            animator.SetFloat("DirectionX", moveValue.x);
            rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
        } else
        {
            animator.SetFloat("DirectionX", moveValue.x); //Change to actual run animation later
            rb.AddForce((movement * Vector2.right) * 1.25f, ForceMode2D.Force);
        }

        //Set velocity to 0 when you stop holding the stick
        if (moveValue.x == 0)
        {
            if (rb.linearVelocityX > 0)
            {
                rb.linearVelocityX -= 0.075f;
            }
            else
            {
                rb.linearVelocityX += 0.075f;
            }
        }  

    }

    private void Jump()
    {
        //Jump
        if (jumpValue && IsGrounded() && LastPressedJumpTime < 0)
        {
            LastPressedJumpTime = 0.2f;
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
        //Make it so when you release the jump button velocity is halfed, so you can do shorter jumps by just tapping
        else if (!jumpValue && rb.linearVelocityY > 0 && !IsGrounded())
        {
            rb.linearVelocityY /= 3;
        }
        //Make it so you gravity increases when you start falling
        else if (rb.linearVelocityY < 0 && isFalling == false)
        {
            isFalling = true;
            rb.gravityScale *= fallSpeedIncreaseAtJumpApex;
        }
        else
        {
            rb.gravityScale = 2;
        }
        //Limit max fall speed
        rb.linearVelocityY = Mathf.Max(rb.linearVelocityY, -maxFallSpeed);
    }

    private void WallSlide()
    {
        if(IsWalled() && !IsGrounded() && moveValue.x != 0f)
        {
            isWallSliding = true;
            rb.linearVelocityY = Mathf.Clamp(rb.linearVelocityY, -wallSlidingSpeed, maxSpeed);
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if(isWallSliding == true)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }
        if (jumpAction.WasPressedThisFrame() && wallJumpingCounter > 0f && LastPressedJumpTime < 0)
        {
            isWallJumping = true;
            LastPressedJumpTime = 0.2f;
            rb.linearVelocity = new Vector2(-wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                //Flip
                isFacingRight = !isFacingRight;
                Vector3 wallCheckLocalPos = wallCheck.transform.localPosition;
                wallCheckLocalPos.x *= -1f;
                wallCheck.transform.localPosition = wallCheckLocalPos;
            }
            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
        
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }
}
