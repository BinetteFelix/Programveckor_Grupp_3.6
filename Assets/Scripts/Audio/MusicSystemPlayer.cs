using SoundSystem;
using UnityEngine;

public class MusicSystemPlayer : MonoBehaviour
{
    [SerializeField] private MusicEvent _songA;
    [SerializeField] private MusicEvent _songB;


    private void Start()
    {
        _songA.Play(2.5f);
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