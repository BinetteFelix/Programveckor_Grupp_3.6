using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Vector2 direction;
    private Rigidbody2D rb;
    private GameObject player;
    private float speed = 2.5f;

    [SerializeField] private Transform hurtBox;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        direction = player.transform.position - transform.position;
        if (Mathf.Abs(direction.x) > 0.5f)
        {
            Move();
        }
        else
        {
            rb.linearVelocityX = 0;
        }

        if(direction.y > 0.5f)
        {
            Jump(3f);
        }
        if(direction.y > 1.25f)
        {
            Jump(6f);
        }
        if (Physics2D.OverlapBox(hurtBox.position, this.transform.localScale, 90f).gameObject.CompareTag("Player"))
        {
            //Debug.Log("hit!");
            //Hurt player code here
        }

    }

    private void Move()
    {
        rb.linearVelocityX = direction.normalized.x * speed;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.02f, groundLayer);

    }

    private void Jump(float force)
    {
        if(IsGrounded())
        {
            rb.AddForce(new Vector2(0, force), ForceMode2D.Impulse);
        }
    }

}
