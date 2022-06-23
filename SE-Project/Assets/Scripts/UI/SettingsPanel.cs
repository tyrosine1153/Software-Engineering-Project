using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Text bgmHandleText;
    [SerializeField] private Toggle bgmMuteToggle;
    [Space]
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Text sfxHandleText;
    [SerializeField] private Toggle sfxMuteToggle;
    [Space]
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullScreenToggle;
    [SerializeField] private Button entryButton;
    [SerializeField] private Button cancelButton;
    [Space]
    [SerializeField] private Toggle effectToggle;
    
    private float bgmVolume;
    private float sfxVolume;
    
    private void Start()
    {
        bgmSlider.onValueChanged.AddListener(value =>
        {
            AudioManager.Instance.BGMVolume = value;
            bgmHandleText.text = value.ToString("P0");
        });
        bgmMuteToggle.onValueChanged.AddListener(value =>
        {
            if (value) bgmVolume = AudioManager.Instance.BGMVolume;
            
            AudioManager.Instance.BGMVolume = value ? 0 : bgmVolume;
            bgmSlider.interactable = !value;
        });
        sfxSlider.onValueChanged.AddListener(value =>
        {
            AudioManager.Instance.SFXVolume = value;
            sfxHandleText.text = value.ToString("P0");
        });
        sfxMuteToggle.onValueChanged.AddListener(value =>
        {
            if (value) sfxVolume = AudioManager.Instance.SFXVolume;
            
            AudioManager.Instance.SFXVolume = value ? 0 : sfxVolume;
            sfxSlider.interactable = !value;
        });

        resolutionDropdown.options.Clear();
        foreach (var resolution in VideoManager.Instance.Resolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData($"{resolution.width} x {resolution.height}"));
        }
        resolutionDropdown.RefreshShownValue();
        resolutionDropdown.value = VideoManager.Instance.currentResolutionIndex;
        fullScreenToggle.isOn = VideoManager.Instance.currentFullscreen;
        
        entryButton.onClick.AddListener(() =>
        {
            VideoManager.Instance.SetResolution(resolutionDropdown.value, fullScreenToggle.isOn);
        });
        cancelButton.onClick.AddListener(() =>
        {
            resolutionDropdown.value = VideoManager.Instance.currentResolutionIndex;
            fullScreenToggle.isOn = VideoManager.Instance.currentFullscreen;
        });
        
        effectToggle.onValueChanged.AddListener(_ => { /* Todo */ });
    }
}
