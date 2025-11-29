using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemSO : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public Item ItemPrefab;
    public int MaxStack;
    public bool IsConsumable;

    public List<Material> CraftingMaterials;
}
