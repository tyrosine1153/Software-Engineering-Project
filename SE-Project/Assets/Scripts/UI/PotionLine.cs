using UnityEngine;
using UnityEngine.UI;

public class PotionLine : MonoBehaviour
{
    public Image image;
    public Text nameText;
    public Text recipeText;

    public void Set(Sprite sprite, string potionName, int[] recipe = null)
    {
        image.sprite = sprite;
        nameText.text = potionName;
        recipeText.text = recipe == null ? "?" : $"R-{recipe[0]} G-{recipe[1]} B-{recipe[2]}";
    }
}
