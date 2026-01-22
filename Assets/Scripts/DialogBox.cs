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
    public float scrollSpeed = 0.05f;
    public GameObject skipTextTip;
    
    
    public IEnumerator scrollText(string titleText, string dialogue)
    {
        if (isScrolling) StopCoroutine(scrollText("", ""));
        dialogueBoxText.text = "";
        dialogueBoxTitle.text = "";
        isScrolling = true;
        dialogueBoxTitle.text = titleText;

        for (int i = 0; i < dialogue.Length; i++)
        {
            if (skipped && SceneController.Instance.CurrentOpenScene != 2) break; //only stop scrolling the text if its not level 2, i gave up on making it so u can skip the end dialog.
            dialogueBoxText.text += dialogue.Substring(i, 1);
            yield return new WaitForSeconds(scrollSpeed);
        }
        isScrolling = false;
        if (skipped) skipped = false;
        //if (skipped == false) ControlsPopup.SetActive(true);
        yield return new WaitForSeconds(2f);
        if(SceneManager.GetActiveScene().buildIndex == 1 && !skipped)
        {
            SceneController.Instance.ControlsPopup.SetActive(true);
            this.gameObject.SetActive(false);
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
        InputManager.instance.EnablePlayerActions();
    }
}
