using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private Button button;


    public ItemSO Item => item;
    public int ItemCount => itemCount;
    public Button Button => button;
    public Inventory Inventory => inventory;

    public Action<ItemSO> OnItemChanged;

    protected ItemSO item;
    protected int itemCount;
    protected Inventory inventory;

    public void Initialize(Inventory inventory)
    {
        this.inventory = inventory;
        ClearItem();
    }

    public void SetItem(ItemSO newItem)
    {
        item = newItem;
        iconImage.sprite = item.Icon;
        iconImage.enabled = true;

        UpdateCountText();
        OnItemChanged?.Invoke(item);
    }

    public void SetItem(ItemSO newItem, int count)
    {
        item = newItem;
        itemCount = count;
        iconImage.sprite = item.Icon;
        iconImage.enabled = true;

        UpdateCountText();
        OnItemChanged?.Invoke(item);
    }

    public void ClearItem()
    {
        item = null;
        itemCount = 0;
        iconImage.sprite = null;
        iconImage.enabled = false;

        UpdateCountText();
        OnItemChanged?.Invoke(item);
    }

    public bool TryAddItem(int count, out int remainder)
    {
        bool result = false;

        if (item == null)
        {
            remainder = count;
            result = false;
        }
        else if (itemCount + count > item.MaxStack)
        {
            remainder = itemCount + count - item.MaxStack;
            itemCount = item.MaxStack;
            result = true;
        }
        else
        {
            itemCount += count;
            remainder = 0;
            result = true;
        }

        UpdateCountText();
        return result;
    }

    public bool TryRemoveItem(int count)
    {
        if (item == null || count < 0)
        {
            return false;
        }

        itemCount = Mathf.Max(itemCount - count, 0);

        if (itemCount == 0)
        {
            ClearItem();
        }
        else
        {
            UpdateCountText();
        }

        return true;
    }

    public bool IsFull()
    {
        return item != null && itemCount >= item.MaxStack;
    }

    public void SetIconAlpha(float alpha)
    {
        iconImage.color = iconImage.color.WithAlpha(alpha);
    }

    public void MoveItemTo(Slot targetSlot)
    {
        targetSlot.SetItem(item, itemCount);
        targetSlot.UpdateCountText();

        ClearItem();
    }

    public void SwapItemWith(Slot targetSlot)
    {
        var tempItem = item;
        var tempCount = itemCount;

        SetItem(targetSlot.Item, targetSlot.ItemCount);
        UpdateCountText();

        targetSlot.SetItem(tempItem, tempCount);
        targetSlot.UpdateCountText();
    }

    private void UpdateCountText()
    {
        countText.text = itemCount.ToString();
        countText.enabled = itemCount > 1;
    }
}

public static class ColorExtensions
{
    public static Color WithAlpha(this Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }
}