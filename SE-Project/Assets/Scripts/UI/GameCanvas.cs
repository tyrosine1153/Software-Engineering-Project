using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : Singleton<GameCanvas>
{
    [Header("Day")]
    [SerializeField] private GameObject dayStart;
    [SerializeField] private Text dayStartDayText;
    [SerializeField] private GameObject dayEnd;
    [SerializeField] private Text dayEndDayText;
    [SerializeField] private Text dayEndUnlockedRecipeText;
    [SerializeField] private Button dayEndButton;
    
    [Header("HUD")]
    [SerializeField] private Text dayText;
    [SerializeField] private Button settingsButton;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Transform recipeBookViewContent;
    [SerializeField] private GameObject recipeBookPotionLine;
    [SerializeField] private Button recipeBookButton;
    [SerializeField] private Animator recipeBookAnimator;
    private bool isRecipeBookOpen;
    private static readonly int Show = Animator.StringToHash("Show");
    private static readonly int Hide = Animator.StringToHash("Hide");
    
    [Header("Story, Craft")]
    public SpriteRenderer[] actors;
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
        settingsButton.onClick.AddListener(() =>
        {
            settingsPanel.gameObject.SetActive(true);
        });
        recipeBookButton.onClick.AddListener(() =>
        {
            isRecipeBookOpen = !isRecipeBookOpen;
            recipeBookAnimator.SetTrigger(isRecipeBookOpen ? Show : Hide);
        });
    }

    #region Day

    // Day start 켜기, 일차 ui 설정
    public void ShowDayStart(int day)
    {
        dayText.text = day == 0 ? "튜토리얼" : $"{day}일 차";
        dayStartDayText.text = day == 0 ? "튜토리얼" : $"{day}일 차";
        dayStart.SetActive(true);
        isRecipeBookOpen = false;
        recipeBookAnimator.SetTrigger(isRecipeBookOpen ? Show : Hide);
        
        StartCoroutine(CoCloseDayStart());
    }
    
    private IEnumerator CoCloseDayStart()
    { 
        yield return new WaitForSeconds(3f);
        dayStart.SetActive(false);
    }

    // Day end 켜기, ui : 일차 해금된 레시피
    public void ShowDayEnd(int day)
    {
        dayEndDayText.text = day == 0 ? "튜토리얼" : $"{day}일 차";
        var str = "";
        foreach (var unlockedRecipe in RecipeModel.Instance.DailyUnlockedRecipes)
        {
            var potion = DataManager.Instance.potions[unlockedRecipe.PotionId];
            str += $"{potion.name}\n";
        }

        dayEndUnlockedRecipeText.text = str;
        // Todo : 오늘 공개된 레시피 출력
        dayEnd.SetActive(true);
        
        dayEndButton.onClick.AddListener(() =>
        {
            // Todo : 오늘 공개된 레시피 리스트 지우기
            GameManager.Instance.DayEnd();
            dayEnd.SetActive(false);
        });
    }

    #endregion

    public void RecipeBookUpdate()
    {
        var lines = recipeBookViewContent.GetComponentsInChildren<PotionLine>();
        foreach (var line in lines)
        {
            Destroy(line.gameObject);
        }

        foreach (var recipe in RecipeModel.Instance.Recipes)
        {
            if(!recipe.IsPotionUnlocked) continue;
            
            var line = Instantiate(recipeBookPotionLine, recipeBookViewContent);
            var potion = DataManager.Instance.potions[recipe.PotionId];
            line.GetComponent<PotionLine>().Set(SpriteUtil.LoadPotionSprite(potion.id), 
                potion.name, recipe.IsPotionUnlocked ? potion.material : null);
        }
        
    }
    
    #region Story/Craft

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
            var nextStory = DataManager.Instance.storyScenario.FirstOrDefault(s => s.id == currentStoryScenario.nextId);

            if (nextStory != null && nextStory.prevId == -1)
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
            ShowDayEnd(GameManager.Instance.currentDay);
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
        
        // 이미 만든 적이 있는가?
        // IsPotionUnlocked => Weekend Unlocked
        // IsRecipeUnlocked => Creat Unlocked
        // Todo : 포션 레시피 해금 여부 결정, 포션 레시피 힌트
        if (success && !RecipeModel.Instance.Recipes[potion.id].IsRecipeUnlock)
        {
            RecipeModel.Instance.Recipes[potion.id].IsRecipeUnlock = true;
            RecipeModel.Instance.AddDailyUnlockedRecipes(potion.id);
            RecipeBookUpdate();
        }
        craftResult.gameObject.SetActive(true);
        craftResult.ShowResult(success, potion);
    }
    
    public void GetPotionStory(Potion potion)
    {
        isRecipeBookOpen = false;
        recipeBookAnimator.SetTrigger(isRecipeBookOpen ? Show : Hide);
        craftResult.gameObject.SetActive(false);
        
        // First는 맞는 값이 없으면 exception 발생. 근데 정해진 선택지 외의 값이 들어오면 그에 맞는 스크립트가 필요함
        var matchingOrder = currentStoryScenario.order.FirstOrDefault(o => o.potionId == potion.id) ??
                            currentStoryScenario.order.First(o => o.potionId == -1);

        var endingPoint = new EndingPoint(
            GameManager.Instance.currentDay, currentStoryScenario.id,
            matchingOrder.nextScenarioID, matchingOrder.result);
        
        DataManager.Instance.SaveProgress(endingPoint);

        craft.gameObject.SetActive(false);
        SetStory(matchingOrder.nextScenarioID);
    }

    public static bool TryMakePotion(int[] materialCount, out Potion result)
    {
        result = new Potion();
        if (materialCount.Length != Enum.GetValues(typeof(Material)).Length) return false;

        // 해금된 포션 list
        var unlockedPotions = RecipeModel.Instance.GetUnlockedPotions();
        foreach (var potion in unlockedPotions)
        {
            if (potion.material.Length != Enum.GetValues(typeof(Material)).Length) return false;
            if (potion.material.Where((t, i) => t != materialCount[i]).Any()) continue;

            result = potion;
            return true;
        }

        return false;
    }
    
    #endregion
}
