using System.Collections.Generic;
using UnityEngine;

public class CraftingController : MonoBehaviour
{
    [SerializeField] List<ItemSO> items;

    private Dictionary<Recipe, ItemSO> recipes;

    void Awake()
    {
        recipes = new Dictionary<Recipe, ItemSO>();

        foreach (var item in items)
        {
            var recipe = new Recipe(item, new List<Material>());

            if (item.CraftingMaterials == null || item.CraftingMaterials.Count == 0)
                continue;

            recipe.Materials.AddRange(item.CraftingMaterials);
            recipes[recipe] = item;
        }
    }

    public Recipe GetCraftingRecipe(List<Material> providedMaterials)
    {
        foreach (var recipe in recipes.Keys)
        {
            if (recipe.CanCraftFrom(providedMaterials))
            {
                return recipe;
            }
        }

        return null;
    }

    public ItemSO Craft(Recipe recipe, ref List<Material> materials)
    {
        if (!recipes.ContainsKey(recipe)) return null;

        if (recipe.TryCraftFrom(ref materials))
        {
            // Crafting successful
            Debug.Log($"Crafted: {recipes[recipe].Name}");
            return recipes[recipe];
        }

        return null;
    }
}
