using System.Collections;
using TMPro;
using UnityEngine;

public class frienddialogue : MonoBehaviour
{

    [SerializeField] public GameObject creditsObject;
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
            InputManager.instance.DisablePlayerActions();
            SceneController.Instance.dialogBox.gameObject.SetActive(true);
            TriggerDialog();

        }
    }

    IEnumerator TriggerDialog()
    {
        dialogBox.nameText.text = "Tessa:";
        StartCoroutine(dialogBox.scrollText("Opal…is that you?"));

        yield return new WaitForSeconds(5f);

        dialogBox.nameText.text = "Opal:";
        StartCoroutine(dialogBox.scrollText("Tessa? ... Tessa!"));
        yield return new WaitForSeconds(4f);
        StartCoroutine(dialogBox.scrollText("How did you find me?"));

        yield return new WaitForSeconds(5f);

        dialogBox.nameText.text = "Tessa:";
        StartCoroutine(dialogBox.scrollText("Long story, but at least we're together"));

        yield return new WaitForSeconds(5f);

        creditsObject.SetActive(true);

    }
}
