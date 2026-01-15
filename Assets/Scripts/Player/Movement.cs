using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private AudioClip[] slashSoundFXs;

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
    private float jumpPower = 8f;
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
    private Vector2 wallJumpingPower = new Vector2(10f, 7.5f);

    private Animator animator;
    private float runSpeedMultiplier = 5f;
    private bool canRun = true;
    private float AnimationDirection;

    private bool lastDirection;

    
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
        animator.SetBool("IsGrounded", IsGrounded());

        if (rb.linearVelocityY < -1) animator.ResetTrigger("Jump");
        spriteRenderer.flipX = (isWallSliding && !isFacingRight) ? true : false;
        LastPressedJumpTime -= Time.deltaTime;
        LastOnGroundTime -= Time.deltaTime;

        #region GET ACTION VALUES
        moveValue = moveAction.ReadValue<Vector2>();
        jumpValue = jumpAction.IsPressed();
        runValue = runAction.IsPressed();
        #endregion

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

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SoundFXManager.Instance.PlaySoundFXClip(slashSoundFXs, transform);
        }
        if (IsGrounded())
        {
            LastOnGroundTime = 0.1f;
        }
    }

    private void FixedUpdate()
    {
        if (!isWallJumping)
        {
            Move(1);
            Jump();
        }

       /* if (Input.GetKeyDown(KeyCode.Mouse0))
        {
          //  SoundFXManager.Instance.PlaySoundFXClip(slashSoundFX, transform);
        }
       */
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
        float wallCheckRadius = 0.05f;
        bool isWalled = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallLayer); //wallLayer is just ground map because that makes it so you don't have to separate the tilemaps.
        if (isWalled && isWallSliding)
        {
            canRun = false;
        }
        else
            canRun = true;

        return isWalled;
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
        Vector2 force = (runValue) ? (movement * Vector2.right * runSpeedMultiplier) : (movement * Vector2.right);

        if (!IsGrounded())
        {
            force = movement * Vector2.right;
        }

        rb.AddForce(force, ForceMode2D.Force);
        animator.SetFloat("DirectionX", (runValue) ? moveValue.x * 2 : moveValue.x);

        //Increase speed if running
        /*
        if (!runValue && !isWallSliding)
        {
            animator.SetFloat("DirectionX", moveValue.x);
            rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
        } else if (runValue && canRun && !isWallSliding && IsGrounded())
        {
            animator.SetFloat("DirectionX", moveValue.x * 2); //Change to actual run animation later
            rb.AddForce((movement * Vector2.right) * runSpeedMultiplier, ForceMode2D.Force);
        }
        */

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
        Vector2 force = (runValue) ? ((Vector2.up * jumpPower) * 1.20f) : (Vector2.up * jumpPower);
        //Jump
        if (jumpValue && IsGrounded() && LastPressedJumpTime < 0 && LastOnGroundTime > 0)
        {
            animator.Play("Jump");
            LastPressedJumpTime = 0.2f;
            rb.AddForce(force, ForceMode2D.Impulse);
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
            animator.SetFloat("DirectionX", 0);
            rb.linearVelocityY = Mathf.Clamp(rb.linearVelocityY, -wallSlidingSpeed, maxSpeed);
        }
        else
        {
            
            Debug.Log($"IsWalled: {IsWalled()}, IsGrounded: {IsGrounded()}, moveValue.x: {moveValue.x}");//Debugging why wall jump wont work sometimes
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if(isWallSliding == true)
        {
            isWallJumping = false;
            wallJumpingDirection = (isFacingRight) ? -1 : 1;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }
        if (jumpAction.WasPressedThisFrame() && wallJumpingCounter > 0f && LastPressedJumpTime < 0)
        {
            Debug.Log("walljump!");
            isWallJumping = true;
            //StartCoroutine(DisableAndReenableMovement());
            LastPressedJumpTime = 0.2f;
            rb.linearVelocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;
            int direction = (isFacingRight) ? 1 : -1;
            if (direction != wallJumpingDirection)
            {
                //Flip
                isFacingRight = !isFacingRight;
                Vector3 wallCheckLocalPos = wallCheck.transform.localPosition;
                wallCheckLocalPos.x *= -1f;
                wallCheck.transform.localPosition = wallCheckLocalPos;
            }
            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
        else if(jumpAction.WasPressedThisFrame())
        {
            if (wallJumpingCounter < 0f | LastPressedJumpTime > 0f)
            {
                Debug.Log($"WallJumpCounter: {wallJumpingCounter > 0f}");
                Debug.Log($"LastPressedJumpTime: {LastPressedJumpTime < 0}");
                Debug.Log($"WallSliding: {isWallSliding}");
            }

        }
        
    }

    private IEnumerator DisableAndReenableMovement()
    {
        moveAction.Disable();
        animator.SetFloat("DirectionX", (isFacingRight) ? -2 : 2);
        canRun = false;
        yield return new WaitForSeconds(0.025f);
        moveAction.Enable();
        yield return new WaitForSeconds(0.5f);
        canRun = true;

    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }
}
