using UnityEditor.Rendering;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private GameObject IO;
    private InteractableObject ioHandler;
    public PlayerData PlayerData;
    private CircleCollider2D cC2D;

    public bool IsInteractRange { get; private set; }
    private bool isItemTrigger;

    #region INPUT PARAMETERS
    private bool _interactInput;
    public float LastPressedInteractTime {  get; private set; }
    #endregion
    private void Start()
    {
        cC2D = GetComponent<CircleCollider2D>();
        cC2D.radius = PlayerData.interactionRadius;
    }

    private void Update()
    {
        #region TIMERS
        LastPressedInteractTime -= Time.deltaTime;
        #endregion

        #region INPUT HANDLER
        _interactInput = Input.GetButtonUp("Interact");

        if (_interactInput)
            OnInteractInput();
        #endregion
    }

    private void OnInteractInput()
    {
        LastPressedInteractTime = 0.1f;
    }
    
    private void Interact()
    {
        GameObject[] itemObjects = GameObject.FindGameObjectsWithTag("Item");
        foreach (var item in itemObjects)
        {
            if (itemObjects != null && Vector2.Distance(gameObject.transform.position, item.transform.position) < PlayerData.interactionRadius)
            {

                ioHandler.Interact();
                Debug.Log($"{ioHandler.Data.objectTag} was picked up");
            }
        }
    }

    private bool CanPickUp()
    {
        return IsInteractRange;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IO = collision.gameObject;
        ioHandler = IO.GetComponent<InteractableObject>();

        if (ioHandler != null)
        {
            if (ioHandler.Data.interactionLayer == PlayerData._itemLayer)
            {
                ioHandler.Interact();
            }
        }
    }
}
