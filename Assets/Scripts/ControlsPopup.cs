using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ControlsPopup : MonoBehaviour
{
    [SerializeField] private GameObject ExitButton;
    void Start()
    {
        Debug.Log("ControlPopup startmethod ran!");
        Debug.Log(ExitButton);
        EventSystem.current.SetSelectedGameObject(ExitButton);
        InputManager.instance.moveAction.Disable();
        InputManager.instance.jumpAction.Disable();
        InputManager.instance.menuAction.Disable();
    }

    private void Awake()
    {
        Debug.Log("ControlPopup startmethod ran!");
        Debug.Log(ExitButton);
        EventSystem.current.SetSelectedGameObject(ExitButton);
        InputManager.instance.moveAction.Disable();
        InputManager.instance.jumpAction.Disable();
        InputManager.instance.menuAction.Disable();
    }

    public void EnableActions()
    {
        InputManager.instance.moveAction.Enable();
        InputManager.instance.jumpAction.Enable();
        InputManager.instance.menuAction.Enable();
    }
}
