using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Recipe
{
    public readonly int PotionId;
    public bool IsPotionUnlocked;
    public bool IsRecipeUnlock;
    public OpenStatus Status;
    
    public Recipe(int potionId, bool isPotionUnlocked, bool isRecipeUnlock, OpenStatus status)
    {
        PotionId = potionId;
        IsPotionUnlocked = isPotionUnlocked;
        IsRecipeUnlock = isRecipeUnlock;
        Status = status;
    }
}

public enum OpenStatus
{
    Hide,
    PotionUnlocked,
    RecipeUnlock
}
public class RecipeModel : Singleton<RecipeModel>
{
    public List<Recipe> DailyUnlockedRecipes = new List<Recipe>();
    public List<Recipe> Recipes = new List<Recipe>();
    public readonly (int start, int end)[] WeeklyUnlockPotion =
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
        Recipes = DataManager.Instance.potions
            .Select(potion => new Recipe(potion.id, false, false, OpenStatus.Hide))
            .ToList();
    }

    public void ResetDailyUnlockedRecipes()
    {
        // 이전 주에 새로 만든 포션 리스트 초기화 
        DailyUnlockedRecipes = new List<Recipe>();
    }
    
    public void AddDailyUnlockedRecipes(int id)
    {
        DailyUnlockedRecipes.Add(Recipes[id]);
    }

    public void UnlockWeeklyPotions(int day)
    {
        if (day == 0)
        {
            Recipes[0].IsPotionUnlocked = true;

        }
        else
        {
            var week = day / 7;
            // 이번주 포션 해금, week 0 = Tutorial
            var potionRange = WeeklyUnlockPotion[week];
        
            for (var i = potionRange.start; i <= potionRange.end; i++)
            {
                Recipes[i].IsPotionUnlocked = true;
            }
        }
    }

    public List<Potion> GetUnlockedPotions()
    {
        var potions = DataManager.Instance.potions;
        return Recipes.Where(recipe => recipe.IsPotionUnlocked)
            .Select(recipe => potions.First(potion => potion.id == recipe.PotionId)).ToList();
    }
    
    public List<Potion> GetUnlockedRecipes()
    {
        var potions = DataManager.Instance.potions;
        return Recipes.Where(recipe => recipe.IsRecipeUnlock)
            .Select(recipe => potions.First(potion => potion.id == recipe.PotionId)).ToList();
    }

    // === === === === END === === === === //
    
    public List<Recipe> GetShowRecipes()
    {
        foreach (var recipe in Recipes)
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

        return Recipes;
    }
    
    public OpenStatus GetRecipeStatus(int id)
    {
        if (Recipes[id].IsRecipeUnlock)
        {
            // 이미 열려 있음
            return OpenStatus.RecipeUnlock;
        }

        if (Recipes[id].IsPotionUnlocked)
        {
            // 새롭게 해금한 레시피
            Recipes[id].IsRecipeUnlock = true;
            return OpenStatus.PotionUnlocked;
        }
        
        // 미해금 포션
        return OpenStatus.Hide;
    }
}
