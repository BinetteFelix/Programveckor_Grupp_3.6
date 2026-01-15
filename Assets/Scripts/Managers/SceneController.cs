using SoundSystem;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
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
    [SerializeField] private GameObject MainMenuUI;

    [SerializeField] private GameObject ControlsPopup;

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

    private bool IsMainMenuScene()
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
                Debug.Log(child.gameObject.name);
                ToggleInventory();
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
            SettingsMenu.SetActive(false);
            PauseMenu.SetActive(true);
        }
        else
        {
            MainMenuUI.SetActive(true);
            PauseMenu.SetActive(false);
            SettingsMenu.SetActive(false);
        }
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
        MainMenuUI.SetActive(true);
    }

    public void ChangeScene(int sceneIndex)
    {
        Debug.Log("tried changing scene!");
        
        StartCoroutine(LoadScene(sceneIndex));
    }
    private IEnumerator Loading()
    {
        yield return new WaitForSecondsRealtime(1);
        LoadingMenu.SetActive(false);
        Time.timeScale = 1.0f;
        HasFinishedLoading = true;
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            MusicManager.Instance.StopMenuMusic(_songA, mixerGroup);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
            MusicManager.Instance.StopMenuMusic(_songB, mixerGroup);

        yield return new WaitForSecondsRealtime(0.1f);

        if (SceneManager.GetActiveScene().buildIndex == 0)
            MusicManager.Instance.PlayMenuMusic(_songA, mixerGroup);
        else
            MusicManager.Instance.PlayMenuMusic(_songB, mixerGroup);
    }
}