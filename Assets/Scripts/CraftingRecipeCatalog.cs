using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftingRecipeCatalog", menuName = "ScriptableObjects/Crafting/Crafting Recipe Catalog")]
public class CraftingRecipeCatalog : ScriptableObject
{
    public List<CraftingRecipe> recipes = new List<CraftingRecipe>();

    public List<CraftingRecipe> GetRecipesForHammerLevel(int hammerLevel)
    {
        List<CraftingRecipe> availableRecipes = new List<CraftingRecipe>();

        foreach (CraftingRecipe recipe in recipes)
        {
            if (recipe != null && recipe.hammerLevelRequired <= hammerLevel)
            {
                availableRecipes.Add(recipe);
            }
        }

        return availableRecipes;
    }
}
