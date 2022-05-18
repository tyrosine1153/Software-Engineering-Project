public struct StoryScenario  // json
{
    public int ID { get; set; }
    public int PrevId { get; set; }
    public int NextId { get; set; }
    
    public string Speaker { get; set; }
    public string Content { get; set; }
    public int[] Effects { get; set; }  // 일러스트는 나중에 이미지 경로로 대체할지 고민중
    
    public CharacterAct[] Characters { get; set; }
    
    public OrderOption[] Order { get; set; }
}

public struct EndingPoint // json
{
    public int ID { get; set; }
    public int NextScenarioID { get; set; }
    public int Result { get; set; }  // 결과값 (임시)
}

public struct Character  // json
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public CharacterEmotionType[] Emotion { get; set; }
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

public class CharacterAct
{
    public int CharacterId { get; set; }
    public CharacterEmotionType Emotion { get; set; }    
    
    public CharacterAct(int characterId, CharacterEmotionType emotion)
    {
        CharacterId = characterId;
        Emotion = emotion;
    }
}

public struct OrderOption
{
    // PotionID에 따라 결정되는 NextScenarioID, Result의 모임 자료형
    // -1의 경우 정의된 옵션 외의 값을 뜻함
    public int PotionId { get; set; }
    public int NextScenarioID { get; set; }
    public int Result { get; set; }  // 결과값 (임시) 
    
    public OrderOption(int potionId, int nextScenarioID, int result)
    {
        PotionId = potionId;
        NextScenarioID = nextScenarioID;
        Result = result;
    }
}