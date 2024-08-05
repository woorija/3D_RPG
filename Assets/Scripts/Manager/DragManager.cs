using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragManager : SingletonBehaviour<DragManager>
{
    int tempIndex;
    public bool isClick {  get; private set; }
    public DragUIType dragType { get; private set; }
    public ItemType dragItemType { get; private set; }

    [SerializeField] DragUI dragUI;
    [SerializeField] ItemSellEndDeleteUI itemSellEndDeleteUI;
    [SerializeField] PlayerStatus playerStatus; // 장비착용 관련 스테이터스를 체크하기위해 가져옴
    protected override void Awake()
    {
        DragReset();
    }
    public void DragStart(DragUIType _type, int _index, Sprite _sprite)
    {
        tempIndex = _index;
        dragType = _type;
        isClick = true;
        switch(dragType)
        {
            case DragUIType.Inventory:
                dragItemType = InventoryData.Instance.GetItemType();
                break;
            case DragUIType.Equipment:
                dragItemType = ItemType.Equipment;
                break;
            case DragUIType.Skill:
                break;
        }
        dragUI.SetImage(_sprite);
    }
    public void DragEndToEquipment(int _index)
    {
        switch (dragType)
        {
            case DragUIType.Inventory:
                if (InventoryData.Instance.IsEquipmentSlotEmpty()) return; //인벤토리에 빈칸이 없으면 생략
                EquipmentItem _tempItem = InventoryData.Instance.GetEquipmentItemData(_index);
                if (!IsEquipable(_tempItem.itemId, tempIndex)) return;
                InventoryData.Instance.GetEquipmentItem(EquipmentData.Instance.GetEquipmentItemData(tempIndex), _index);
                EquipmentData.Instance.SetEquipmentItem(_tempItem, tempIndex);
                break;
            case DragUIType.Equipment:
                //중복착용이 가능한 장비 교체시에만 사용될 예비함수
                break;
        }
        DragReset();
    }
    public void DragEndToInventory(int _index)
    {
        switch (dragType)
        {
            case DragUIType.Inventory:
                InventoryData.Instance.ChangeSlot(tempIndex, _index);
                break;
            case DragUIType.Equipment:
                if (InventoryData.Instance.IsEquipmentSlotEmpty()) return; //인벤토리에 빈칸이 없으면 생략
                EquipmentItem _tempItem = InventoryData.Instance.GetEquipmentItemData(_index);
                if (!IsEquipable(_tempItem.itemId, tempIndex)) return;
                InventoryData.Instance.GetEquipmentItem(EquipmentData.Instance.GetEquipmentItemData(tempIndex), _index);
                EquipmentData.Instance.SetEquipmentItem(_tempItem, tempIndex);
                break;
        }
        DragReset();
    }
    public void DragEndToQuickSlot(int _index)
    {
        switch (dragType)
        {
            case DragUIType.Inventory:
                int id = InventoryData.Instance.GetUseableItemData(tempIndex).itemId;
                QuickSlotData.Instance.SetSlot(_index, UseType.Item, id);
                break;
            case DragUIType.Skill:
                QuickSlotData.Instance.SetSlot(_index, UseType.Skill, tempIndex);
                break;
        }
        DragReset();
    }
    bool IsEquipable(int _requirements, int _index)
    {
        int requirements = _requirements / 1000 % 100000; // 조건에 맞게 5자릿수로 맞추기

        int reqGender = requirements / 10000;
        int reqClass = requirements / 1000 % 10;
        int reqRank = requirements / 100 % 10;
        int reqIndex = requirements % 100;
        Debug.Log(reqGender + "/" + reqClass + "/" + reqRank + "/" + reqIndex);
        if (reqGender != 0 && reqGender != playerStatus.gender) { Debug.Log("genderError"); return false; }
        if (reqClass != 0 && reqClass != playerStatus.playerClass) { Debug.Log("classError"); return false; }
        if (reqRank > playerStatus.classRank) { Debug.Log("rankError"); return false; }
        if (reqIndex != 0 && reqIndex != _index + 1) { Debug.Log("indexError"); return false; }
        return true;
    }

    public void DragReset()
    {
        isClick = false;
        tempIndex = -1;
        dragUI.ResetImage();
    }

    public void DeleteItem()
    {
        itemSellEndDeleteUI.gameObject.SetActive(true);
        itemSellEndDeleteUI.GetItem(dragItemType, tempIndex, false);
        DragReset();
    }
    public void SellItem()
    {
        itemSellEndDeleteUI.gameObject.SetActive(true);
        itemSellEndDeleteUI.GetItem(dragItemType, tempIndex, true);
        DragReset();
    }
}
