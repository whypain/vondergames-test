using System.Collections.Generic;

public class Recipe
{
    public List<Material> Materials;
    public ItemSO Result;

    public Recipe(ItemSO result, List<Material> materials)
    {
        Result = result;
        Materials = materials;
    }

    public bool CanCraftFrom(List<Material> otherMaterials)
    {
        if (Materials.Count != otherMaterials.Count)
            return false;

        foreach (var material in Materials)
        {
            var matchingMaterial = otherMaterials.Find(m => m.Item == material.Item);
            if (matchingMaterial == null || matchingMaterial.Quantity < material.Quantity)
                return false;
        }

        return true;
    }

    public bool TryCraftFrom(ref List<Material> providedMaterials)
    {
        if (Materials.Count != providedMaterials.Count)
            return false;

        foreach (var material in Materials)
        {
            var matchingMaterial = providedMaterials.Find(m => m.Item == material.Item);
            if (matchingMaterial == null || matchingMaterial.Quantity < material.Quantity)
                return false;

            matchingMaterial.Quantity -= material.Quantity;
        }

        return true;
    }
}
