using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public InputAction moveAction { get; private set; }
    public InputAction jumpAction {get; private set;}
    public InputAction runAction  {get; private set;}
    public InputAction menuAction { get; private set; }
    
    public InputAction inventoryAction { get; private set; }

    public InputAction interactAction { get; private set; }

    public InputAction attackAction { get; private set; }

    private void Awake()
    {
        InputSystem.actions.Enable();
        if(instance == null)
        {
            instance = this;
        }
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        runAction = InputSystem.actions.FindAction("Run");
        menuAction = InputSystem.actions.FindAction("Menu");
        inventoryAction = InputSystem.actions.FindAction("Inventory");
        interactAction = InputSystem.actions.FindAction("Interact");
        attackAction = InputSystem.actions.FindAction("Attack");

    }


    public void DisablePlayerActions()
    {
        moveAction.Disable();
        jumpAction.Disable();
        runAction.Disable();
        menuAction.Disable();
        attackAction.Disable();
        inventoryAction.Disable();
        interactAction.Disable();
    }

    public void EnablePlayerActions()
    {
        moveAction.Enable();
        jumpAction.Enable();
        runAction.Enable();
        menuAction.Enable();
        attackAction.Enable();
        inventoryAction.Enable();
        interactAction.Enable();
    }
}
