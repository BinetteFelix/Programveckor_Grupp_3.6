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

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.resource = audioClip;

        audioSource.volume = volume;

        if (!audioSource.isPlaying)
            audioSource.Play();

        float clipLength = audioSource.clip.length;

        float randomPitch = Random.Range(0.95f, 1.05f);

        audioSource.pitch = randomPitch;

        Destroy(audioSource.gameObject, clipLength);
    }
}