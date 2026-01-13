using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance;

    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void PlaySoundFXClip(AudioClip[] audioClips, Transform spawnTransform)
    {
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        int randomClip = Random.Range(0, audioClips.Length);
        audioSource.resource = audioClips[randomClip];

        if (!audioSource.isPlaying)
            audioSource.Play();

        float clipLength = audioSource.clip.length;

        float randomPitch = Random.Range(0.95f, 1.05f);

        audioSource.pitch = randomPitch;

        Destroy(audioSource.gameObject, clipLength);
    }
}