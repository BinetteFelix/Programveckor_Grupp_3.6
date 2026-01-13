using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject LoadingMenu;

    public void LoadScene(int index)
    {
        StartCoroutine(StartLoadingScene(index));
    }

    private IEnumerator StartLoadingScene(int index)
    {
        LoadingMenu.SetActive(true);
        yield return new WaitForSecondsRealtime(1);
        SceneManager.LoadScene(index);
    }

    public void SetSelectedGameObject(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(button);
    }
}
