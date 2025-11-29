using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemSO : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public Item ItemPrefab;
    public int MaxStack;
}
