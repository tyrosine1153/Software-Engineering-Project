using UnityEngine;

public class VideoManager : Singleton<VideoManager>
{
    public Resolution[] Resolutions;
    public int currentResolutionIndex;
    public bool currentFullscreen;
    
    private const string ResolutionIndexKey = "RESOLUTION_INDEX";
    private const string FullscreenKey = "FULLSCREEN";
    
    protected override void Awake()
    {
        base.Awake();
        
        Resolutions = Screen.resolutions;
        currentResolutionIndex = PlayerPrefs.GetInt(ResolutionIndexKey, currentResolutionIndex);
        currentFullscreen = PlayerPrefs.GetInt(FullscreenKey, currentFullscreen ? 1 : 0) == 1;
    }
    public void SetResolution(int index, bool fullscreen)
    {
        if (index < 0 || index >= Resolutions.Length) return;
        currentResolutionIndex = index;
        currentFullscreen = fullscreen;
        PlayerPrefs.SetInt(ResolutionIndexKey, currentResolutionIndex);
        PlayerPrefs.SetInt(FullscreenKey, currentFullscreen ? 1 : 0);
        Screen.SetResolution(Resolutions[index].width, Resolutions[index].height, fullscreen);
    }
} 