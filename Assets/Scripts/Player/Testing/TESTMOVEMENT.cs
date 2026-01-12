using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TESTMOVEMENT : MonoBehaviour
{
    [SerializeField] private AudioClip slashSoundFX;

    //Variables
    Rigidbody2D rb;
    InputAction moveAction, jumpAction, runAction;
    Vector2 moveValue;
    bool jumpValue;
    bool running;
    bool isFalling = false;
    bool isRunning;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;

    [SerializeField] Transform leftRayOrigin;
    [SerializeField] Transform rightRayOrigin;

    public PlayerData Data;
    bool canjump;
    //Values
    float maxSpeed = 7f;
    [SerializeField] float jumpPower = 10f;
    float maxFallSpeed = 10f;
    float fallSpeedIncreaseAtJumpApex = 2f;

    private float LastPressedJumpTime;
    private float LastOnGroundTime;


    void Start()
    {
        InputSystem.actions.Enable();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        runAction = InputSystem.actions.FindAction("Run");

        rb = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        LastPressedJumpTime -= Time.deltaTime;
        LastOnGroundTime -= Time.deltaTime;

        // Get the values from the actions;
        moveValue = moveAction.ReadValue<Vector2>();
        jumpValue = jumpAction.IsPressed();
        running = runAction.IsPressed();

        Collider2D warpObjectCollision = Physics2D.OverlapCircle(transform.position, Data.interactionRadius, Data._interactableSceneObjectsLayer);

        if (!SceneController.Instance.IsPaused)
        {
            if (Input.GetButtonUp("Inventory"))
            {
                SceneController.Instance.ToggleInventory();
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SoundFXManager.Instance.PlaySoundFXClip(slashSoundFX, transform);
        }

        if (GroundCheck())
        {
            LastOnGroundTime = 0.1f;
        }

        if (LastOnGroundTime < 0 && rb.linearVelocityY < -1.5)
        {
            jumpAction.Disable();
        }
        else
            jumpAction.Enable();
    } 

    bool GroundCheck()
    {
        bool rayHit;
        RaycastHit2D[] rayCasts = new RaycastHit2D[2];

        rayCasts[0] = Physics2D.Raycast(leftRayOrigin.position, Vector2.down, 0.63f, groundLayer);
        rayCasts[1] = Physics2D.Raycast(rightRayOrigin.position, Vector2.down, 0.63f, groundLayer);

        if (Physics2D.RaycastNonAlloc(transform.position, Vector2.down, rayCasts, 0.63f, groundLayer) > 0)
        {
            rayHit = true;
            return rayHit;
        }
        else
        {
            rayHit = false;
            return rayHit;
        }

        
    }


    private void FixedUpdate()
    {
        Move(1);
        Jump();
    }

    private void Move(float lerpAmount)
    {

        //Make the player move faster depending on how close they are to the max speed
        float targetSpeed = moveValue.x * maxSpeed;
        targetSpeed = Mathf.Lerp(rb.linearVelocityX, targetSpeed, lerpAmount);
        float speedDif = targetSpeed - rb.linearVelocityX;
        float movement = speedDif * 3f;
        //Increase speed if running
        if (!running)
        {
            rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
        }
        else
        {
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
        if (jumpValue && GroundCheck() && LastPressedJumpTime < 0 && LastOnGroundTime > 0)
        {
            LastPressedJumpTime = 0.25f;
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
        //Make it so when you release the jump button velocity is halfed, so you can do shorter jumps by just tapping
        else if (!jumpValue && rb.linearVelocityY > 0 && !GroundCheck())
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
}