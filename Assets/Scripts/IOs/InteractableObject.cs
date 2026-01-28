using Unity.Cinemachine;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public int NormalSceneIndex;
    public int DistortedSceneIndex;

    private InputManager inputs;
    [SerializeField] LayerMask playerLayer;
    public bool IsWarping {  get; set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputs = FindAnyObjectByType<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputs.interactAction.WasPressedThisFrame() && Physics2D.OverlapCircle(transform.position, 2, playerLayer))
        {
            InteractWarpObject();
        }
    }
    public void InteractWarpObject()
    {
        if (SceneController.Instance.CurrentScene() == NormalSceneIndex)
        {
            SaveController.Instance.SaveGame();
            StartCoroutine(SceneController.Instance.LoadScene(2));

        }
        else if (SceneController.Instance.CurrentScene() == DistortedSceneIndex)
        {
            SaveController.Instance.SaveGame();
            StartCoroutine(SceneController.Instance.LoadScene(1));
        }
    }
}