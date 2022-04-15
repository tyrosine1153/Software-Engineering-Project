using UnityEngine;

public class VideoManager : Singleton<VideoManager>
{
    public Resolution[] Resolutions;
    public int currentResolutionIndex;
    public bool currentFullscreen;
    protected override void Awake()
    {
        base.Awake();
        
        Resolutions = Screen.resolutions;
    }
    public void SetResolution(int index, bool fullscreen)
    {
        if (index < 0 || index >= Resolutions.Length) return;
        currentResolutionIndex = index;
        currentFullscreen = fullscreen;
        Screen.SetResolution(Resolutions[index].width, Resolutions[index].height, fullscreen);
    }
} 