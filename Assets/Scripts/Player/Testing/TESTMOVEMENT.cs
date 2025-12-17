using UnityEngine;

public class TESTMOVEMENT : MonoBehaviour
{
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
            rb.linearVelocityX = -5;
        else if (Input.GetKey(KeyCode.D))
            rb.linearVelocityX = 5;
        else 
            rb.linearVelocityX = 0;
    }
}
