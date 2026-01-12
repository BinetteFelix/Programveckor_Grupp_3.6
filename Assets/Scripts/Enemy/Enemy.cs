using UnityEngine;

public class Enemy : MonoBehaviour
{
    Vector2 direction;
    Rigidbody2D rb;
    [SerializeField] GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = player.transform.position - transform.position;
        Debug.DrawLine(player.transform.position, transform.position);
        Debug.Log(direction);
        if (Mathf.Abs(direction.x) > 1f)
        {
            rb.linearVelocityX = direction.x * 2f;

        }
        else rb.linearVelocityX = 0;


    }
}
