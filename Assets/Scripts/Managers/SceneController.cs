using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject SettingsMenu;
    [SerializeField] private GameObject LoadingMenu;

    #region STATE PARAMETERS
    public bool IsPaused { get; private set; }

    public int CurrentOpenScene { get; private set; }
    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    private void Start()
    {
    }
    private void Update()
    {
        DontDestroyOnLoad(gameObject);
        CurrentOpenScene = SceneManager.GetActiveScene().buildIndex;

        if (Input.GetButtonUp("Pause"))
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

    public IEnumerator LoadScene(int index)
    {
        yield return new WaitForSeconds(4.5f);

        SceneManager.LoadScene(index);
        LoadingMenu.SetActive(true);

        StartCoroutine(Loading());
    }
    private IEnumerator Loading()
    {
        yield return new WaitForSecondsRealtime(1);
        LoadingMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }
}