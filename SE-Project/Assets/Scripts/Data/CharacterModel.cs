using System;

[Serializable]
public class Character
{
    public int id;
    public string name;
    public string description;
    public CharacterEmotionType[] emotion;
}