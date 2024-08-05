using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopSlot : PoolBehaviour<ShopSlot>, IPointerClickHandler
{
    [SerializeField] ShopSlotIcon icon;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text priceText;
    ItemBuyUI itemBuyUI;

    ItemBase item;
    int price;

    public void SetSlotData(int _id, int _price,ItemBuyUI _ui)
    {
        GetItemInfor(_id);
        price = _price;
        itemBuyUI = _ui;
        priceText.text = $"{price}G";
    }
    private void GetItemInfor(int _id)
    {
        switch (_id / 100000000)
        {
            case 1:
                item = ItemDataBase.EquipmentItemDB[_id];
                break;
            case 2:
                item = ItemDataBase.UseableItemDB[_id];
                break;
            case 3:
                item = ItemDataBase.MiscItemDB[_id];
                break;
        }
        nameText.text = item.name;
        icon.SetIcon(item);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (InventoryData.Instance.gold >= price)
        {
            itemBuyUI.SetItem(item, price);
        }
        else
        {
            itemBuyUI.FailUIOpen();
        }
    }
}
