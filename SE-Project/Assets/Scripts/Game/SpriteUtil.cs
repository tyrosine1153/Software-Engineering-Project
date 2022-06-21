using UnityEngine;

public static class SpriteUtil
{
    private const string PotionSpritePathFormat = "Sprites/Potions/{0}";

    public static Sprite LoadPotionSprite(int id)
    {
        return Resources.Load<Sprite>(string.Format(PotionSpritePathFormat, id))
               ?? Resources.Load<Sprite>(string.Format(PotionSpritePathFormat, 0));
    }
}
