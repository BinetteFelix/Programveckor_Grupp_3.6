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
    private bool skipped = false;
    
    
    public IEnumerator scrollText(string titleText, string dialogue)
    {
        if (isScrolling) StopCoroutine(scrollText("", ""));
        dialogueBoxText.text = "";
        dialogueBoxTitle.text = "";
        isScrolling = true;

        for (int i = 0; i < dialogue.Length; i++)
        {
            dialogueBoxText.text += dialogue.Substring(i, 1);
            yield return new WaitForSeconds(0.05f);
        }
        isScrolling = false;
        //if (skipped == false) ControlsPopup.SetActive(true);
        yield return new WaitForSeconds(2f);
        if(SceneManager.GetActiveScene().buildIndex == 1)
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
        else skipped = false;

        if(skipped && SceneManager.GetActiveScene().buildIndex == 1)
        {
            this.gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        dialogueBoxText.text = "";
        dialogueBoxTitle.text = "";
        InputManager.instance.DisablePlayerActions();
    }

    private void OnDisable()
    {
        InputManager.instance.EnablePlayerActions();
    }
}
