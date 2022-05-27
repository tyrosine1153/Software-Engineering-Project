using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public static string DataPath;
    
    public Potion[] Potions;
    public StoryScenario[] StoryScenario;
    public Character[] Characters;
    public List<EndingPoint> EndingPoints;
    
    protected override void Awake()
    {
        base.Awake();

        DataPath = Path.Combine(Application.dataPath, "Resources/Data");
        
        Potions = LoadByJson<Potion>(DataPath, "Potions").ToArray();
        StoryScenario = LoadByJson<StoryScenario>(DataPath, "StoryScenario").ToArray();
        Characters = LoadByJson<Character>(DataPath, "Characters").ToArray();
        EndingPoints = LoadByJson<EndingPoint>(DataPath, "EndingPoints").ToList();

        StoryScenario = StoryScenario.OrderBy(s => s.ID).ToArray();
    }

    public static bool TryMakePotion(int[] materialCount, out Potion result)
    {
        result = new Potion();
        if (materialCount.Length != Enum.GetValues(typeof(Potion)).Length) return false;

        foreach (var potion in Instance.Potions)
        {
            if (potion.Material.Length != Enum.GetValues(typeof(Potion)).Length) return false;
            if (potion.Material.Where((t, i) => t != materialCount[i]).Any()) continue;

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
        EndingPoints.Add(endingPoint);
        SaveByJson(DataPath, "EndingPoints", EndingPoints);
        
        PlayerPrefs.SetInt("StoryPoint", endingPoint.NextScenarioID);
    }
    
    public int LoadGameStoryPoint()
    {
        return PlayerPrefs.GetInt("StoryPoint", 0);
    }
    
    public void ResetGameStoryPoint()
    {
        PlayerPrefs.SetInt("StoryPoint", 0);
        
        EndingPoints.Clear();
        SaveByJson(DataPath, "EndingPoints", EndingPoints);
    }
    #endregion
    
    #region File IO
    public static void SaveByJson<T>(string filePath, string fileName, T obj)
    {
        File.WriteAllText($"{filePath}/{fileName}.json", JsonConvert.SerializeObject(obj, Formatting.Indented));
    }
    
    public static IEnumerable<T> LoadByJson<T>(string filePath, string fileName)
    {
        return JsonConvert.DeserializeObject<IEnumerable<T>>(File.ReadAllText($"{filePath}/{fileName}.json"));
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
