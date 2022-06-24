using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Craft : MonoBehaviour
{
    [SerializeField] private Image potionImage;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button submitButton;
    [SerializeField] private Button[] materialButtons;
    [SerializeField] private Text[] materialCountTexts;

    [SerializeField] private GameObject tutorialPage;
    
    [SerializeField] private Text tutorialSpeakerText;
    [SerializeField] private Text tutorialStoryText;
    [SerializeField] private Button nextTutorialStoryButton;
    [SerializeField] private Button prevTutorialStoryButton;

    private const int TotalLimit = 10000; // 최대 재료 추가 횟수
    private const int MaterialLimit = 5; // 재료당 최대 추가 횟수
    private const float ColorRatio = 1f / MaterialLimit; // 횟수당 더해지는 색상 수치

    private int totalCount; // 현재 재료 추가 횟수
    private readonly int[] counts = new int[Enum.GetValues(typeof(Material)).Length]; // 재료당 현재 추가 횟수

    private void Start()
    {
        ResetPotionMaterial();
        resetButton.onClick.AddListener(ResetPotionMaterial);
        submitButton.onClick.AddListener(MakePotion);

        nextTutorialStoryButton.onClick.AddListener(TutorialNextButton);
        prevTutorialStoryButton.onClick.AddListener(TutorialPrevButton);
        
        var materialCount = Enum.GetValues(typeof(Material)).Length;
        for (var i = 0; i < materialButtons.Length; i++)
        {
            
            // 0 => 0 : R
            // 1 => 1 : M
            // 2 => 2 : B
            
            // 3 => 0 : C
            // 4 => 1 : G
            // 5 => 2 : Y
            
            var arg = (Material)(i % materialCount);
            var plus = i % 2 == 0;
            materialButtons[i].onClick.AddListener(() => AddPotionMaterial(arg, plus));
        }
    }

    private int _currentId = -1;
    
    private void OnEnable()
    {
        tutorialPage.SetActive(GameManager.Instance.currentDay == 0);
        _currentId = 6;
        var story = DataManager.Instance.storyScenario.FirstOrDefault(scenario => scenario.id == _currentId);

        MoveCheck(story);
    }

    public void TutorialNextButton()
    {
        var beforeStory = DataManager.Instance.storyScenario.FirstOrDefault(scenario => scenario.id == _currentId);
        _currentId = beforeStory.nextId;
        var story = DataManager.Instance.storyScenario.FirstOrDefault(scenario => scenario.id == _currentId);
        _currentId = story.id;
        
        MoveCheck(story);
    }
    
    public void TutorialPrevButton()
    {
        var beforeStory = DataManager.Instance.storyScenario.FirstOrDefault(scenario => scenario.id == _currentId);
        _currentId = beforeStory.prevId;
        var story = DataManager.Instance.storyScenario.FirstOrDefault(scenario => scenario.id == _currentId);
        _currentId = story.id;
        
        MoveCheck(story);
    }

    private void MoveCheck(StoryScenario story)
    {
        if (story == null)
        {
            print("Story is null");
            return;
        }
        
        print($"{_currentId} : {story.prevId} <- {story.id} -> {story.nextId}");
        
        tutorialSpeakerText.text = story.speaker;
        tutorialStoryText.text = story.content;
        
        nextTutorialStoryButton.gameObject.SetActive(story.nextId != -1);
        prevTutorialStoryButton.gameObject.SetActive(story.prevId != -1);
    }
    
    // 조합 데이터 초기화
    private void ResetPotionMaterial()
    {
        totalCount = 0;
        for (var i = 0; i < counts.Length; i++)
        {
            counts[i] = 0;
        }

        ShowPotionColor();
    }

    // 조합
    private void AddPotionMaterial(Material potionMaterial, bool plus)
    {
        if (TotalLimit <= totalCount) return; // 전체 조합 물약 개수 제한

        var index = (int)potionMaterial;
        var result = counts[index] + (plus ? 1 : -1);
        if (0 > result || result > MaterialLimit) return; // 물약 종류별 제한

        totalCount++;
        if (plus)
            counts[index]++;
        else
            counts[index]--;

        ShowPotionColor();
    }

    // 조합 결과 표시
    private void ShowPotionColor()
    {
        var r = counts[(int)Material.R];
        var g = counts[(int)Material.G];
        var b = counts[(int)Material.B];

        var color = new Color(
            Math.Abs(r) * ColorRatio,
            Math.Abs(g) * ColorRatio,
            Math.Abs(b) * ColorRatio
        );
        Debug.Log($"{r} {g} {b} {color}");
        potionImage.color = color;
        materialCountTexts[0].text = $"{r}";
        materialCountTexts[1].text = $"{g}";
        materialCountTexts[2].text = $"{b}";
        
        // rgb값이 음수일 경우 각 이펙트 출력
        ShowPotionEffect(r < 0, b < 0, g < 0);
    }

    // 재료 별 이펙트 출력
    private void ShowPotionEffect(bool redEffect, bool greenEffect, bool blueEffect)
    {
        // Todo : 색깔별 이펙트 표시
    }

    // 조합 결과 결정
    private void MakePotion()
    {
        var materialCount = new[] { counts[(int)Material.R], counts[(int)Material.G], counts[(int)Material.B] };

        ResetPotionMaterial();
        
        GameCanvas.Instance.GetPotionResult(materialCount);
    }
}
