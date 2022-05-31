using System;
using System.Linq;
using UnityEngine;

public class GameTest : Singleton<GameTest>
{
    [SerializeField] private Story story;
    [SerializeField] private Craft craft;
    
    private int currentStoryId;
    private StoryScenario currentStoryScenario;
    private OrderOption[] currentStoryOptions;
    private bool openCraftPage;

    private void Start()
    {
        story.gameObject.SetActive(true);
        craft.gameObject.SetActive(false);
        
        SetStory(0);
    }

    public void SetStory(int id)
    {
        if(id == -1) Debug.LogError($"Story was not found. invalid id: {id}");

        currentStoryId = id;
        currentStoryScenario = DataManager.Instance.StoryScenario.FirstOrDefault(s => s.ID == currentStoryId);

        if (currentStoryScenario.Order != null)
        {
            currentStoryOptions = currentStoryScenario.Order;
            openCraftPage = true;
        }
        else
        {
            var nextStory = DataManager.Instance.StoryScenario.FirstOrDefault(s => s.ID == currentStoryScenario.NextId);
            if (nextStory.PrevId == -1)
            {
                nextStory.PrevId = currentStoryScenario.ID;
            }
        }

        story.ShowStory(currentStoryScenario);
    }
    
    public void SetPrevStory()
    {
        if (currentStoryScenario.PrevId == -1)
        {
            Debug.LogError("Not created button.");  // 생성되지 않은 버튼이 눌릴 경우
            return;
        }
        openCraftPage = false;

        SetStory(currentStoryScenario.PrevId);
    }
    
    public void SetNextStory()
    {
        // 조합 페이지로 넘어감
        if (openCraftPage)
        {
            craft.gameObject.SetActive(true);
            openCraftPage = false;
            return;
        }
        
        if (currentStoryScenario.NextId == -1)
        {
            // Todo : Story End
            return;
        }
        SetStory(currentStoryScenario.NextId);
    }

    public void GetPotionStory(Potion potion)
    {
        OrderOption matchingOrder;
        try
        {
            // First는 맞는 값이 없으면 exception 발생
            matchingOrder = currentStoryOptions.First(o => o.PotionId == potion.ID);
        }
        catch (Exception)
        {
            // 근데 정해진 선택지 외의 값이 들어오면 그에 맞는 스크립트가 필요함
            matchingOrder = currentStoryOptions.FirstOrDefault(o => o.PotionId == -1);
        }
        
        var endingPoint = new EndingPoint
        {
            ID = currentStoryScenario.ID,
            NextScenarioID = matchingOrder.NextScenarioID,
            Result = matchingOrder.Result
        };
        DataManager.Instance.SaveGameStoryPoint(endingPoint);

        craft.gameObject.SetActive(false);
        SetStory(matchingOrder.NextScenarioID);
    }
}