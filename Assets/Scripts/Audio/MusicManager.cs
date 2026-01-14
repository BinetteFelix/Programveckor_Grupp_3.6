using SoundSystem;
using UnityEngine;
using UnityEngine.Audio;

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
    public void PlayMusic(MusicEvent musicEvent, AudioMixerGroup audioMixerGroup)
    {
        if (!IsPlayingClip)
        {
            _audioSource.clip = musicEvent.MusicLayers[0];
            _audioSource.outputAudioMixerGroup = audioMixerGroup;
            _audioSource.Play();
        }
    }
    public void StopMusic(MusicEvent musicEvent, AudioMixerGroup audioMixerGroup)
    {
        if (IsPlayingClip)
        {
            _audioSource.clip = musicEvent.MusicLayers[0];
            _audioSource.outputAudioMixerGroup = audioMixerGroup;
            _audioSource.Stop();
        }
    }

    public void PlayMenuMusic(MusicEvent song, AudioMixerGroup audioMixerGroup)
    {
        if (!_audioSource.isPlaying && _audioSource.clip != song)
        {
            PlayMusic(song, audioMixerGroup);
        }   
    }
    public void StopMenuMusic(MusicEvent song, AudioMixerGroup audioMixerGroup)
    {
        StopMusic(song, audioMixerGroup);
    }
}