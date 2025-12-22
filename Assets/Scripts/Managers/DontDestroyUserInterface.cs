using UnityEngine;

public class DontDestroyUserInterface : MonoBehaviour
{
    public static DontDestroyUserInterface Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        OnSceneLoad();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void OnSceneLoad()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
}