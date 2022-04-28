public struct Potion  // json
{
    public int ID;
    public string Name;
    public string Description;
    public Material PotionType;
    public int[] Material;
}

public enum Material
{
    R,
    G,
    B,
}
