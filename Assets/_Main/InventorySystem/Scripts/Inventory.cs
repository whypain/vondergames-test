using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] Transform slotsParent;
    [SerializeField] ItemSO TestItem;

    private Slot selectedSlot;
    private List<Slot> slots;

    void Awake()
    {
        slots = new List<Slot>();
        InitializeSlots();
    }

    public void AddItem(ItemSO itemSO, int count)
    {
        if (FindAvailableSlot(itemSO, out Slot foundSlot))
        {
            if (foundSlot.TryAddItem(count, out int remainder))
            {
                if (remainder > 0)
                {
                    AddItem(itemSO, remainder);
                }
                return;
            } 
        }

        foreach (var slot in slots)
        {
            if (!slot.IsFull() && slot.Item == null)
            {
                slot.SetItem(itemSO);
                AddItem(itemSO, count);
                return;
            }
        }
    }

    public void SelectSlot(Slot slot)
    {
        selectedSlot = slot;
    }


    private void InitializeSlots()
    {
        Slot[] slots = slotsParent.GetComponentsInChildren<Slot>();
        foreach (var slot in slots)
        {
            slot.Initialize(this);
            this.slots.Add(slot);
        }
    }

    private bool FindAvailableSlot(ItemSO itemSO, out Slot foundSlot)
    {
        foreach (var slot in slots)
        {
            if (slot.Item == itemSO && !slot.IsFull())
            {
                foundSlot = slot;
                return true;
            }
        }
        foundSlot = null;
        return false;
    }


    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private void TestAddItem()
    {
        AddItem(TestItem, 1);
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private void TestAdd10Item()
    {
        AddItem(TestItem, 10);
    }
}
