using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public InputAction moveAction { get; private set; }
    public InputAction jumpAction {get; private set;}
    public InputAction runAction  {get; private set;}
    public InputAction menuAction { get; private set; }

    private PlayerInput playerInput;
    

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        playerInput = this.GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        runAction = playerInput.actions["Run"];
        menuAction = playerInput.actions["Menu"];
    }
}
