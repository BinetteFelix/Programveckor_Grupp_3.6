using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TESTMOVEMENT : MonoBehaviour
{
    public PlayerData Data;

    Rigidbody2D rb;

    [SerializeField] CinemachineImpulseSource CMISource;

    private bool _iswarping;

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
        Collider2D warpObjectCollision = Physics2D.OverlapCircle(transform.position, Data.interactionRadius, Data._interactableSceneObjectsLayer);

        if (!SceneController.Instance.IsPaused)
        {
            if (Input.GetKey(KeyCode.A))
                rb.linearVelocityX = -5;
            else if (Input.GetKey(KeyCode.D))
                rb.linearVelocityX = 5;
            else
                rb.linearVelocityX = 0;
        }

        if (warpObjectCollision)
        {
            if (Input.GetKeyUp(KeyCode.E) && !_iswarping)
            {
                CMISource.GenerateImpulse();
                _iswarping = true;
                if (SceneController.Instance.CurrentOpenScene == 1)
                    SceneController.Instance.StartCoroutine(SceneController.Instance.LoadScene(2));
                else if (SceneController.Instance.CurrentOpenScene == 2)
                    SceneController.Instance.StartCoroutine(SceneController.Instance.LoadScene(1));
            }
        }
        else if (!warpObjectCollision)
        {
            _iswarping = false;
        }
    }

    #region EDITOR METHODS
    
    #endregion
}