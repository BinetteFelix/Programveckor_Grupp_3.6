using SoundSystem;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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
    [SerializeField] private GameObject SaveButton;
    [SerializeField] private GameObject MainMenuUI;
    [SerializeField] private GameObject UpdatePanel;

   [SerializeField] private GameObject DeathPanel;
    [SerializeField] private GameObject HealthUI;
    [SerializeField] public GameObject ControlsPopup;
    [SerializeField] private GameObject textBox;
    [SerializeField] public DialogBox dialogBox;

    public bool gameOver;
    private bool isScrolling;
    private string tempText;
    public GameObject creditsUi;

    [SerializeField] private AudioClip[] deathSoundFx;

    #region UI ANIMATIONS
    [SerializeField] private Animator settingsAnimator;
    [SerializeField] private Animator controlsAnimator;
    [SerializeField] private Animator updateAnimator;
    [SerializeField] private Animator pauseAnimator;
    #endregion

    #region MUSIC 
    [SerializeField] private AudioMixerGroup mixerGroup;
    [SerializeField] private MusicEvent _songA;
    [SerializeField] private MusicEvent _songB;
    #endregion

    #region STATE PARAMETERS
    public bool IsPaused { get; private set; }
    public bool HasFinishedLoading { get; private set; }
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

    public int CurrentScene()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public void SetSelectedButton(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(button);
    }

    private void Update()
    {

        if (InputManager.instance.menuAction.WasPressedThisFrame() && CurrentScene() != 0 && !InventoryMenu.activeSelf && !SettingsMenu.activeSelf)
        {
            TogglePause();
        }
        if (InputManager.instance.inventoryAction.WasPressedThisFrame() && !PauseMenu.activeSelf && CurrentScene() != 0)
        {
            foreach (Transform child in UserInterfaceObject.transform.Find("Canvas"))
            {
                if (child.gameObject.activeSelf && child.gameObject != InventoryMenu) return;
                ToggleInventory();
                foreach (Transform slot in InventoryManager.Instance.InventoryGrid)
                {
                    if (slot != null) SetSelectedButton(slot.gameObject);
                    break;
                }

            }
        }

        if(Application.isEditor && InputManager.instance.teleportSceneAction.WasPressedThisFrame())
        {
            StartCoroutine(LoadScene(2));
        }
    }

    #region OPENING & CLOSING PANELS
    public void TogglePause()
    {
        IsPaused = !PauseMenu.activeSelf;
        PauseMenu.SetActive(IsPaused);
        if (PauseMenu.activeSelf)
            pauseAnimator.SetTrigger("Pause");
        SettingsMenu.SetActive(false);
        SetSelectedButton(ResumeButton);
        Time.timeScale = PauseMenu.activeSelf ? 0.0f : 1.0f;
        if(PauseMenu.activeSelf)
        {
            MusicManager.Instance.PauseMusic(_songB, mixerGroup);   
        }
        else
        {
            MusicManager.Instance.PlayMusic(_songB, mixerGroup);
        }
    }

    public void CloseSettingsMenu()
    {
        if (CurrentScene() != 0)
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

        if (CurrentScene() == 0)
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
    public void ExitGame()
    {
        Application.Quit();
    }
    bool IsPlayingAnimation(Animator anim, string stateName)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return true;
        else
            return false;
    }
    public void SendGameUpdate(string update)
    {
        UpdatePanel.GetComponentInChildren<TextMeshProUGUI>().text = update;
        updateAnimator.SetTrigger("Update");
    }
    public IEnumerator LoadScene(int index)
    {
        HasFinishedLoading = false;
        yield return new WaitForSecondsRealtime(0f);
        SceneManager.LoadScene(index);

        LoadingMenu.SetActive(true);
        if(index != 0) MainMenuUI.SetActive(false);
        if(index == 1)
        {
            Debug.Log("start dialog");
            dialogBox.gameObject.SetActive(true);
            StartCoroutine(dialogBox.scrollText("Narrator", "In a vast forest filled with a wide range of animals and vegetation alike, Tessa has a mission. A mission to save her lost best friend, Opal. ", new Color(0.48f, 0.44f, 0.44f)));
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

        if (CurrentScene() == 0)
        {
            MusicManager.Instance.StopMenuMusic(_songA, mixerGroup);
        }
        else if (CurrentScene() == 1)
            MusicManager.Instance.StopMenuMusic(_songB, mixerGroup);

        yield return new WaitForSecondsRealtime(0.1f);

        if (CurrentScene() == 0)
            MusicManager.Instance.PlayMenuMusic(_songA, mixerGroup);
        else
            MusicManager.Instance.PlayMenuMusic(_songB, mixerGroup);
    }

    public void TriggerGameOver()
    {
        gameOver = true;
        TurnOffAllUI();
        MusicManager.Instance.CeaseMusic();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        SoundFXManager.Instance.PlaySoundFXClip(deathSoundFx, player.transform);
        player.GetComponent<SpriteRenderer>().color = Color.red;

        InputManager.instance.DisablePlayerActions();
        DeathPanel.SetActive(true);

        GameObject restartButton = DeathPanel.transform.Find("Panel").transform.Find("RestartButton").gameObject;
        SetSelectedButton(restartButton);

    }

    public void TextEnable()
    {
        InputManager.instance.DisablePlayerActions();
        textBox.SetActive(true);
    }
}