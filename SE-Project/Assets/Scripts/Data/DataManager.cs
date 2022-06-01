using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public static string DataPath;
    
    public Potion[] potions;
    public StoryScenario[] storyScenario;
    public Character[] characters;
    public List<EndingPoint> endingPoints;
    
    protected override void Awake()
    {
        base.Awake();

        DataPath = Path.Combine(Application.dataPath, "Resources/Data");
        
        potions = LoadByJson<Potion>(DataPath, "Potions").ToArray();
        storyScenario = LoadByJson<StoryScenario>(DataPath, "StoryScenario").ToArray();
        characters = LoadByJson<Character>(DataPath, "Characters").ToArray();
        endingPoints = LoadByJson<EndingPoint>(DataPath, "EndingPoints").ToList();

        storyScenario = storyScenario.OrderBy(s => s.id).ToArray();
    }

    public static bool TryMakePotion(int[] materialCount, out Potion result)
    {
        result = new Potion();
        if (materialCount.Length != Enum.GetValues(typeof(Potion)).Length) return false;

        foreach (var potion in Instance.potions)
        {
            if (potion.material.Length != Enum.GetValues(typeof(Potion)).Length) return false;
            if (potion.material.Where((t, i) => t != materialCount[i]).Any()) continue;

            result = potion;
            return true;
        }

        return false;
    }

    #region Save/Load
    public void SaveGameStoryPoint(int scenarioId)
    {
        PlayerPrefs.SetInt("StoryPoint", scenarioId);
    }

    public void SaveGameStoryPoint(EndingPoint endingPoint)
    {
        endingPoints.Add(endingPoint);
        SaveByJson(DataPath, "EndingPoints", endingPoints);
        
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
        SaveByJson(DataPath, "EndingPoints", endingPoints);
    }
    #endregion
    
    #region File IO
    public static void SaveByJson<T>(string filePath, string fileName, T obj)
    {
        File.WriteAllText($"{filePath}/{fileName}.json", JsonUtility.ToJson(obj, true));
    }
    
    public static IEnumerable<T> LoadByJson<T>(string filePath, string fileName)
    {
        return JsonUtility.FromJson<IEnumerable<T>>(File.ReadAllText($"{filePath}/{fileName}.json"));
    }

    public static void SaveByCsv<T>(string filePath, string fileName, IEnumerable<T> records)
    {
        
    }
    
    public static IEnumerable<T> LoadByCsv<T>(string filePath, string fileName)
    {
        return CSVSerializer.Deserialize<T>(File.ReadAllText($"{filePath}/{fileName}.csv"));
    }
    #endregion
}
