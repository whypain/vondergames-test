using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryBar : Inventory
{
    [SerializeField] Transform selectedFrame;
    [SerializeField] List<InputActionReference> hotkeyActions;

    public ItemSO CurrItem => currItem;
    public Slot CurrSlot => currSlot;
    public Action<Slot> OnCurrSlotChanged;

    private Slot currSlot;
    private ItemSO currItem => currSlot?.Item;

    protected override void Awake()
    {
        base.Awake();
        InitHotkeys();

        currSlot = slots[0];
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

    private void OnHotkeyPressed(int index)
    {
        currSlot = slots[index];
        selectedFrame.position = currSlot.transform.position;
        OnCurrSlotChanged?.Invoke(currSlot);
    }
}