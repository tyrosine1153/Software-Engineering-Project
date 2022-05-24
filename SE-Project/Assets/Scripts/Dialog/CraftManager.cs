using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum PotionMaterials
{
    Red,
    Green,
    Blue,
    Cyan,
    Magenta,
    Yellow,
    Max
}

public class CraftManager : MonoBehaviour
{
    [SerializeField] private Button resetButton;
    [SerializeField] private Button submitButton;
    [SerializeField] private Button[] addColorButtons;
    [SerializeField] private Image potionImage;
    
    private const int MaxCount = 10000;
    private const int MaterialLimit = 5;
    private const float ColorRatio = 1f / MaterialLimit;
    private int totalCount;
    private readonly int[] count = new int[(int) PotionMaterials.Max];

    private void ResetCraftData()
    {
        totalCount = 0;
        for (var i = 0; i < count.Length; i++)
        {
            count[i] = 0;
        }
        ShowPotion();
    }
    private void AddPotionMaterial(PotionMaterials potionMaterial)
    {
        // 전체 조합 물약 개수 제한 
        if(MaxCount < totalCount) return;
        
        var index = (int) potionMaterial;
        
        // 물약 종류별 제한
        if (MaterialLimit <= count[index]) return;
        
        totalCount++;
        count[index]++;
        ShowPotion();
    }

    private void ShowPotion()
    {
        var r = count[(int) PotionMaterials.Red] - count[(int) PotionMaterials.Cyan];
        var g = count[(int) PotionMaterials.Green] - count[(int) PotionMaterials.Magenta];
        var b = count[(int) PotionMaterials.Blue] - count[(int) PotionMaterials.Yellow];
        
        ShowPotionEffect(r < 0, b < 0, g < 0);
        
        var color = new Color(Math.Abs(r) * ColorRatio, Math.Abs(g) * ColorRatio, Math.Abs(b) * ColorRatio);
        
        potionImage.color = color;
    }

    private void ShowPotionEffect(bool redEffect, bool greenEffect, bool blueEffect)
    {
        // 색깔별 이펙트 표시
    }

    private void SubmitColor()
    {
        var r = count[(int) PotionMaterials.Red];
        var g = count[(int) PotionMaterials.Green];
        var b = count[(int) PotionMaterials.Blue];
        var c = count[(int) PotionMaterials.Cyan];
        var y = count[(int) PotionMaterials.Yellow];
        var m = count[(int) PotionMaterials.Magenta];
        
        ResetCraftData();
        var foodId = PotionToPotionId(r, g, b, c, y, m);
        var food = FindFood(foodId);
        if (food.ID == -1)
        {
            // 없는 조합
            print("Food Not Found");
            return;
        }
        StoryManager.Instance.GetFoodStory(food);
    }

    private static int[] PotionToPotionId(int r, int g, int b, int c, int y, int m)
    {
        return new[] {r - c, g - y, b - m};
    }

    private static Potion FindFood(int[] foodId)
    {
        foreach (var potion in DataManager.Instance.Potions)
        {
            if (CompareMaterial(potion, foodId)) return potion;
        }

        return new Potion()
        {
            ID = -1
        };
    }

    private static bool CompareMaterial(Potion potion, int[] potionId )
    {
        return !potionId.Where((potionIdIndex, i) => potion.Material[i] != potionIdIndex).Any();
    }
    
    private void Start()
    {
        ResetCraftData();
        resetButton.onClick.AddListener(ResetCraftData);
        submitButton.onClick.AddListener(SubmitColor);
        for (var i = 0; i < (int) PotionMaterials.Max; i++)
        {
            var arg = (PotionMaterials) i;
            addColorButtons[i].onClick.AddListener(() => AddPotionMaterial(arg));
        }
    }
}
