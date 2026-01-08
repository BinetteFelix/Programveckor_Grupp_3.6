using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    //Variables
    Rigidbody2D rb;
    InputAction moveAction;
    InputAction jumpAction;
    bool isJumping;
    bool isFalling;

    //Values
    [SerializeField]
    float maxSpeed = 5f;
    float jumpPower = 10f;
    float maxFallSpeed = 10f;

    void Start()
    {
        InputSystem.actions.Enable();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        rb = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        // Get the values from the move action and jump action
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        bool jumpValue = jumpAction.IsPressed();

        //Make the player move faster depending on how close they are to the max speed
        float targetSpeed = moveValue.x * maxSpeed;
        float speedDif = targetSpeed - rb.linearVelocityX;
        float movement = speedDif * 3f;
        rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
        //Jump
        if(jumpValue && isJumping == false)
        {
            isJumping = true;
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        } 
        if(!jumpValue && rb.linearVelocityY > 0)
        {
            rb.linearVelocityY = rb.linearVelocityY / 2;
        }
        //Make it so you gravity increases when you start falling
        if (rb.linearVelocityY < 0 && isFalling == false)
        {
            isFalling = true;
            SetGravityScale(1.5f);
            Debug.Log(rb.gravityScale);
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
        rb.gravityScale = rb.gravityScale * amount;
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
