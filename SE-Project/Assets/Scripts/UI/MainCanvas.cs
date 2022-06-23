using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] private Button newStartButton;
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject settingsPanel;

    [Header("Story")]
    [SerializeField] private Button storyButton;
    [SerializeField] private GameObject storyPanel;
    [SerializeField] private Text storyContentPanel;
    [SerializeField] private Image storyContentImage;
    [SerializeField] private Button storySkipButton;

    public void Start()
    {
        newStartButton.onClick.AddListener(() => { /* Todo */ GameManager.Instance.NewGameStart(); });
        startButton.onClick.AddListener(() => { /* Todo */ GameManager.Instance.GameStart();});
        settingsButton.onClick.AddListener(() => { settingsPanel.gameObject.SetActive(true);});
        quitButton.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif 
        });
        
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
