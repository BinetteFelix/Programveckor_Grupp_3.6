using UnityEngine;
using Unity.Cinemachine;

public class TESTMOVEMENT : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] CinemachineImpulseSource CMISource;

    private bool _iswarping;

    [SerializeField] private Transform _interactCheckPoint;
    [SerializeField] private Vector2 _interactCheckSize;

    [Header("Layers & Tags")]
    [SerializeField] private LayerMask _warpLayer;

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

        if(Physics2D.OverlapBox(_interactCheckPoint.position, _interactCheckSize, _warpLayer))
        {
            if (Input.GetKeyUp(KeyCode.E) && !_iswarping)
            {
                CMISource.GenerateImpulse();
                _iswarping = true;
            }
        }
        else if (!Physics2D.OverlapBox(_interactCheckPoint.position, _interactCheckSize, _warpLayer))
        {
            _iswarping = false;
        }
    }
    
}
