using System;

[Serializable]
public class Potion
{
    public int id;
    public string name;
    public string description;
    public Material potionType;
    public int[] material;
}

public enum Material
{
    R,
    G,
    B,
}
