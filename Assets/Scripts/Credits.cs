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
    }

    private void OnEnable()
    {
        InputSystem.actions.Disable();
        StartCoroutine(QuitGame());
    }

    IEnumerator QuitGame()
    {
        yield return new WaitForSeconds(15f);
        SceneController.Instance.ReturnToMenu();
    }
}
