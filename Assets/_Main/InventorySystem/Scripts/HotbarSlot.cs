using UnityEngine.EventSystems;

public class HotbarSlot : Slot
{
    private bool isDraggable;

    public void SetDraggable(bool draggable)
    {
        isDraggable = draggable;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (!isDraggable) return;
        base.OnBeginDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!isDraggable) return;
        base.OnEndDrag(eventData);
    }

    public override void OnDrop(PointerEventData eventData)
    {
        if (!isDraggable) return;
        base.OnDrop(eventData);
    }
}
