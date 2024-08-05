using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEngine.Rendering.DebugUI;

public class ItemSellEndDeleteUI : MonoBehaviour
{
    [SerializeField] NumericInputField field;
    [SerializeField] TMP_Text inforText;
    [SerializeField] ItemDeleteFaleUI deleteFailUI;
    bool isSell;
    ItemBase item;
    int index;
    ItemType type;

    string equipDeleteInforText = "아이템을 버리시겠습니까?";
    string otherDeleteInforText = "버릴 갯수를 입력하세요.";
    string equipSellInforText = "아이템을 판매하시겠습니까?";
    string otherSellInforText = "판매할 갯수를 입력하세요.";
    public void DeleteItem() // 삭제버튼에 등록할것
    {
        switch (type)
        {
            case ItemType.Equipment:
                DeleteEquipmentItem();
                break;
            case ItemType.Useable:
                DeleteUseableItem();
                break;
            case ItemType.Misc:
                DeleteMiscItem();
                break;
        }
        DragManager.Instance.DragReset();
    }
    public void Cancel()
    {
        gameObject.SetActive(false);
    }
    public void GetItem(ItemType _type, int _index, bool _isSell)
    {
        type = _type;
        index = _index;
        isSell = _isSell;
        switch (type)
        {
            case ItemType.Equipment:
                field.gameObject.SetActive(false);
                inforText.text = isSell ? equipSellInforText : equipDeleteInforText;
                break;
            default:
                field.gameObject.SetActive(true);
                inforText.text = isSell ? otherSellInforText : otherDeleteInforText;
                break;
        }
    }
    void DeleteEquipmentItem()
    {
        item = InventoryData.Instance.GetEquipmentItemData(index);
        if (isSell)
        {
            InventoryData.Instance.GetGold(item.sellPrice);
        }
        item.ChangeAmount(-1);
        InventoryData.Instance.SetEquipmentSlot(index);
        Cancel();
    }
    void DeleteUseableItem()
    {
        item = InventoryData.Instance.GetUseableItemData(index);
        int value = field.GetValue();
        if (value == 0) return;
        if (item.curruntAmount >= value)
        {
            if (isSell)
            {
                InventoryData.Instance.GetGold(value * item.sellPrice);
            }
            item.ChangeAmount(-value);
            InventoryData.Instance.SetUseableSlot(index);
            Cancel();
        }
        else
        {
            FailUIOpen();
        }
    }
    void DeleteMiscItem()
    {
        item = InventoryData.Instance.GetMiscItemData(index);
        int value = field.GetValue();
        if (value == 0) return;
        if (item.curruntAmount >= value)
        {
            if(isSell)
            {
                InventoryData.Instance.GetGold(value * item.sellPrice);
            }
            item.ChangeAmount(-value);
            InventoryData.Instance.SetMiscSlot(index);
            Cancel();
        }
        else
        {
            FailUIOpen();
        }
    }
    void FailUIOpen()
    {
        deleteFailUI.SetText(isSell);
        gameObject.SetActive(false);
        deleteFailUI.gameObject.SetActive(true);
    }
}
