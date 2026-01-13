using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyUserInterface : MonoBehaviour
{
    public static DontDestroyUserInterface Instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void MoveObjectToScene()
    {
        //SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
    }
}