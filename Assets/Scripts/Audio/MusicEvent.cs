using UnityEngine.Audio;
using UnityEngine;

namespace SoundSystem
{
    public enum LayerType
    {
        Additive,
        Single
    }

    [CreateAssetMenu(menuName = "SoundSystem/Music Event", fileName = "Mus_")]
    public class MusicEvent : ScriptableObject
    {
        [SerializeField] AudioClip[] _musicLayers;
        [SerializeField] LayerType _layerType = LayerType.Additive;
        [SerializeField] AudioMixerGroup _mixer;

        public AudioClip[] MusicLayers => _musicLayers;
        public LayerType LayerType => _layerType;
        public AudioMixerGroup Mixer => _mixer;

        public void Play(float fadeTime)
        {
            MusicManager.Instance.PlayMusic(this, _mixer);
        }
        public void Stop()
        {
            MusicManager.Instance.StopMusic(this, _mixer);
        }
    }
}