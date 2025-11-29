using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class CraftFromBar : MonoBehaviour
{
    [SerializeField] InventoryBar inventoryBar;
    [SerializeField] CraftingController craftingController;
    [SerializeField] Slot resultSlot;
    [SerializeField] CanvasGroup canvasGroup;

    [SerializeField] ItemSO testItem;
    [SerializeField] int testQuantity;

    private Recipe cacheRecipe;


    private void OnEnable()
    {
        inventoryBar.OnCurrItemChanged += OnCurrItemChanged;
    }

    private void Start()
    {
        resultSlot.Initialize(null);
        resultSlot.Button.onClick.AddListener(() => Craft());

        canvasGroup.alpha = 0f;
    }

    private void OnCurrItemChanged(ItemSO item)
    {
        var recipe = TryGetRecipe();
        if (recipe != null)
        {
            resultSlot.SetItem(recipe.Result);
            canvasGroup.alpha = 1f;
        }
        else
        {
            resultSlot.ClearItem();
            canvasGroup.alpha = 0f;
        }
    }

    private void Craft()
    {
        if (cacheRecipe == null) return;

        List<Material> materials = new List<Material>() { new Material(inventoryBar.CurrItem, inventoryBar.CurrSlot.ItemCount) };
        ItemSO craftedItem = craftingController.Craft(cacheRecipe, ref materials);
        if (craftedItem != null)
        {
            inventoryBar.CurrSlot.SetItem(inventoryBar.CurrSlot.Item, materials[0].Quantity);
            inventoryBar.AddItem(craftedItem, 1);
        }
    }

    public Recipe TryGetRecipe()
    {
        var currItem = inventoryBar.CurrItem;
        if (currItem == null)
            return null;

        var result = craftingController.GetCraftingRecipe(new List<Material>()
        {
            new Material(inventoryBar.CurrItem, inventoryBar.CurrSlot.ItemCount)
        });

        // string name = result != null ? result.Name : "Not Found";
        // Debug.Log($"Checking {inventoryBar.CurrItem.Name} x {inventoryBar.CurrSlot.ItemCount}: {name}");
        cacheRecipe = result;
        return result;
    }


    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private void TestDetectRecipe()
    {
        var recipe = craftingController.GetCraftingRecipe(new List<Material>()
        {
            new Material(testItem, testQuantity)
        });

        Debug.Log($"Testing Material of {testItem.Name} x {testQuantity}: ");
        Debug.Log(recipe == null ? "Not Found" : recipe.Result.Name);
    }
}
