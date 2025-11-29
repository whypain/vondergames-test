using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
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
        button.onClick.AddListener(OnSlotClicked);

        ClearItem();
    }

    public void SetItem(ItemSO newItem, int count)
    {
        item = newItem;
        itemCount = count;
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

    public bool IsFull()
    {
        return item != null && itemCount >= item.MaxStack;
    }

    private void UpdateCountText()
    {
        countText.text = itemCount.ToString();
        countText.enabled = itemCount > 1;
    }

    private void OnSlotClicked()
    {
        inventory.SelectSlot(this);
    }
}
