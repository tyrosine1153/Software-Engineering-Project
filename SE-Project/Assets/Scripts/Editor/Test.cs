using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        int i;

        var potions = new List<Potion>();
        for (i = 0; i < 3; i++)
        {
            var food = new Potion
            {
                ID = i,
                Name = "name",
                Description = "Potions to test",
                Material = new[] {i - 1, i, i + 1},
            };
            potions.Add(food);
        }

        DataManager.SaveByJson(DataPath, "Potions", potions);

        var scenario = new List<StoryScenario>();
        for (i = 0; i < 3; i++)
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
                Order = null
            };
            scenario.Add(storyScenario);
        }

        var storyScenario_ = new StoryScenario
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
            Order = new[]
            {
                new OrderOption(0, 4, 0),
                new OrderOption(1, 5, 1),
                new OrderOption(2, 6, 2),
                new OrderOption(-1, 7, 3),
            }
        };
        scenario.Add(storyScenario_);
        DataManager.SaveByJson(DataPath, "StoryScenario", scenario);

        var characters = new List<Character>();
        for (i = 0; i < 3; i++)
        {
            var character = new Character
            {
                ID = i,
                Name = "Jeongmin",
                Description = "The Greatest Programmer",
                Emotion = new[] {CharacterEmotionType.Idle, CharacterEmotionType.Happy, CharacterEmotionType.Sad},
            };
            characters.Add(character);
        }

        DataManager.SaveByJson(DataPath, "Characters", characters);

        var endingPoints = new List<EndingPoint>();
        DataManager.SaveByJson(DataPath, "EndingPoints", endingPoints);
    }

    // 스토리 읽기 로직
    public static void StoryReadTest(int id)
    {
        var log = new StringBuilder();
        log.AppendLine("Story Read Test\n");

        var currentStory = DataManager.Instance.StoryScenario.FirstOrDefault(s => s.ID == id); // 임시 값 (필드)

        log.AppendLine($"{currentStory.Speaker} : {currentStory.Content}");

        log.AppendLine("[Effect]");
        if (currentStory.Effects[(int) EffectType.None] != 1)
        {
            if (currentStory.Effects[(int) EffectType.Dark] != -1)
            {
                // currentStory.Effects[(int) EffectType.Dark]
                // -3 : All, -2 : Others, -1 : None, 0~n : Position
                log.AppendLine($"Dark : {currentStory.Effects[(int) EffectType.Dark]}");
            }

            if (currentStory.Effects[(int) EffectType.Zoom] != -1)
            {
                // currentStory.Effects[(int) EffectType.Zoom]
                // -1 : None, 0~n : Position
                log.AppendLine($"Zoom : {currentStory.Effects[(int) EffectType.Zoom]}");
            }

            if (currentStory.Effects[(int) EffectType.Illustration] != -1)
            {
                // currentStory.Effects[(int) EffectType.Illustration]
                // -1 : None, 0~n : illustration id
                log.AppendLine($"Illustration : {currentStory.Effects[(int) EffectType.Illustration]}");
            }
        }
        else
        {
            log.AppendLine("None");
        }

        log.AppendLine("[Character]");
        for (int i = 0; i < 4; i++)
        {
            var character =
                DataManager.Instance.Characters.FirstOrDefault(c => c.ID == currentStory.Characters[0].CharacterId);
            var emotion = character.Emotion.FirstOrDefault(emotion => emotion == currentStory.Characters[0].Emotion);
            log.AppendLine($"{i} : {character.Name}_{emotion.ToString()}");
        }

        if (currentStory.Order != null)
        {
            // 대충 order 값 변수에 저장, UI에 표시, 요리 시작
        }
        else if (DataManager.Instance.StoryScenario[currentStory.NextId].PrevId == -1)
        {
            DataManager.Instance.StoryScenario[currentStory.NextId].PrevId = currentStory.ID;
        }
    }

    // 스토리 분기 로직
    public static void StoryServeTest(Potion potion)
    {
        var currentStory = DataManager.Instance.StoryScenario[3]; // 임시 값 (필드)
        var order = currentStory.Order; // 임시 order 값(필드) : read에서 저장했다는 전재

        var log = new StringBuilder();
        log.AppendLine($"Potion id : {potion.ID}");
        
        OrderOption matchingOrder;
        try
        {
            matchingOrder = order.First(o => o.PotionId == potion.ID);
        }
        catch (Exception e)
        {
            matchingOrder = order.FirstOrDefault(o => o.PotionId == -1);
        }
        log.AppendLine($"Serve Result : {matchingOrder.PotionId} {matchingOrder.NextScenarioID} {matchingOrder.Result}");

        var endingPoint = new EndingPoint
        {
            ID = currentStory.ID,
            NextScenarioID = matchingOrder.NextScenarioID,
            Result = matchingOrder.Result
        };

        DataManager.Instance.SaveGameStoryPoint(endingPoint);
        log.AppendLine($"Save EndingPoint : {endingPoint.ID} {endingPoint.NextScenarioID} {endingPoint.Result}");
    }

    [MenuItem("Tools/Save Game Story Point")]
    public static void StoryServeTest()
    {
        var potion = new Potion
        {
            ID = 9
        };
        StoryServeTest(potion);
    }
}
