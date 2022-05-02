using System;
using UnityEngine;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    private int _currentStoryId;
    
    [SerializeField] private Text speakerText;
    [SerializeField] private Text contentText;
    [SerializeField] private Button prevScenarioButton;
    [SerializeField] private Button nextScenarioButton;

    private void Start()
    {
        prevScenarioButton.onClick.AddListener(PreviousDialogButtonOnClick);
        nextScenarioButton.onClick.AddListener(NextDialogButtonOnClick);
        
    }

    public void ShowStory(int id)
    {
        var story = DataManager.Instance.StoryScenario[id];
        var characters = story.Characters;
        var content = story.Content;
        var effects = story.Effects;
        var order = story.Order;
        var storySpeaker = story.Speaker;
        var storyId= story.ID;
        var nextStoryId= story.NextId;
        var prevStoryId= story.PrevId;

        print($"Content : {story.Content}");    
    }

    public static int GetNextStoryId(int id) => DataManager.Instance.StoryScenario[id].NextId;
    
    public static int GetPreviousStoryId(int id) => DataManager.Instance.StoryScenario[id].PrevId;
    
    
    public void NextDialogButtonOnClick()
    {
        var nextStoryId = GetNextStoryId(_currentStoryId);
        if (nextStoryId == -1)
        {
            // 조합법을 통해 next id가 정해져야 함
            return;
        }
        _currentStoryId = nextStoryId;
        ShowStory(_currentStoryId);
    }
    
    public void PreviousDialogButtonOnClick()
    {
        var previousStoryId = GetPreviousStoryId(_currentStoryId);

        if (previousStoryId == -1)
        {
            
            // 이전으로 돌아가기가 막힘
            return;
        }
        _currentStoryId = previousStoryId;
        ShowStory(_currentStoryId);
    }
}
