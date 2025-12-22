using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject SettingsMenu;

    #region STATE PARAMETERS
    public bool IsPaused { get; private set; }
    #endregion

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        IsPaused = !PauseMenu.activeSelf;
        PauseMenu.SetActive(IsPaused);
        SettingsMenu.SetActive(false);

        Time.timeScale = PauseMenu.activeSelf ? 0.0f : 1.0f;
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
        Time.timeScale = 1.0f;
    }
}