using System.Collections;
using UnityEngine;
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
    [SerializeField] private PlayerInput playerInput;

    #region STATE PARAMETERS
    public bool IsPaused { get; private set; }
    public bool HasFinishedLoading { get; private set; }
    public int CurrentOpenScene { get; private set; }
    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(UserInterfaceObject);
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
        CurrentOpenScene = SceneManager.GetActiveScene().buildIndex;

        if (InputManager.instance.menuAction.WasPressedThisFrame() && CurrentOpenScene != 0)
        {
            TogglePause();
        }
    }

    #region OPENING & CLOSING PANELS
    public void TogglePause()
    {
        IsPaused = !PauseMenu.activeSelf;
        PauseMenu.SetActive(IsPaused);
        SettingsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(ResumeButton);
        Time.timeScale = PauseMenu.activeSelf ? 0.0f : 1.0f;
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

    public void SetSelectedGameObject(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(button);
    }
    #endregion

    public IEnumerator LoadScene(int index)
    {
        HasFinishedLoading = false;
        yield return new WaitForSecondsRealtime(0f);

        SceneManager.LoadScene(index);
        LoadingMenu.SetActive(true);

        StartCoroutine(Loading());
    }

    public void ReturnToMenu()
    {
        SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());
        SceneManager.MoveGameObjectToScene(UserInterfaceObject, SceneManager.GetActiveScene());
        TogglePause();
        StartCoroutine(LoadScene(0));
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
    }
}