using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using Newtonsoft.Json;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public static string DataPath;
    public Food[] Foods;
    public StoryScenario[] StoryScenario;
    public Character[] Characters;
    
    protected override void Awake()
    {
        base.Awake();

        DataPath = Path.Combine(Application.dataPath, "Resources/Data");
        Foods = LoadByCsv<Food>(DataPath, "Foods").ToArray();
        StoryScenario = LoadByJson<StoryScenario>(DataPath, "StoryScenario").ToArray();
        Characters = LoadByJson<Character>(DataPath, "Characters").ToArray();
    }
    
    public static bool TryMakeFood(int[] materialCount, ref Food result)
    {
        var material = 0;
        for (int i = 0; i < materialCount.Length; i++)
        {
            material += materialCount[i] * (10 ^ i);
        }

        foreach (var food in DataManager.Instance.Foods)
        {
            if (food.Material == material)
            {
                result = food;
                return true;
            }
        }
        return false;
    }
    
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
        using var writer = new StreamWriter($"{filePath}/{fileName}.csv");
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteHeader<T>();
        csv.NextRecord();
        foreach (var record in records)
        {
            csv.WriteRecord(record);
            csv.NextRecord();
        }
    }
    
    public static IEnumerable<T> LoadByCsv<T>(string filePath, string fileName)
    {
        var result = new List<T>();
        using var reader = new StreamReader($"{filePath}/{fileName}.csv");
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        csv.Read();
        csv.ReadHeader();
        while (csv.Read())
        {
            var record = csv.GetRecord<T>();
            result.Add(record);
        }

        return result;
    }
    #endregion
}
