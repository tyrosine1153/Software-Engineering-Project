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
        currentStoryScenario = DataManager.Instance.StoryScenario.FirstOrDefault(s => s.id == currentStoryId);

        if (currentStoryScenario.order != null)
        {
            currentStoryOptions = currentStoryScenario.order;
            openCraftPage = true;
        }
        else
        {
            var nextStory = DataManager.Instance.StoryScenario.FirstOrDefault(s => s.id == currentStoryScenario.nextId);
            if (nextStory.prevId == -1)
            {
                nextStory.prevId = currentStoryScenario.id;
            }
        }

        story.ShowStory(currentStoryScenario);
    }
    
    public void SetPrevStory()
    {
        if (currentStoryScenario.prevId == -1)
        {
            Debug.LogError("Not created button.");  // 생성되지 않은 버튼이 눌릴 경우
            return;
        }
        openCraftPage = false;

        SetStory(currentStoryScenario.prevId);
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
        
        if (currentStoryScenario.nextId == -1)
        {
            // Todo : Story End
            return;
        }
        SetStory(currentStoryScenario.nextId);
    }

    public void GetPotionStory(Potion potion)
    {
        OrderOption matchingOrder;
        try
        {
            // First는 맞는 값이 없으면 exception 발생
            matchingOrder = currentStoryOptions.First(o => o.potionId == potion.id);
        }
        catch (Exception)
        {
            // 근데 정해진 선택지 외의 값이 들어오면 그에 맞는 스크립트가 필요함
            matchingOrder = currentStoryOptions.FirstOrDefault(o => o.potionId == -1);
        }
        
        var endingPoint = new EndingPoint
        {
            id = currentStoryScenario.id,
            nextScenarioID = matchingOrder.nextScenarioID,
            result = matchingOrder.result
        };
        DataManager.Instance.SaveGameStoryPoint(endingPoint);

        craft.gameObject.SetActive(false);
        SetStory(matchingOrder.nextScenarioID);
    }
}