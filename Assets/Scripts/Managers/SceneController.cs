using SoundSystem;
using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject InventoryMenu;
    [SerializeField] private GameObject SettingsMenu;
    [SerializeField] private GameObject LoadingMenu;

    [SerializeField] private GameObject UserInterfaceObject;
    [SerializeField] private GameObject ResumeButton;
    [SerializeField] private GameObject PlayButton;
    [SerializeField] private GameObject MainMenuUI;

    [SerializeField] private GameObject DeathPanel;
    [SerializeField] private GameObject HealthUI;
    [SerializeField] private GameObject ControlsPopup;

    public bool gameOver;

    #region UI ANIMATIONS
    [SerializeField] private Animator settingsAnimator;
    [SerializeField] private Animator controlsAnimator;

    #endregion

    #region MUSIC 
    [SerializeField] private AudioMixerGroup mixerGroup;
    [SerializeField] private MusicEvent _songA;
    [SerializeField] private MusicEvent _songB;
    #endregion

    #region STATE PARAMETERS
    public bool IsPaused { get; private set; }
    public bool HasFinishedLoading { get; private set; }
    public int CurrentOpenScene { get; private set; }
    #endregion

    #region TIMERS
    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            MusicManager.Instance.PlayMenuMusic(_songA, mixerGroup);
    }

    public bool IsMainMenuScene()
    {
        return (CurrentOpenScene == 0);
    }

    public void SetSelectedButton(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(button);
    }

    private void Update()
    {
        CurrentOpenScene = SceneManager.GetActiveScene().buildIndex;

        if (InputManager.instance.menuAction.WasPressedThisFrame() && !IsMainMenuScene() && !InventoryMenu.activeSelf && !SettingsMenu.activeSelf)
        {
            TogglePause();
        }
        if (InputManager.instance.inventoryAction.WasPressedThisFrame() && !PauseMenu.activeSelf && !IsMainMenuScene())
        {
            foreach (Transform child in UserInterfaceObject.transform.Find("Canvas"))
            {
                if (child.gameObject.activeSelf && child.gameObject != InventoryMenu) return;
                ToggleInventory();
                foreach (Transform grid in InventoryManager.Instance.InventoryGrid)
                {
                    if (grid != null) SceneController.Instance.SetSelectedButton(grid.gameObject);
                    break;
                }
            }
        }
    }

    #region OPENING & CLOSING PANELS
    public void TogglePause()
    {
        IsPaused = !PauseMenu.activeSelf;
        PauseMenu.SetActive(IsPaused);
        SettingsMenu.SetActive(false);
        SetSelectedButton(ResumeButton);
        Time.timeScale = PauseMenu.activeSelf ? 0.0f : 1.0f;
    }

    public void CloseSettingsMenu()
    {
        if (!IsMainMenuScene())
        {
            PauseMenu.SetActive(true);
            SettingsMenu.SetActive(false);
        }
        else
        {
            SettingsMenu.SetActive(false);
            MainMenuUI.SetActive(true);
            PauseMenu.SetActive(false);
        }
    }
    public void OpenSettings()
    {
        SettingsMenu.SetActive(true);
        PauseMenu.SetActive(false);
    }

    public void GoBack()
    {
        
    }

    public void RestoreMenuButtons()
    {
        GameObject titleText;
        GameObject buttons;
        GameObject playButton;

        titleText = MainMenuUI.transform.Find("TitleText").gameObject;
        buttons = MainMenuUI.transform.Find("Buttons").gameObject;
        playButton = buttons.transform.Find("PlayButton").gameObject;

        if (IsMainMenuScene())
        {
            titleText.SetActive(true);
            buttons.SetActive(true);
            SetSelectedButton(playButton);
        }
    }

    public void ToggleInventory()
    {
        InventoryMenu.SetActive(!InventoryMenu.activeSelf);
        if(InventoryMenu.activeSelf)
        {
            InputManager.instance.DisablePlayerActions();
            InputManager.instance.inventoryAction.Enable();
        } else
        {
            InputManager.instance.EnablePlayerActions();
        }

            #region Close ItemInformationTab
            GameObject itemInformationParent = GameObject.FindGameObjectWithTag("ItemInformation");

        if (itemInformationParent != null)
        {
            itemInformationParent.GetComponent<ItemInformationContent>().selectedItemSprite.gameObject.SetActive(false);
            itemInformationParent.GetComponent<ItemInformationContent>().selectedItemName.gameObject.SetActive(false);
            itemInformationParent.GetComponent<ItemInformationContent>().selectedItemDescription.gameObject.SetActive(false);
        }
        #endregion
    }
    #endregion

    public IEnumerator LoadScene(int index)
    {
        HasFinishedLoading = false;
        yield return new WaitForSecondsRealtime(0f);
        SceneManager.LoadScene(index);

        LoadingMenu.SetActive(true);
        if(index != 0) MainMenuUI.SetActive(false);
        if(index == 1)
        {
            ControlsPopup.SetActive(true);
            HealthUI.SetActive(true);
        }
        if(index == 0)
        {
            SetSelectedButton(PlayButton);
        }
        StartCoroutine(Loading());

    }

    public void ReturnToMenu()
    {
        TogglePause();
        StartCoroutine(LoadScene(0));
        TurnOffAllUI();
        MainMenuUI.SetActive(true);
        gameOver = false;
    }

    public void TurnOffAllUI()
    {
        foreach (Transform child in UserInterfaceObject.transform.Find("Canvas"))
        {
            child.gameObject.SetActive(false);
        }
    }

    public void ChangeScene(int sceneIndex)
    {
        StartCoroutine(LoadScene(sceneIndex));
    }
    private IEnumerator Loading()
    {
        yield return new WaitForSecondsRealtime(1);
        LoadingMenu.SetActive(false);
        Time.timeScale = 1.0f;
        HasFinishedLoading = true;
        controlsAnimator.SetTrigger("Open");

        if (IsMainMenuScene())
        {
            MusicManager.Instance.StopMenuMusic(_songA, mixerGroup);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
            MusicManager.Instance.StopMenuMusic(_songB, mixerGroup);

        yield return new WaitForSecondsRealtime(0.1f);

        if (IsMainMenuScene())
            MusicManager.Instance.PlayMenuMusic(_songA, mixerGroup);
        else
            MusicManager.Instance.PlayMenuMusic(_songB, mixerGroup);
    }

    public void TriggerGameOver()
    {
        gameOver = true;
        TurnOffAllUI();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<SpriteRenderer>().enabled = false;

        InputManager.instance.DisablePlayerActions();
        DeathPanel.SetActive(true);

        GameObject restartButton = DeathPanel.transform.Find("Panel").transform.Find("RestartButton").gameObject;
        SetSelectedButton(restartButton);
    }
}