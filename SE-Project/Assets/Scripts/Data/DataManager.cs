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
    
    protected override void Awake()
    {
        base.Awake();

        DataPath = Path.Combine(Application.dataPath, "Resources/Data");
        Foods = LoadByCsv<Food>(DataPath, "Foods").ToArray();
    }
    
    public static void SaveByJson<T>(string filePath, string fileName, T obj)
    {
        var fileStream = new FileStream($"{filePath}/{fileName}.json", FileMode.Create);
        var jsonData = JsonConvert.SerializeObject(obj);
        var data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);  // byte로 인코딩 한 것을 파일스트림에 작성
        fileStream.Close();
    }
    
    public static T LoadByJson<T>(string filePath, string fileName)
    {
        var fileStream = new FileStream($"{filePath}/{fileName}.json", FileMode.Open);
        var data = new byte[fileStream.Length];
        var jsonData = Encoding.UTF8.GetString(data);
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();

        return JsonConvert.DeserializeObject<T>(jsonData);
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
}
