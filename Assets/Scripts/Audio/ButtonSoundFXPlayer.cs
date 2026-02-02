using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundFXPlayer : MonoBehaviour
{
    public AudioClip[] audioClips;

    public void PlaySoundFX()
    {
        SoundFXManager.Instance.PlaySoundFXClip(audioClips, transform);
    }
}
