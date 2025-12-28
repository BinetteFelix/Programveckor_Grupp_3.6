using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private GameObject IO;
    private InteractableObject ioHandler;
    private IOData ioData;
    private CircleCollider2D cC2D;

    public PlayerData PlayerData;
    

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
        //_interactInput = Input.GetButtonUp("Interact");
        _interactInput = Input.GetKeyUp(KeyCode.E);

        if (_interactInput)
            OnInteractInput();
        #endregion

        if (CanPickUp() && LastPressedInteractTime > 0)
        {
            Interact();
        }

    }

    private void OnInteractInput()
    {
        LastPressedInteractTime = 0.1f;
    }
    
    private void Interact()
    {
        if (ioData.isNPC)
            ioHandler.TalkToPlayer();
        else if (ioData.isItem)
            ioHandler.PickUpItem();
        else if (ioData.isWarpObject && !ioHandler.IsWarping)
            ioHandler.InteractWarpObject();
    }

    private bool CanPickUp()
    {
        return IsInteractRange;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IO = collision.gameObject;
        ioHandler = IO.GetComponent<InteractableObject>();
        ioData = ioHandler.ioData;

        if (ioHandler != null && collision.isTrigger)
        {
            IsInteractRange = true;
            Debug.Log("Can interact");
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        IO = collision.gameObject;
        ioHandler = IO.GetComponent<InteractableObject>();
        ioData = ioHandler.ioData;

        if (ioHandler != null && collision.isTrigger)
        {
            Debug.Log("Can't interact");
            IsInteractRange = false;
        }
    }
}