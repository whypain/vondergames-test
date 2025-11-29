using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] Transform slotsParent;
    [SerializeField] ItemSO TestItem;

    public bool IsFull { get; private set; }
    public static Slot DraggedFromSlot => draggedFromSlot;
    public static Slot DraggedToSlot => draggedToSlot;

    private static Slot draggedFromSlot;
    private static Slot draggedToSlot;
    protected List<Slot> slots;

    protected virtual void Awake()
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

        IsFull = true;
        Debug.Log("Inventory is full!");
    }

    public void RemoveItem(ItemSO item, int count)
    {
        if (!DoesItemExists(item, out Slot foundSlot))
        {
            Debug.LogWarning("Item does not exist in inventory!");
            return;
        }

        RemoveItem(foundSlot, count);
    }

    public void RemoveItem(Slot slot, int count)
    {
        if (!slot.TryRemoveItem(count))
        {
            Debug.LogWarning("Not enough items to remove!");
        }

        IsFull = false;
    }

    public static void SetDraggedFrom(Slot slot)
    {
        draggedFromSlot = slot;
    }

    public static void SetDraggedTo(Slot slot)
    {
        draggedToSlot = slot;
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

    private bool DoesItemExists(ItemSO itemSO, out Slot foundSlot)
    {
        foreach (var slot in slots)
        {
            if (slot.Item == itemSO)
            {
                foundSlot = slot;
                return true;
            }
        }
        foundSlot = null;
        return false;
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

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private void TestRemoveItem()
    {
        RemoveItem(TestItem, 1);
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private void TestRemove10Item()
    {
        RemoveItem(TestItem, 10);
    }

}
