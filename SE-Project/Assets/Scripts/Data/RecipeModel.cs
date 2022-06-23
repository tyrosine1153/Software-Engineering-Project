using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Recipe
{
    public int PotionId { get; set; }
    public bool IsPotionUnlocked { get; set; }
    public bool IsRecipeUnlock { get; set; }
    public OpenStatus Status { get; set; }
}

public enum OpenStatus
{
    Hide,
    PotionUnlocked,
    RecipeUnlock
}
public class RecipeModel : Singleton<RecipeModel>
{
    public List<Recipe> weeklyUnlockedRecipes = new List<Recipe>();
    public List<Recipe> recipes = new List<Recipe>();
    public readonly (int start, int end)[] potionOpenWeekendData =
    {
        (0, 3),
        (4, 7),
        (8, 10),
        (11, 13),
    };

    private void Start()
    {
        ResetRecipes();
    }

    public void ResetRecipes()
    {
        foreach (var potion in DataManager.Instance.potions)
        {
            recipes.Add(
                new Recipe
                {
                    PotionId = potion.id,
                    IsPotionUnlocked = false,
                    IsRecipeUnlock = false,
                    Status = OpenStatus.Hide
                }
             );
        }
    }

    public void ResetWeeklyUnlockedRecipes()
    {
        // 이전 주에 새로 만든 포션 리스트 초기화 
        weeklyUnlockedRecipes = new List<Recipe>();
    }
    
    public void AddWeeklyUnlockedRecipes(int id)
    {
        weeklyUnlockedRecipes.Add(recipes[id]);
    }

    public void UnlockWeeklyRecipes(int week)
    {
        // 이번주 레시피 해금
        // week 0 = Tutorial
        var potionIdRange = potionOpenWeekendData[week];
        
        for (var i = potionIdRange.start; i <= potionIdRange.end; i++)
        {
            recipes[i].IsPotionUnlocked = true;
        }
    }

    public List<Potion> GetUnlockedPotions()
    {
        var potions = DataManager.Instance.potions;
        return recipes.Where(recipe => recipe.IsPotionUnlocked)
            .Select(recipe => potions.First(potion => potion.id == recipe.PotionId)).ToList();
    }

    // === === === === END === === === === //
    
    public List<Recipe> GetShowRecipes()
    {
        foreach (var recipe in recipes)
        {
            if (!recipe.IsRecipeUnlock)
            {
                recipe.Status = OpenStatus.Hide;
            }
            else if (!recipe.IsPotionUnlocked)
            {
                recipe.Status = OpenStatus.PotionUnlocked;
            }
            else
            {
                recipe.Status = OpenStatus.RecipeUnlock;
            }
        }

        return recipes;
    }
    
    public OpenStatus GetRecipeStatus(int id)
    {
        if (recipes[id].IsRecipeUnlock)
        {
            // 이미 열려 있음
            return OpenStatus.RecipeUnlock;
        }

        if (recipes[id].IsPotionUnlocked)
        {
            // 새롭게 해금한 레시피
            recipes[id].IsRecipeUnlock = true;
            return OpenStatus.PotionUnlocked;
        }
        
        // 미해금 포션
        return OpenStatus.Hide;
    }
}
