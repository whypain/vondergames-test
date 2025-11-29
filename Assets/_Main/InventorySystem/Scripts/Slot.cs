using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private Button button;

    [Range(0f, 1f)]
    [SerializeField] private float dragIconAlpha = 0.6f;

    [Range(0f, 1f)]
    [SerializeField] private float normalIconAlpha = 1f;

    public ItemSO Item => item;
    public int ItemCount => itemCount;
    public Button Button => button;

    public Action<ItemSO> OnItemChanged;

    private ItemSO item;
    private int itemCount;
    private Inventory inventory;

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



    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (item == null) return;
        Inventory.SetDraggedFrom(this);

        SetIconAlpha(dragIconAlpha);
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        if (Inventory.DraggedFromSlot == null) return;
        // Debug.Log($"From {Inventory.DraggedFromSlot.inventory.name}/{Inventory.DraggedFromSlot.name} to {inventory.name}/{name}");

        Inventory.DraggedFromSlot.SetIconAlpha(normalIconAlpha);
        Inventory.SetDraggedTo(this);

        if (item != null)
        {
            SwapItemWith(Inventory.DraggedFromSlot);
        }
        else 
        {
            Inventory.DraggedFromSlot.MoveItemTo(this);
        }

        Inventory.SetDraggedFrom(null);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (Inventory.DraggedToSlot == null)
        {
            SetIconAlpha(normalIconAlpha);
            Inventory.DraggedFromSlot.inventory.RemoveItem(this, Inventory.DraggedFromSlot.ItemCount);
        }
        else
        {
            Inventory.SetDraggedTo(null);
        }
    }


    public void OnDrag(PointerEventData eventData)
    {
        // 
    }
}

public static class ColorExtensions
{
    public static Color WithAlpha(this Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }
}