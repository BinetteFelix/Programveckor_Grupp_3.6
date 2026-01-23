using UnityEngine;
using UnityEngine.EventSystems;

public class ControlsPopup : MonoBehaviour
{
    [SerializeField] private GameObject ExitButton;
    void Start()
    {

    }


    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(ExitButton);
        InputManager.instance.DisablePlayerActions();
    }
    public void EnableActions()
    {
        InputManager.instance.EnablePlayerActions();
    }
}
