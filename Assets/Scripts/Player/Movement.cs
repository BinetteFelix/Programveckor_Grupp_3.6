using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    //Variables
    Rigidbody2D rb;
    InputAction moveAction, jumpAction, runAction;
    Vector2 moveValue;
    bool jumpValue;
    bool runValue;
    bool isJumping;
    bool isFalling = false;
    bool isRunning;


    //Values
    [SerializeField]
    float maxSpeed;
    [SerializeField]
    float jumpPower = 5.5f;
    float maxFallSpeed = 10f;
    [SerializeField]
    float fallSpeedIncrease = 5f;

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
        // Get the values from the actions;
        moveValue = moveAction.ReadValue<Vector2>();
        jumpValue = jumpAction.IsPressed();
        runValue = runAction.IsPressed();
        
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
        rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
        //Deaccelerera när man slutar röra på sig
        if (moveValue.x == 0 && isJumping == false)
        {
           // rb.linearVelocityX = 0;
        }
        //Increase speed if running
        maxSpeed = (runValue) ? 7.5f : 6f;
    }

    private void Jump()
    {
        //Jump
        if (jumpValue && isJumping == false)
        {
            Debug.Log("Jump!");
            isJumping = true;
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
        //Make it so when you release the jump button velocity is halfed, so you can do shorter jumps by just tapping
        if (!jumpValue && rb.linearVelocityY > 0)
        {
            rb.linearVelocityY /= 2;
        }
        //Make it so you gravity increases when you start falling
        if (rb.linearVelocityY < 0 && isFalling == false)
        {
            isFalling = true;
            SetGravityScale(5f);
        }
        //Limit max fall speed
        rb.linearVelocityY = Mathf.Max(rb.linearVelocityY, -maxFallSpeed);
        //Reset gravity scale
        if (isFalling == false && rb.linearVelocityY == 0 | rb.linearVelocityY > 0)
        {
            rb.gravityScale = 1.0f;
        }
    }
    private void SetGravityScale(float amount)
    {
        rb.gravityScale *= amount;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Ground"))
        {
            isJumping = false;
            isFalling = false;
        }
    }
}
