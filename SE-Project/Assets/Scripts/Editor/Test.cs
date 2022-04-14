using System.Collections.Generic;
using UnityEditor;

public static class Test
{
    [MenuItem("Tools/Create Empty File")]
    public static void CreateEmptyFile()
    {
        var foods = new List<Food>();
        for (int i = 0; i < 3; i++)
        {
            var food = new Food
            {
                ID = 123,
                Name = "Test",
                Description = "asdf",
                Image = "asdf",
                Material = 123
            };
            foods.Add(food);
        }
        
        DataManager.SaveByJson(DataManager.DataPath, "Foods", foods);
        DataManager.SaveByCsv(DataManager.DataPath, "Foods", foods);
    }
}
