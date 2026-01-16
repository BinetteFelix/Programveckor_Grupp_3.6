using Unity.Cinemachine;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public IOData ioData;
    public NPCData npcData;
    private CinemachineImpulseSource CMISource;
    public int NormalSceneIndex;
    public int DistortedSceneIndex;

    public bool IsWarping {  get; set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CMISource = GetComponent<CinemachineImpulseSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneController.Instance.HasFinishedLoading)
            IsWarping = false;
    }

    public void PickUpItem()
    {
        Destroy(gameObject);
    }

    public void TalkToPlayer()
    {

    }
    public void InteractWarpObject()
    {
        IsWarping = true;
        Debug.Log("startcamerashake");
        
        CMISource.GenerateImpulse();
       
        if (SceneController.Instance.CurrentOpenScene == NormalSceneIndex)
            StartCoroutine(SceneController.Instance.LoadScene(2));
        else if (SceneController.Instance.CurrentOpenScene == DistortedSceneIndex)
            StartCoroutine(SceneController.Instance.LoadScene(1));
    }
}