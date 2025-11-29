using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryBar : Inventory
{
    [SerializeField] Transform selectedFrame;
    [SerializeField] List<InputActionReference> hotkeyActions;

    public ItemSO CurrItem => currItem;
    public HotbarSlot CurrSlot => currSlot;

    public Action<HotbarSlot> OnCurrSlotChanged;
    public Action<ItemSO> OnCurrItemChanged;

    private HotbarSlot currSlot;
    private ItemSO currItem => currSlot?.Item;

    protected override void Awake()
    {
        base.Awake();
        InitHotkeys();

        currSlot = slots[0] as HotbarSlot;
        currSlot.OnItemChanged += OnItemChanged;
    }

    public void SetDraggable(bool draggable)
    {
        foreach (var slot in slots)
        {
            (slot as HotbarSlot).SetDraggable(draggable);
        }
    }

    private void InitHotkeys()
    {
        for (int i = 0; i < hotkeyActions.Count; i++)
        {
            int index = i; // Capture the current index
            hotkeyActions[index].action.performed += ctx => OnHotkeyPressed(index);
            hotkeyActions[index].action.Enable();

            slots[index].Button.onClick.AddListener(() => OnHotkeyPressed(index));
        }
    }

    private void OnItemChanged(ItemSO item)
    {
        OnCurrItemChanged?.Invoke(item);
    }

    private void OnHotkeyPressed(int index)
    {
        if (index < 0 || index >= slots.Count) return;
        if (currSlot == slots[index]) return;

        currSlot.OnItemChanged -= OnItemChanged;
        currSlot = slots[index] as HotbarSlot;
        currSlot.OnItemChanged += OnItemChanged;

        selectedFrame.position = currSlot.transform.position;
        OnCurrSlotChanged?.Invoke(currSlot);
        OnCurrItemChanged?.Invoke(currItem);
    }
}