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
}