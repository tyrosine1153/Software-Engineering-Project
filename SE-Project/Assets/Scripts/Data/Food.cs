public class Food
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

public class FoodUtil
{
    public static bool TryMakeFood(int[] materialCount, ref Food result)
    {
        var material = 0;
        for (int i = 0; i < materialCount.Length; i++)
        {
            material += materialCount[i] * (10 ^ i);
        }

        foreach (var food in DataManager.Instance.Foods)
        {
            if (food.Material == material)
            {
                result = food;
                return true;
            }
        }
        return false;
    }
}
