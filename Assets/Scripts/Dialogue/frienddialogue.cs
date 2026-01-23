using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class frienddialogue : MonoBehaviour
{
    public DialogBox dialogBox;
    float time = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogBox = SceneController.Instance.dialogBox;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Application.isEditor && InputManager.instance.skipAction.IsPressed())
        {
            time += Time.fixedDeltaTime;
            Debug.Log("test" + time);
            if(time >= 1.5f)
            {
                SceneController.Instance.creditsUi.SetActive(true);
                dialogBox.skipTextTip.SetActive(true);
                StopCoroutine(TriggerDialog());
                time = 0f;
            }

        }
        else time = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            SaveController.Instance.SaveGame();
            InputManager.instance.DisablePlayerActions();
            SceneController.Instance.dialogBox.gameObject.SetActive(true);
            dialogBox.skipTextTip.SetActive(false);
            StartCoroutine(TriggerDialog());

        }


    }

    IEnumerator TriggerDialog()
    {
        StartCoroutine(dialogBox.scrollText("Tessa:", "Opal… is that you?"));
        while (dialogBox.isScrolling)
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(dialogBox.scrollText("Opal:", "Tessa? ... Tessa!"));
        while (dialogBox.isScrolling)
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(dialogBox.scrollText("Opal:", "How did you find me?"));
        while (dialogBox.isScrolling)
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(dialogBox.scrollText("Tessa:", "Long story, but at least we're together now..."));
        while (dialogBox.isScrolling)
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2.5f);
        SceneController.Instance.creditsUi.SetActive(true);
        dialogBox.skipTextTip.SetActive(true);
    }
}
