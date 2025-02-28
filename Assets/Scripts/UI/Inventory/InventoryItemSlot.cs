using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryItemSlot : ItemSlotBase
{
    public int slotIndex {  get; private set; }
    TMP_Text countText;
    [SerializeField] Image cooltimeBar;
    protected void Awake()
    {
        countText = GetComponentInChildren<TMP_Text>();
    }
    public void SetSlotIndex(int _index)
    {
        slotIndex = _index;
    }
    void ChangeCount(int _num)
    {
        if (_num > 1)
        {
            countText.text = _num.ToString();
        }
        else
        {
            countText.text = string.Empty;
        }
    }
    public void UpdateCooltimeBar(float _fillAmount)
    {
        cooltimeBar.fillAmount = _fillAmount;
    }
    public void ResetCooltimeBar()
    {
        cooltimeBar.fillAmount = 0f;
    }
    protected override void ChangeSlotData(ItemBase _item)
    {
        base.ChangeSlotData(_item);
        ChangeCount(_item.curruntAmount);
    }
    public void SetSlot(ItemType _itemType)
    {
        ItemBase item;
        switch (_itemType)
        {
            case ItemType.Equipment:
                item = InventoryData.Instance.GetEquipmentItemData(slotIndex);
                break;
            case ItemType.Useable:
                item = InventoryData.Instance.GetUseableItemData(slotIndex);
                break;
            case ItemType.Misc:
                item = InventoryData.Instance.GetMiscItemData(slotIndex);
                break;
            default:
                item = null;
                break;
        }
        ChangeSlotData(item);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (!DragManager.Instance.isClick)
        {
            if (iconImage.sprite != null)
            {
                ItemBase temp;
                switch (InventoryData.Instance.GetItemType())
                {
                    case ItemType.Equipment:
                        temp = InventoryData.Instance.GetEquipmentItemData(slotIndex);
                        break;
                    case ItemType.Useable:
                        temp = InventoryData.Instance.GetUseableItemData(slotIndex);
                        break;
                    default:
                        temp = InventoryData.Instance.GetMiscItemData(slotIndex);
                        break;
                }
                ItemInformationUI.Instance.SetInformation(temp);
            }
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                if (!DragManager.Instance.isClick)
                {
                    if (iconImage.sprite != null)
                    {
                        DragManager.Instance.DragStart(DragUIType.Inventory, slotIndex, iconImage.sprite);
                    }
                }
                else
                {
                    DragManager.Instance.DragEndToInventory(slotIndex);
                }
                break;
            case PointerEventData.InputButton.Right:
                break;
        }
    }
}
