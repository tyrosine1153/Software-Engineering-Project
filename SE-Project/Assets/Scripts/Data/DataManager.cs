using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public Potion[] potions;
    public StoryScenario[] storyScenario;
    public Character[] characters;
    public List<EndingPoint> endingPoints;
    
    protected override void Awake()
    {
        base.Awake();

        var gameData = Resources.Load<GameDataScriptableObject>("Data/GameData");
        potions = gameData.potions;
        characters = gameData.characters;
        endingPoints = LoadByJson<List<EndingPoint>>("EndingPoints");
    }

    #region Save/Load

    public void LoadScenario(int day)
    {
        storyScenario = LoadByJson<FuckingStoryScenarioArray>(day.ToString(), "StoryScenario").scenarios;
        storyScenario = storyScenario.OrderBy(s => s.id).ToArray();
    }
    
    public void SaveScenario()
    {
        SaveByJson(storyScenario, "StoryScenario");
    }
    
    public void ResetProgress()
    {
        SaveProgress();
        
        endingPoints.Clear();
        SaveByJson(endingPoints, "EndingPoints");
    }
    
    public (int day, int scenario) LoadProgress()
    {
        endingPoints = LoadByJson<List<EndingPoint>>("EndingPoints");

        return (PlayerPrefs.GetInt("DayPoint", 0), PlayerPrefs.GetInt("StoryPoint", 0));
    }
    
    public void SaveProgress(int day = 1, int scenarioId = 0)
    {
        PlayerPrefs.SetInt("DayPoint", day);
        PlayerPrefs.SetInt("StoryPoint", scenarioId);
    }

    public void SaveProgress(EndingPoint endingPoint)
    {
        endingPoints.Add(endingPoint);
        SaveByJson(endingPoints, "EndingPoints");
        
        SaveProgress(endingPoint.day, endingPoint.nextScenarioID);
    }
    
    // Day, Scenario id, ending points
    // 새로 하기
    // 이어하기
    // 게임 저장
    // V 음료 제공 : day, scenario id, ending point 
    // - 일과 시작 : day, scenario id = 0, ending point = null
    // - 새로 하기 : day = 0, scenario id = 0, ending point = null
    #endregion
    
    #region File IO
    public static void SaveByJson<T>(T obj, string fileName, string filePath = null)
    {
        filePath = filePath == null
            ? Path.Combine(Application.dataPath, "Resources/Data")
            : Path.Combine(Application.dataPath, "Resources/Data", filePath);
        File.WriteAllText($"{filePath}/{fileName}.json", JsonUtility.ToJson(obj, true));
    }

    public static T LoadByJson<T>(string fileName, string filePath = null)
    {
        var path = filePath == null ? $"Data/{fileName}" : $"Data/{filePath}/{fileName}";
        var textAsset = Resources.Load<TextAsset>(path);

        return JsonUtility.FromJson<T>(textAsset.text);
    }

    public static void SaveByCsv<T>(string filePath, string fileName, IEnumerable<T> records)
    {
        
    }
    
    public static IEnumerable<T> LoadByCsv<T>(string filePath, string fileName)
    {
        var textAsset = Resources.Load<TextAsset>($"{filePath}/{fileName}");
        return CSVSerializer.Deserialize<T>(textAsset.text);
    }
    #endregion
}
