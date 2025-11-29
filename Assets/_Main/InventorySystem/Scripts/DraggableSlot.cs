using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableSlot : Slot, IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler
{
    [Range(0f, 1f)]
    [SerializeField] private float dragIconAlpha = 0.6f;

    [Range(0f, 1f)]
    [SerializeField] private float normalIconAlpha = 1f;

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (item == null) return;
        Inventory.SetDraggedFrom(this);

        SetIconAlpha(dragIconAlpha);
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        if (Inventory.DraggedFromSlot == null) return;

        Inventory.DraggedFromSlot.SetIconAlpha(normalIconAlpha);
        Inventory.SetDraggedTo(this);

        if (item != null && item != Inventory.DraggedFromSlot.Item)
        {
            SwapItemWith(Inventory.DraggedFromSlot);
        }
        else if (item != null && item == Inventory.DraggedFromSlot.Item)
        {
            Inventory.DraggedFromSlot.MergeItemTo(this);
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
            Inventory.DraggedFromSlot?.Inventory?.RemoveItem(this, Inventory.DraggedFromSlot.ItemCount);
        }
        else
        {
            Inventory.SetDraggedTo(null);
        }
    }


    public virtual void OnDrag(PointerEventData eventData)
    {
        // 
    }
}
