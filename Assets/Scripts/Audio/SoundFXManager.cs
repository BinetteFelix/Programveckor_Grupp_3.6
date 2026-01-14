using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance;

    [SerializeField] private AudioSource soundFXObject;

    [Range(0.5f, 5f)] public float SoundFXDelay;
    private bool canPlayClip;

    public float LastPlayedSoundFX {  get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

    }
    private void Update()
    {
        LastPlayedSoundFX -= Time.deltaTime;

        if (LastPlayedSoundFX < 0)
            canPlayClip = true;
        else
            canPlayClip = false;
    }
    public void PlaySoundFXClip(AudioClip[] audioClips, Transform spawnTransform)
    {
        if (canPlayClip)
        {
            AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

            int randomClip = Random.Range(0, audioClips.Length);
            audioSource.resource = audioClips[randomClip];

            audioSource.Play();

            float clipLength = audioSource.clip.length;

            float randomPitch = Random.Range(0.95f, 1.05f);

            audioSource.pitch = randomPitch;

            Destroy(audioSource.gameObject, clipLength);

            LastPlayedSoundFX = SoundFXDelay;
        }
    }
}