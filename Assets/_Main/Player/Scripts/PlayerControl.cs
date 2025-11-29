using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] InputActionReference toggleInventoryAction;
    [SerializeField] InputActionReference useItemAction;
    [SerializeField] Inventory inventory;
    [SerializeField] InventoryBar inventoryBar;
    [SerializeField] Transform itemHoldPoint;

    private bool isInventoryOpen = false;
    private Slot currSlot;
    private Item currItem;

    private void Start()
    {
        inventory.gameObject.SetActive(false);
        isInventoryOpen = false;

        currSlot = inventoryBar.CurrSlot;
        currSlot.OnItemChanged += OnItemChanged;
        OnItemChanged(currSlot.Item);

        useItemAction.action.Enable();
    }

    private void OnEnable()
    {
        toggleInventoryAction.action.performed += ToggleInventory;
        useItemAction.action.performed += UseItem;
        inventoryBar.OnCurrSlotChanged += OnCurrSlotChanged;

        toggleInventoryAction.action.Enable();
    }

    private void OnDisable()
    {
        toggleInventoryAction.action.performed -= ToggleInventory;
        useItemAction.action.performed -= UseItem;

        toggleInventoryAction.action.Disable();
    }

    private void ToggleInventory(InputAction.CallbackContext context)
    {
        isInventoryOpen = !isInventoryOpen;
        inventory.gameObject.SetActive(isInventoryOpen);

        if (isInventoryOpen) useItemAction.action.Disable();
        else useItemAction.action.Enable();
    }


    private void UseItem(InputAction.CallbackContext context)
    {
        if (currItem == null) return;
        currItem.Use();
        if (inventoryBar.CurrItem.IsConsumable)
        {
            inventoryBar.RemoveItem(currSlot, 1);
        }
    }

    private void OnCurrSlotChanged(Slot newSlot)
    {
        currSlot.OnItemChanged -= OnItemChanged;
        currSlot = newSlot;
        currSlot.OnItemChanged += OnItemChanged;

        OnItemChanged(currSlot.Item);
    }

    private void OnItemChanged(ItemSO item)
    {
        if (currItem == item) return;

        if (currItem != null)
        {
            Destroy(currItem.gameObject);
            currItem = null;
        }
        if (item != null) currItem = Instantiate(item.ItemPrefab, itemHoldPoint);
    }
}
