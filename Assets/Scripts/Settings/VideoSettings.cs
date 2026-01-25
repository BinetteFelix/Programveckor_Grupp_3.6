using Unity.VisualScripting;
using UnityEngine;

public class VideoSettings : MonoBehaviour
{
    [SerializeField] private Vector2[] resolutions;
    [SerializeField] private FullScreenMode[] fullScreenModes;

    private FullScreenMode selectedFullscreenMode;
    private Vector2 selectedResolution;

    private void Awake()
    {
        selectedFullscreenMode = FullScreenMode.FullScreenWindow;
    }
    public void SetResolution()
    {
        Screen.SetResolution(Mathf.RoundToInt(selectedResolution.x), Mathf.RoundToInt(selectedResolution.y), selectedFullscreenMode);
    }
    public void SelectResolution(int resolutionIndex)
    {
        selectedResolution = resolutions[resolutionIndex];
    }
    public void SelectFullscreenMode(int fullscreenModeIndex)
    {
        selectedFullscreenMode = fullScreenModes[fullscreenModeIndex];
    }
}