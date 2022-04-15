public struct StoryScenario  // json
{
    public int ID { get; set; }
    public int PrevId { get; set; }
    public int NextId { get; set; }
    
    public string Speaker { get; set; }
    public string Content { get; set; }
    public int[] Effects { get; set; }  // 일러스트는 나중에 이미지 경로로 대체할지 고민중
    
    public CharacterAct[] Characters { get; set; }
}

// Index of Effects array in StoryScenario
public enum EffectType
{
    None, // if it is 0, it means no effect
    Dark, // -3 : All, -2 : Others, -1 : None, 0~n : Position
    Zoom, // -1 : None, 0~n : Position
    Illustration, // -1 : None, 0~n : illustration id
}

public struct Character  // json
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public CharacterEmotionType[] Emotion { get; set; }
}

public enum CharacterEmotionType
{
    Idle,
    Happy,
    Sad,
}

public class CharacterAct
{
    public int CharacterID { get; set; }
    public CharacterEmotionType Emotion { get; set; }    
    
    public CharacterAct(int id, CharacterEmotionType emotion)
    {
        CharacterID = id;
        Emotion = emotion;
    }
}

