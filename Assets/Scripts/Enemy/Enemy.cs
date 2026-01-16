using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Vector2 direction;
    private Rigidbody2D rb;
    private GameObject player;
    

    [SerializeField] private Transform hurtBox;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private float speed = 2.5f;
    [SerializeField] private int damage;
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

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            InvokeRepeating(nameof(Damage), 0.5f, 1);
        }
        else
        {
            Jump(3f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CancelInvoke(nameof(Damage));
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.02f, groundLayer);

    }
    private void Move()
    {
        rb.linearVelocityX = direction.normalized.x * speed;
    }

    private void Damage()
    {
        if (SceneController.Instance.gameOver) return;
        HealthManager.Instance.Damage(damage);
    }



    private void Jump(float force)
    {
        if(IsGrounded())
        {
            rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        }
    }

}
