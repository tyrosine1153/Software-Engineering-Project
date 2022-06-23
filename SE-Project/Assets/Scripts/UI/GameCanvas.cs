using System;
using System.Linq;
using UnityEngine;

public class GameCanvas : Singleton<GameCanvas>
{
    [SerializeField] public SpriteRenderer[] actors;
    [SerializeField] private Story story;
    [SerializeField] private Craft craft;
    [SerializeField] private CraftResult craftResult;
    private StoryScenario currentStoryScenario;
    private bool openCraftPage;

    private void Start()
    {
        story.gameObject.SetActive(true);
        craft.gameObject.SetActive(false);
        craftResult.gameObject.SetActive(false);
   
        SetStory(0);
    }

    public void SetStory(int id)
    {
        if(id == -1) Debug.LogError($"Story was not found. invalid id: {id}");

        currentStoryScenario = DataManager.Instance.storyScenario.First(s => s.id == id);

        if (currentStoryScenario.order.Length > 0)
        {
            openCraftPage = true;
        }
        else
        {
            var nextStory = DataManager.Instance.storyScenario.First(s => s.id == currentStoryScenario.nextId);
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

    public void RetryCraft()
    {
        craftResult.gameObject.SetActive(false);
        craft.gameObject.SetActive(true);
    }

    public void GetPotionResult(int[] materialCount)
    {
        var success = TryMakePotion(materialCount, out var potion);
        
        // Todo : 포션 레시피 해금 여부 결정, 포션 레시피 힌트
        craftResult.gameObject.SetActive(true);
        craftResult.ShowResult(success, potion);
    }

    public void GetPotionStory(Potion potion)
    {
        craftResult.gameObject.SetActive(false);
        
        // First는 맞는 값이 없으면 exception 발생. 근데 정해진 선택지 외의 값이 들어오면 그에 맞는 스크립트가 필요함
        var matchingOrder = currentStoryScenario.order.FirstOrDefault(o => o.potionId == potion.id) ??
                            currentStoryScenario.order.First(o => o.potionId == -1);

        var endingPoint = new EndingPoint(
            GameManager.Instance.Day, currentStoryScenario.id,
            matchingOrder.nextScenarioID, matchingOrder.result);
        
        DataManager.Instance.SaveProgress(endingPoint);

        craft.gameObject.SetActive(false);
        SetStory(matchingOrder.nextScenarioID);
    }

    public static bool TryMakePotion(int[] materialCount, out Potion result)
    {
        result = new Potion();
        if (materialCount.Length != Enum.GetValues(typeof(Material)).Length) return false;

        foreach (var potion in DataManager.Instance.potions)
        {
            if (potion.material.Length != Enum.GetValues(typeof(Material)).Length) return false;
            if (potion.material.Where((t, i) => t != materialCount[i]).Any()) continue;

            result = potion;
            return true;
        }

        return false;
    }
}