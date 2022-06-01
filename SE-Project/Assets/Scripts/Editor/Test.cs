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
                id = i,
                name = "name",
                description = "Potions to test",
                material = new[] {i - 1, i, i + 1},
            };
            potions.Add(food);
        }

        DataManager.SaveByJson(DataPath, "Potions", potions);

        var scenario = new List<StoryScenario>();
        for (i = 0; i < 3; i++)
        {
            var storyScenario = new StoryScenario
            {
                id = i,
                prevId = i - 1,
                nextId = i + 1,
                speaker = "Speaker",
                content = "The Quick Brown Fox Jumps Over The Lazy Dog",
                effects = new[] {0, -2, 2, -1},
                characters = new[]
                {
                    new CharacterAct(0, CharacterEmotionType.Idle),
                    new CharacterAct(1, CharacterEmotionType.Happy),
                    null,
                    new CharacterAct(2, CharacterEmotionType.Sad),
                },
                order = null
            };
            scenario.Add(storyScenario);
        }

        var storyScenario_ = new StoryScenario
        {
            id = i,
            prevId = i - 1,
            nextId = i + 1,
            speaker = "Speaker",
            content = "The Quick Brown Fox Jumps Over The Lazy Dog",
            effects = new[] {0, -2, 2, -1},
            characters = new[]
            {
                new CharacterAct(0, CharacterEmotionType.Idle),
                new CharacterAct(1, CharacterEmotionType.Happy),
                null,
                new CharacterAct(2, CharacterEmotionType.Sad),
            },
            order = new[]
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
                id = i,
                name = "Jeongmin",
                description = "The Greatest Programmer",
                emotion = new[] {CharacterEmotionType.Idle, CharacterEmotionType.Happy, CharacterEmotionType.Sad},
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

        var currentStory = DataManager.Instance.StoryScenario.FirstOrDefault(s => s.id == id); // 임시 값 (필드)

        log.AppendLine($"{currentStory.speaker} : {currentStory.content}");

        log.AppendLine("[Effect]");
        if (currentStory.effects[(int) EffectType.None] != 1)
        {
            if (currentStory.effects[(int) EffectType.Dark] != -1)
            {
                // currentStory.Effects[(int) EffectType.Dark]
                // -3 : All, -2 : Others, -1 : None, 0~n : Position
                log.AppendLine($"Dark : {currentStory.effects[(int) EffectType.Dark]}");
            }

            if (currentStory.effects[(int) EffectType.Zoom] != -1)
            {
                // currentStory.Effects[(int) EffectType.Zoom]
                // -1 : None, 0~n : Position
                log.AppendLine($"Zoom : {currentStory.effects[(int) EffectType.Zoom]}");
            }

            if (currentStory.effects[(int) EffectType.Illustration] != -1)
            {
                // currentStory.Effects[(int) EffectType.Illustration]
                // -1 : None, 0~n : illustration id
                log.AppendLine($"Illustration : {currentStory.effects[(int) EffectType.Illustration]}");
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
                DataManager.Instance.Characters.FirstOrDefault(c => c.id == currentStory.characters[0].characterId);
            var emotion = character.emotion.FirstOrDefault(emotion => emotion == currentStory.characters[0].emotion);
            log.AppendLine($"{i} : {character.name}_{emotion.ToString()}");
        }

        if (currentStory.order != null)
        {
            // 대충 order 값 변수에 저장, UI에 표시, 요리 시작
        }
        else if (DataManager.Instance.StoryScenario[currentStory.nextId].prevId == -1)
        {
            DataManager.Instance.StoryScenario[currentStory.nextId].prevId = currentStory.id;
        }
    }

    // 스토리 분기 로직
    public static void StoryServeTest(Potion potion)
    {
        var currentStory = DataManager.Instance.StoryScenario[3]; // 임시 값 (필드)
        var order = currentStory.order; // 임시 order 값(필드) : read에서 저장했다는 전재

        var log = new StringBuilder();
        log.AppendLine($"Potion id : {potion.id}");
        
        OrderOption matchingOrder;
        try
        {
            matchingOrder = order.First(o => o.potionId == potion.id);
        }
        catch (Exception)
        {
            matchingOrder = order.FirstOrDefault(o => o.potionId == -1);
        }
        log.AppendLine($"Serve Result : {matchingOrder.potionId} {matchingOrder.nextScenarioID} {matchingOrder.result}");

        var endingPoint = new EndingPoint
        {
            id = currentStory.id,
            nextScenarioID = matchingOrder.nextScenarioID,
            result = matchingOrder.result
        };

        DataManager.Instance.SaveGameStoryPoint(endingPoint);
        log.AppendLine($"Save EndingPoint : {endingPoint.id} {endingPoint.nextScenarioID} {endingPoint.result}");
    }

    [MenuItem("Tools/Save Game Story Point")]
    public static void StoryServeTest()
    {
        var potion = new Potion
        {
            id = 9
        };
        StoryServeTest(potion);
    }
}
