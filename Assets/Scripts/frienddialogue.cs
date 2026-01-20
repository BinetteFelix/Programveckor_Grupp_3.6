using System.Collections;
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
        StartCoroutine(dialogBox.scrollText("Tessa:", "Opal… is that you?"));
        yield return new WaitForSeconds(3f);
        StartCoroutine(dialogBox.scrollText("Opal:", "Tessa? ... Tessa!"));
        yield return new WaitForSeconds(3f);
        StartCoroutine(dialogBox.scrollText("Opal:", "How did you find me?"));
        yield return new WaitForSeconds(3f);
        StartCoroutine(dialogBox.scrollText("Tessa:", "Long story, but at least we're together now..."));
        yield return new WaitForSeconds(4f);
        SceneController.Instance.creditsUi.SetActive(true);
    }
}
