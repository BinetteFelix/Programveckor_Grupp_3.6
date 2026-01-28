using TMPro;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class DialogBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueBoxText;
    public TextMeshProUGUI dialogueBoxTitle;
    public bool isScrolling = false;
    public bool skipped = false;
    public float scrollSpeed = 0.08f;
    public GameObject skipTextTip;
    
    public IEnumerator scrollText(string titleText, string dialogue, Color textColor)
    {
        if (isScrolling) StopCoroutine(scrollText("", "", dialogueBoxTitle.color));
        dialogueBoxText.text = "";
        dialogueBoxTitle.text = "";
        dialogueBoxTitle.color = textColor;
        Debug.Log(textColor);
        isScrolling = true;
        dialogueBoxTitle.text = titleText;

        for (int i = 0; i < dialogue.Length; i++)
        {
            if (skipped && SceneController.Instance.CurrentScene() != 2) break; //only stop scrolling the text if its not level 2, i gave up on making it so u can skip the end dialogue.
            dialogueBoxText.text += dialogue.Substring(i, 1);
            yield return new WaitForSeconds(scrollSpeed);
        }
        isScrolling = false;
        
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            yield return new WaitForSeconds(3);
        }
        else
        {
            yield return new WaitForSeconds(4);
        }

        if (SceneManager.GetActiveScene().buildIndex == 1 && !skipped)
        {
            SceneController.Instance.ControlsPopup.SetActive(true);
            this.gameObject.SetActive(false);
            skipped = false;
        }
    }
    private void Update()
    {
        if (InputManager.instance.skipAction.WasPressedThisFrame())
        {
            skipped = true;
        }



        if(skipped && SceneManager.GetActiveScene().buildIndex == 1)
        {
            StopCoroutine(scrollText("", "", dialogueBoxTitle.color));
            InputManager.instance.EnablePlayerActions();
            this.gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        Debug.Log("Enable Dialog");
        skipped = false;
        dialogueBoxText.text = "";
        dialogueBoxTitle.text = "";
        InputManager.instance.DisablePlayerActions();
    }

    private void OnDisable()
    {
        Debug.Log("Disable Dialog");
        dialogueBoxText.text = "";
        dialogueBoxTitle.text = "";
    }
}