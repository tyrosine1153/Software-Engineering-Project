using System;
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

        const int currentDay = 3;
        var gameData = Resources.Load<GameDataScriptableObject>("Data/GameData");
        potions = gameData.potions;
        characters = gameData.characters;
        endingPoints = LoadByJson<List<EndingPoint>>("EndingPoints");
        storyScenario = LoadByJson<FuckingStoryScenarioArray>(currentDay.ToString(), "StoryScenario").scenarios;

        storyScenario = storyScenario.OrderBy(s => s.id).ToArray();
    }

    public static bool TryMakePotion(int[] materialCount, out Potion result)
    {
        result = new Potion();
        if (materialCount.Length != Enum.GetValues(typeof(Material)).Length) return false;

        foreach (var potion in Instance.potions)
        {
            if (potion.material.Length != Enum.GetValues(typeof(Material)).Length) return false;
            if (potion.material.Where((t, i) => t != materialCount[i]).Any()) continue;

            result = potion;
            return true;
        }

        return false;
    }

    #region Save/Load

    public void SaveScenario()
    {
        SaveByJson(storyScenario, "StoryScenario");
    }
    
    public void SaveGameStoryPoint(int scenarioId)
    {
        PlayerPrefs.SetInt("StoryPoint", scenarioId);
    }

    public void SaveGameStoryPoint(EndingPoint endingPoint)
    {
        endingPoints.Add(endingPoint);
        SaveByJson(endingPoints, "EndingPoints");
        
        PlayerPrefs.SetInt("StoryPoint", endingPoint.nextScenarioID);
    }
    
    public int LoadGameStoryPoint()
    {
        return PlayerPrefs.GetInt("StoryPoint", 0);
    }
    
    public void ResetGameStoryPoint()
    {
        PlayerPrefs.SetInt("StoryPoint", 0);
        
        endingPoints.Clear();
        SaveByJson(endingPoints, "EndingPoints");
    }
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
        Debug.Log(textAsset.text);

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
