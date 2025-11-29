using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private Button button;

    public ItemSO Item => item;

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
    }

    public void ClearItem()
    {
        item = null;
        itemCount = 0;
        iconImage.sprite = null;
        iconImage.enabled = false;

        UpdateCountText();
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
        targetSlot.SetItem(item);
        targetSlot.itemCount = itemCount;
        targetSlot.UpdateCountText();

        ClearItem();
    }

    public void SwapItemWith(Slot targetSlot)
    {
        var tempItem = item;
        var tempCount = itemCount;

        SetItem(targetSlot.Item);
        itemCount = targetSlot.itemCount;
        UpdateCountText();

        targetSlot.SetItem(tempItem);
        targetSlot.itemCount = tempCount;
        targetSlot.UpdateCountText();
    }

    private void UpdateCountText()
    {
        countText.text = itemCount.ToString();
        countText.enabled = itemCount > 1;
    }



    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item == null) return;
        inventory.SetDraggedFrom(this);

        SetIconAlpha(0.6f);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (inventory.DraggedFromSlot == null) return;

        inventory.DraggedFromSlot.SetIconAlpha(1f);
        inventory.SetDraggedTo(this);

        if (item != null)
        {
            SwapItemWith(inventory.DraggedFromSlot);
        }
        else 
        {
            inventory.DraggedFromSlot.MoveItemTo(this);
        }

        inventory.SetDraggedFrom(null);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (inventory.DraggedToSlot == null)
        {
            SetIconAlpha(1f);
            ClearItem();
        }
        else
        {
            inventory.SetDraggedTo(null);
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