using SoundSystem;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MusicSystemPlayer : MonoBehaviour
{
    public static MusicSystemPlayer Instance;

    [SerializeField] private MusicEvent _songA;
    [SerializeField] private MusicEvent _songB;

    AudioMixerGroup mixerGroup;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        mixerGroup = GetComponent<AudioMixerGroup>();
    }
    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0 && FindFirstObjectByType<MusicManager>() != null)
        {
            MusicManager.Instance.PlayMusic(_songA, mixerGroup);
        }
        else 
        {
            MusicManager.Instance.StopMusic(_songA, mixerGroup);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            _songA.Play(2.5f);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            _songB.Play(2.5f);
        }
    }
}