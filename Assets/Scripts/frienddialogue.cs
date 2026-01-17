using System.Collections;
using TMPro;
using UnityEngine;

public class frienddialogue : MonoBehaviour
{
    public DialogBox dialogBox;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogBox = SceneController.Instance.dialogBox;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            SaveController.Instance.SaveGame();
            InputManager.instance.DisablePlayerActions();
            SceneController.Instance.dialogBox.gameObject.SetActive(true);
            StartCoroutine(TriggerDialog());

        }
    }

    IEnumerator TriggerDialog()
    {
        dialogBox.nameText.text = "Tessa:";
        StartCoroutine(dialogBox.scrollText("Opal… is that you?"));
        yield return new WaitForSeconds(3f);
        dialogBox.ResetText();
        yield return new WaitForSeconds(2.5f);
        dialogBox.nameText.text = "Opal:";
        StartCoroutine(dialogBox.scrollText("Tessa? ... Tessa!"));
        yield return new WaitForSeconds(1f);
        dialogBox.ResetText();
        yield return new WaitForSeconds(3f);
        dialogBox.nameText.text = "Opal:";
        StartCoroutine(dialogBox.scrollText("How did you find me?"));

        yield return new WaitForSeconds(3f);
        dialogBox.ResetText();
        yield return new WaitForSeconds(1f);
        dialogBox.nameText.text = "Tessa:";
        StartCoroutine(dialogBox.scrollText("Long story, but at least we're together now..."));

        yield return new WaitForSeconds(4f);

        SceneController.Instance.creditsUi.SetActive(true);

    }
}
