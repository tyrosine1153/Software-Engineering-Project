using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int currentDay;

    public void NewGameStart()
    {
        DataManager.Instance.ResetProgress();
        GameStart();
    }

    public void GameStart()
    {
        var (day, scenario) = DataManager.Instance.LoadProgress();
        Debug.Log($" day: {day} scenario: {scenario}");
        currentDay = day;
        
        SceneManagerEx.LoadScene(SceneType.Game);
        StartCoroutine(CoSetStory(scenario));
    }

    private IEnumerator CoSetStory(int scenarioId)
    {
        while (!GameCanvas.IsInitialized)
        {
            yield return null;
        }
        
        DayStart(scenarioId);
    }

    private void DayStart(int scenarioId = 0)
    {
        DataManager.Instance.SaveProgress(currentDay);
        DataManager.Instance.LoadScenario(currentDay);
        GameCanvas.Instance.SetStory(scenarioId);
        GameCanvas.Instance.ShowDayStart(currentDay);
        if (scenarioId == 0)
        {
            RecipeModel.Instance.UnlockWeeklyPotions(currentDay);
            GameCanvas.Instance.RecipeBookUpdate();
            RecipeModel.Instance.ResetDailyUnlockedRecipes();
        }
    }

    public void DayEnd()
    {
        currentDay++;
        DayStart();
    }
}
