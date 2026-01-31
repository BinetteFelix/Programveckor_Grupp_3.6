using SoundSystem;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class FriendDialogue : MonoBehaviour
{
    [NonSerialized] public DialogBox dialogBox;
    float time = 0f;

    Color tessaCollor;
    Color opalCollor;

    void Start()
    {
        tessaCollor = new Color(0, 0, 1);
        opalCollor = new Color(1, 0, 0);
        dialogBox = SceneController.Instance.dialogBox;
    }

    void Update()
    {
        
        if (InputManager.instance.skipAction.IsPressed() && !SceneController.Instance.creditsUi.activeSelf)
        {
            time += Time.fixedDeltaTime;
            Debug.Log("test" + time);
            if(time >= 1.5f)
            {
                ActivateCredits();
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
            //MusicManager.Instance.PlayMusic(creditsSong, );
            SceneController.Instance.dialogBox.gameObject.SetActive(true);
            dialogBox.skipTextTip.SetActive(false);
            StartCoroutine(TriggerDialog());

        }


    }

    IEnumerator TriggerDialog()
    {
        StartCoroutine(dialogBox.scrollText("Tessa:", "Opal… is that you?", tessaCollor));
        while (dialogBox.isScrolling)
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(dialogBox.scrollText("Opal:", "Tessa? ... Tessa!", opalCollor));
        while (dialogBox.isScrolling)
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(dialogBox.scrollText("Opal:", "How did you find me?", opalCollor));
        while (dialogBox.isScrolling)
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(dialogBox.scrollText("Tessa:", "Long story, but at least we're together now...", tessaCollor));
        while (dialogBox.isScrolling)
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2.5f);
        ActivateCredits();
    }

    private void ActivateCredits()
    {
        SceneController.Instance.creditsUi.SetActive(true);
        MusicManager.Instance.CeaseMusic();
        dialogBox.skipTextTip.SetActive(true);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = new Vector3(-16.1900005f, -3.75999999f, 0);
        SaveController.Instance.SaveGame();
    }
}
