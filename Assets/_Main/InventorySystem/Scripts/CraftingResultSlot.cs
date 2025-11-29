using UnityEngine.EventSystems;

public class CraftingResultSlot : Slot
{
    public override void OnDrop(PointerEventData eventData)
    {
        // Disable dropping into crafting result slot
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        // Disable end drag from crafting result slot
    }
}
