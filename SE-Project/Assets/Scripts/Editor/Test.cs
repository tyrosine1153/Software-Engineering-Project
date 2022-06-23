using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class Test
{
    private const string AssetDataPath = "Assets/Resources/Data";

    [MenuItem("Tools/Create Empty File")]
    public static void CreateEmptyFile()
    {
        var scenario = new List<StoryScenario>();
        int i;
        StoryScenario storyScenario;
        for (i = 0; i < 3; i++)
        {
            storyScenario = new StoryScenario
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

        storyScenario = new StoryScenario
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
        scenario.Add(storyScenario);

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
        
        var characters = new List<Character>();
        for (i = 0; i < 3; i++)
        {
            var character = new Character
            {
                id = i,
                name = "Jeongmin",
                description = "The Worst Programmer",
                emotion = new[] {CharacterEmotionType.Idle, CharacterEmotionType.Happy, CharacterEmotionType.Sad},
            };
            characters.Add(character);
        }

        var endingPoints = new List<EndingPoint>();

        const int dayToTest = 5;
        AssetDatabase.CreateFolder(AssetDataPath, "StoryScenario");
        for (int j = 1; j <= dayToTest; j++)
        {
            var storyScenarioScriptableObject = ScriptableObject.CreateInstance<StoryScenarioScriptableObject>();
            storyScenarioScriptableObject.scenarios = scenario.ToArray();
            AssetDatabase.CreateAsset(storyScenarioScriptableObject, $"{AssetDataPath}/StoryScenario/{j}.asset");
        }

        var gameDataScriptableObject = ScriptableObject.CreateInstance<GameDataScriptableObject>();
        gameDataScriptableObject.potions = potions.ToArray();
        gameDataScriptableObject.characters = characters.ToArray();
        AssetDatabase.CreateAsset(gameDataScriptableObject, $"{AssetDataPath}/GameData.asset");

        AssetDatabase.Refresh();
        
        DataManager.SaveByJson(endingPoints, "endingPoints");
    }

    // 스토리 읽기 로직
    public static void StoryReadTest(int id)
    {
        var log = new StringBuilder();
        log.AppendLine("Story Read Test\n");

        var currentStory = DataManager.Instance.storyScenario.FirstOrDefault(s => s.id == id); // 임시 값 (필드)

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
                DataManager.Instance.characters.FirstOrDefault(c => c.id == currentStory.characters[0].characterId);
            var emotion = character.emotion.FirstOrDefault(emotion => emotion == currentStory.characters[0].emotion);
            log.AppendLine($"{i} : {character.name}_{emotion.ToString()}");
        }

        if (currentStory.order != null)
        {
            // 대충 order 값 변수에 저장, UI에 표시, 요리 시작
        }
        else if (DataManager.Instance.storyScenario[currentStory.nextId].prevId == -1)
        {
            DataManager.Instance.storyScenario[currentStory.nextId].prevId = currentStory.id;
        }
    }

    // 스토리 분기 로직
    public static void StoryServeTest(Potion potion)
    {
        var currentStory = DataManager.Instance.storyScenario[3]; // 임시 값 (필드)
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
        (
            1,
            currentStory.id,
            matchingOrder.nextScenarioID,
            matchingOrder.result
        );

        DataManager.Instance.SaveProgress(endingPoint);
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

    [MenuItem("Tools/Convert Story Scenario Asset To Json")]
    public static void ConvertStoryScenarioAssetToJson()
    {
        var storyScenarioArray =  Resources.LoadAll<StoryScenarioScriptableObject>("Data/StoryScenario");

        foreach (var storyScenario in storyScenarioArray)
        {
            DataManager.SaveByJson(new FuckingStoryScenarioArray(storyScenario.scenarios), $"{storyScenario.name}", "StoryScenario");
        }
    }
}
