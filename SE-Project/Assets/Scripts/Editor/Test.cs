using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class Test
{
    public static readonly string DataPath = $"{Application.dataPath}/Resources/Data";
    public static readonly string SpritePath = $"{Application.dataPath}/Resources/Sprite";
    public const string SpritePathInResource = "Sprite";

    [MenuItem("Tools/Create Empty File")]
    public static void CreateEmptyFile()
    {
        
        var foods = new List<Food>();
        for (int i = 0; i < 3; i++)
        {
            var food = new Food
            {
                ID = i,
                Name = "name",
                Description = "Food to test",
                Image = $"{SpritePathInResource}/{i}",
                Material = 1001034,
            };
            foods.Add(food);
        }
        DataManager.SaveByJson(DataPath, "Foods", foods);
        DataManager.SaveByCsv(DataPath, "Foods", foods);
        
        var scenario = new List<StoryScenario>();
        for (int i = 0; i < 3; i++)
        {
            var storyScenario = new StoryScenario
            {
                ID = i,
                PrevId = i - 1,
                NextId = i + 1,
                Speaker = "Speaker",
                Content = "The Quick Brown Fox Jumps Over The Lazy Dog",
                Effects = new[] {0, -2, 2, -1},
                Characters = new[]
                {
                    new CharacterAct(0, CharacterEmotionType.Idle), 
                    new CharacterAct(1, CharacterEmotionType.Happy),
                    null,
                    new CharacterAct(3, CharacterEmotionType.Sad),
                },
            };
            scenario.Add(storyScenario);
        }
        DataManager.SaveByJson(DataPath, "StoryScenario", scenario);
        
        var characters = new List<Character>();
        for (int i = 0; i < 3; i++)
        {
            var character = new Character
            {
                ID = i,
                Name = "Jeongmin",
                Description = "The Greatest Programmer",
                Emotion = new [] {CharacterEmotionType.Idle, CharacterEmotionType.Happy, CharacterEmotionType.Sad},
            };
            characters.Add(character);
        }
        DataManager.SaveByJson(DataPath, "Characters", characters); 
    }
}
