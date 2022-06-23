using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] private Button newStartButton;
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    [Header("Settings")]
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
    
    [Header("Story")]
    [SerializeField] private Button storyButton;
    [SerializeField] private GameObject storyPanel;
    [SerializeField] private Text storyContentPanel;
    [SerializeField] private Image storyContentImage;
    [SerializeField] private Button storySkipButton;
    
    private float bgmVolume;
    private float sfxVolume;

    public void Start()
    {
        newStartButton.onClick.AddListener(() => { /* Todo */ });
        startButton.onClick.AddListener(() => { /* Todo */ });
        settingsButton.onClick.AddListener(() => { });
        quitButton.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif 
        });
        
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
        
        // 세이브 데이터를 가져와 스토리 버튼 활성화 여부를 결정한다. 게임을 처음 켜는 경우는 스토리 버튼을 활성화하지 않는다.
        storyButton.onClick.AddListener(() =>
        {
            storyPanel.SetActive(true);
        });
        storySkipButton.onClick.AddListener(() =>
        {
            storyPanel.SetActive(false);
        });
    }
}
