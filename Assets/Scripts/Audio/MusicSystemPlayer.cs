using SoundSystem;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MusicSystemPlayer : MonoBehaviour
{
    public static MusicSystemPlayer Instance;

    private float SongTime;

    [SerializeField] private MusicEvent[] songs;

    MusicEvent currentSong;
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
            MusicManager.Instance.PlayMusic(songs[0], mixerGroup);
        }
        else 
        {
            MusicManager.Instance.StopMusic(songs[0], mixerGroup);
        }
    }
    private void Update()
    {
        SongTime -= Time.deltaTime;

        if (currentSong.MusicLayers[0].length - SongTime == 0)
        {

        }
    }

    private void SwitchMusic(MusicEvent currentMusic)
    {
        if (currentMusic == null)
        {
            MusicManager.Instance.PlayMusic(songs[0], mixerGroup);
            currentSong = currentMusic;
        }
        else if (currentSong != currentMusic)
        {
            MusicManager.Instance.PlayMusic(songs[+1], mixerGroup);
        }
    }
}