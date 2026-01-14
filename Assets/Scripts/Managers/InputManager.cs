using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public InputAction moveAction { get; private set; }
    public InputAction jumpAction {get; private set;}
    public InputAction runAction  {get; private set;}
    public InputAction menuAction { get; private set; }
    

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
    }
}
