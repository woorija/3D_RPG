using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentItemSlot : ItemSlotBase
{
    [SerializeField] EquipmentType slotIndex;
    public void SetSlot()
    {
        ItemBase _item = EquipmentData.Instance.GetEquipmentItemData(slotIndex);
        ChangeSlotData(_item);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (!DragManager.Instance.isClick)
        {
            if (iconImage.sprite != null)
            {
                ItemBase temp = EquipmentData.Instance.GetEquipmentItemData(slotIndex);
                ItemInformationUI.Instance.SetInformation(temp);
            }
        }
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (!DragManager.Instance.isClick)
        {
            if (iconImage.sprite != null)
            {
                DragManager.Instance.DragStart(DragUIType.Equipment, (int)slotIndex, iconImage.sprite);
            }
        }
        else
        {
            DragManager.Instance.DragEndToEquipment((int)slotIndex);
        }
    }
}
