using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Credits : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * 75 * Time.deltaTime;
        if (InputManager.instance.skipAction.WasPressedThisFrame())
        {
            SceneController.Instance.ReturnToMenu();
        }
        if(transform.localPosition.y >= 2150)
        {
            SceneController.Instance.ReturnToMenu();
            InputManager.instance.EnablePlayerActions();
        }
    }

    private void OnEnable()
    {
        InputManager.instance.DisablePlayerActions();
    }
}
