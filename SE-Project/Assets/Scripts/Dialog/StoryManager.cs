using System;
using UnityEngine;
using UnityEngine.UI;

public class StoryManager : Singleton<StoryManager>
{
    private int _currentStoryId;
    private bool _openCraftPage;
    [SerializeField] private GameObject storyCanvas;
    [SerializeField] private GameObject craftCanvas;
    [SerializeField] private Text speakerText;
    [SerializeField] private Text contentText;
    [SerializeField] private Text nextScenarioButtonText;
    [SerializeField] private Button prevScenarioButton;
    [SerializeField] private Button nextScenarioButton;
    
    private void Start()
    {
        storyCanvas.SetActive(true);
        craftCanvas.SetActive(false);
        prevScenarioButton.onClick.AddListener(PreviousDialogButtonOnClick);
        nextScenarioButton.onClick.AddListener(NextDialogButtonOnClick);

        _currentStoryId = 0;
        ShowStory(_currentStoryId);
    }

    public void ShowStory(int id)
    {
        HideButtons(id);
        
        var story = DataManager.Instance.StoryScenario[id];
        var characters = story.Characters;
        var content = story.Content;
        var effects = story.Effects;
        var order = story.Order;
        var storySpeaker = story.Speaker;
        var storyId= story.ID;
        var nextStoryId= story.NextId;
        var prevStoryId= story.PrevId;

        // Add Effects
        
        speakerText.text = $"{prevStoryId} <- {storyId} -> {nextStoryId} : {storySpeaker}";
        contentText.text = content;
        
        print($"Content : {story.Content}");
    }

    private void HideButtons(int id)
    {
        var story = DataManager.Instance.StoryScenario[id];
        prevScenarioButton.gameObject.SetActive(story.PrevId != -1);
    }

    public void NextDialogButtonOnClick()
    {
        if (_openCraftPage)
        {
            // 조합 페이지로 넘어감
            OpenCraftPage();
            _openCraftPage = false;
            return;
        }
        
        var nextStoryId = DataManager.Instance.StoryScenario[_currentStoryId].NextId;
        print($"next : {nextStoryId}");
        
        if (nextStoryId == -1)
        {
            OpenCraftPage();
            return;
        }
        _currentStoryId = nextStoryId;
        ShowStory(nextStoryId);
    }
    
    public void PreviousDialogButtonOnClick()
    {
        var previousStoryId = DataManager.Instance.StoryScenario[_currentStoryId].PrevId;
        print($"prev : {previousStoryId}");
        if (previousStoryId == -1)
        {
            // 이전으로 돌아가기가 막힘
            return;
        }
        _currentStoryId = previousStoryId;
        ShowStory(_currentStoryId);
    }
    private void OpenCraftPage()
    {
        print("Show Craft Page");
        storyCanvas.SetActive(false);
        craftCanvas.SetActive(true);
    }
    private void CloseCraftPage()
    {
        print("Show Craft Page");
        storyCanvas.SetActive(true);
        craftCanvas.SetActive(false);
    }

    public void GetFoodStory(Potion food)
    {
        print(_currentStoryId);
        var story = DataManager.Instance.StoryScenario[_currentStoryId];
        var isNewFoodReceipt = false;
        var find = false;
        foreach (var order in story.Order)
        {
            
            if (order.PotionId != food.ID) continue;

            // Show Result
            // isNewFoodReceipt = true or false;
            _currentStoryId = order.NextScenarioID;
            find = true;
            break;
        }

        if (!find) return;

            // Find
        // If (isNewFoodReceipt) { Do something; }
        print(_currentStoryId); // 4
        
        // 스토리 4가 없어서 발생하는 IndexOutOfRangeException 우회
        _currentStoryId = 0;
        
        CloseCraftPage();
        ShowStory(_currentStoryId);
    }
    
    
}
