using Unity.Cinemachine;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public IOData Data;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        switch (Data.interactMethod)
        {
            case 1:
                


            case 2:

            case 3:

            default:
                return;

        }
        
    }

    private void WarpObject()
    {
        CinemachineImpulseSource CMISource = GetComponent<CinemachineImpulseSource>();
        CMISource.GenerateImpulse();
       
        if (SceneController.Instance.CurrentOpenScene == 1)
            SceneController.Instance.StartCoroutine(SceneController.Instance.LoadScene(2));
        else if (SceneController.Instance.CurrentOpenScene == 2)
            SceneController.Instance.StartCoroutine(SceneController.Instance.LoadScene(1));
    }

    private void OnValidate()
    {
        gameObject.layer = Data.interactionLayer;
        gameObject.tag = Data.objectTag;
    }
}