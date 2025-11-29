[System.Serializable]
public class Material
{
    public ItemSO Item;
    public int Quantity;

    public Material(ItemSO item, int quantity)
    {
        Item = item;
        Quantity = quantity;
    }
}