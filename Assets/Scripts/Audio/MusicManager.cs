using SoundSystem;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    public static MusicManager Instance
    {
        get 
        {
            if(instance == null)
            {
                instance = FindFirstObjectByType<MusicManager>();
                if(instance == null)
                {
                    GameObject singletonGO = new GameObject("MusicManager_singleton");
                    instance = singletonGO.AddComponent<MusicManager>();

                    DontDestroyOnLoad(singletonGO);
                }
            }
            return instance;
        } 
    }

    AudioSource _audioSource;
    public bool IsPlayingClip { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
            instance = this;

        SetupMusicPlayers();
    }
    private void Update()
    {
        IsPlayingClip = _audioSource.isPlaying;
    }

    void SetupMusicPlayers()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
    }
    public void PlayMusic(MusicEvent musicEvent, float fadeTime)
    {
        if (!IsPlayingClip)
        {
            _audioSource.clip = musicEvent.MusicLayers[0];
            _audioSource.Play();
        }
    }
}