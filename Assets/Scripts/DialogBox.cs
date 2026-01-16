using TMPro;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class DialogBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textBoxText;
    public TextMeshProUGUI nameText;
    public bool isScrolling = false;
    
    
    public IEnumerator scrollText(string dialogue)
    {
        
        if (isScrolling) StopCoroutine(scrollText(""));
        isScrolling = true;

        for (int i = 0; i < dialogue.Length; i++)
        {
            textBoxText.text += dialogue.Substring(i, 1);
            yield return new WaitForSeconds(0.1f);
            isScrolling = false;
        }
        //if (skipped == false) ControlsPopup.SetActive(true);
        yield return new WaitForSeconds(2f);
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            SceneController.Instance.ControlsPopup.SetActive(true);
            this.gameObject.SetActive(false);
        }

    }

    private void OnEnable()
    {
        textBoxText.text = "";
        InputManager.instance.DisablePlayerActions();
    }

    private void OnDisable()
    {
        InputManager.instance.EnablePlayerActions();
    }
}
