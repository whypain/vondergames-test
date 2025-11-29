using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] InputActionReference toggleInventoryAction;
    [SerializeField] InputActionReference useItemAction;
    [SerializeField] Inventory inventory;
    [SerializeField] InventoryBar inventoryBar;
    [SerializeField] Transform itemHoldPoint;

    private bool isInventoryOpen;
    private bool isMouseOverUI;
    private Item currItem;

    private void Start()
    {
        inventory.gameObject.SetActive(false);
        isInventoryOpen = false;

        OnItemChanged(inventoryBar.CurrSlot.Item);

        useItemAction.action.Enable();
    }

    private void OnEnable()
    {
        toggleInventoryAction.action.performed += ToggleInventory;
        useItemAction.action.performed += UseItem;
        inventoryBar.OnCurrItemChanged += OnItemChanged;

        toggleInventoryAction.action.Enable();
    }

    private void OnDisable()
    {
        toggleInventoryAction.action.performed -= ToggleInventory;
        useItemAction.action.performed -= UseItem;

        toggleInventoryAction.action.Disable();
    }

    private void Update()
    {
        isMouseOverUI = EventSystem.current.IsPointerOverGameObject();
    }

    private void ToggleInventory(InputAction.CallbackContext context)
    {
        isInventoryOpen = !isInventoryOpen;
        inventory.gameObject.SetActive(isInventoryOpen);

        if (isInventoryOpen) useItemAction.action.Disable();
        else useItemAction.action.Enable();

        inventoryBar.SetDraggable(isInventoryOpen);
    }


    private void UseItem(InputAction.CallbackContext context)
    {
        if (currItem == null || isMouseOverUI) return;

        currItem.Use();
        if (inventoryBar.CurrItem.IsConsumable)
        {
            inventoryBar.RemoveItem(inventoryBar.CurrSlot, 1);
        }
    }

    private void OnItemChanged(ItemSO item)
    {
        if (currItem != null)
        {
            Destroy(currItem.gameObject);
            currItem = null;
        }
        if (item != null) currItem = Instantiate(item.ItemPrefab, itemHoldPoint);
    }
}
