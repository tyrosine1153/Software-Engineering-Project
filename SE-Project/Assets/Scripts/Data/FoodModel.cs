public class Food  // csv? json?
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public int Material { get; set; }
}

public enum Material
{
    Meat,
    Vegetable,
    Fruit,
    Dairy,
    Grain,
    Oil,
    Other,
}
