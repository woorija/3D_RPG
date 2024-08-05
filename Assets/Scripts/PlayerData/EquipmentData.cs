using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentData : SingletonBehaviour<EquipmentData>
{
    /*----------UI정보값------------*/
    [SerializeField] EquipmentUI equipmentUI;
    List<EquipmentItem> equipmentItemList;

    /*--------보조 정보값-------------*/
    int equipmentCount = System.Enum.GetValues(typeof(EquipmentType)).Length;

    /*----------스텟정보값------------*/
    [SerializeField] PlayerStatus _status;
    protected override void Awake()
    {
        base.Awake();
        equipmentItemList = new List<EquipmentItem>(equipmentCount);
        for (int i = 0; i < equipmentCount; i++)
        {
            equipmentItemList.Add(new EquipmentItem());
        }
    }
    public void Init()
    {
        for(int i = 0; i < equipmentCount; i++)
        {
            equipmentItemList[i].Reset();
        }
    }
    public EquipmentItem GetEquipmentItemData(EquipmentType _type)
    {
        return equipmentItemList[(int)_type];
    }
    public EquipmentItem GetEquipmentItemData(int _type)
    {
        return equipmentItemList[_type];
    }
    public void SetEquipmentItem(EquipmentItem _item, int _index)
    {
        int tempAttackPower = -equipmentItemList[_index].attackPower;
        int tempDefencePower = -equipmentItemList[_index].defensePower;
        int tempMaxHp = -equipmentItemList[_index].hp;
        int tempMaxMp = -equipmentItemList[_index].mp;

        equipmentItemList[_index] = _item;

        tempAttackPower += equipmentItemList[_index].attackPower;
        tempDefencePower += equipmentItemList[_index].defensePower;
        tempMaxHp += equipmentItemList[_index].hp;
        tempMaxMp += equipmentItemList[_index].mp;

        _status.GetBonusStatus(StatusType.AttackPoint, tempAttackPower);
        _status.GetBonusStatus(StatusType.DefensePoint, tempDefencePower);
        _status.GetBonusStatus(StatusType.Hp, tempMaxHp);
        _status.GetBonusStatus(StatusType.Mp, tempMaxMp);

        equipmentUI.SetSlot(_index);
    }
    public int GetEquipmentStatus(StatusType _type)
    {
        return CalcEquipmentStatus(_type);
    }
    private int CalcEquipmentStatus(StatusType _type)
    {
        int tempStatus = 0;
        for(int i = 0; i < equipmentCount; i++)
        {
            switch (_type)
            {
                case StatusType.AttackPoint:
                    tempStatus += equipmentItemList[i].attackPower;
                    break;
                case StatusType.DefensePoint:
                    tempStatus += equipmentItemList[i].defensePower;
                    break;
                case StatusType.Hp:
                    tempStatus += equipmentItemList[i].hp;
                    break;
                case StatusType.Mp:
                    tempStatus += equipmentItemList[i].mp;
                    break;
            }
        }
        return tempStatus;
    }
    public List<int> SaveEquipmentItems()
    {
        List<int> ids = new List<int>();
        for(int i = 0;i < equipmentCount;i++)
        {
            ids.Add(equipmentItemList[i].itemId);
        }
        return ids;
    }
    public void LoadEquipmentItems(SaveDataPlayer _data)
    {
        for (int i = 0; i < equipmentCount; i++)
        {
            equipmentItemList[i] = ItemDataBase.EquipmentItemDB[_data.equipmentIds[i]];
        }
    }
}
