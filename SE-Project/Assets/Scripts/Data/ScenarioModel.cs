using System;

[Serializable]
public class StoryScenario
{
    public int id;
    public int prevId;
    public int nextId;

    public int[] effects;  // 일러스트는 나중에 이미지 경로로 대체할지 고민중
    public string speaker;
    public string content;
    
    public CharacterAct[] characters;
    public OrderOption[] order;
}

// Index of Effects array in StoryScenario
public enum EffectType
{
    None, // if it is 1, it means no effect
    Dark, // -3 : All, -2 : Others, -1 : None, 0~n : Position
    Zoom, // -1 : None, 0~n : Position
    Illustration, // -1 : None, 0~n : illustration id
}

public enum CharacterEmotionType
{
    Idle,
    Happy,
    Sad,
}

[Serializable]
public class CharacterAct
{
    public int characterId;
    public CharacterEmotionType emotion;
    
    public CharacterAct(int characterId, CharacterEmotionType emotion)
    {
        this.characterId = characterId;
        this.emotion = emotion;
    }
}
