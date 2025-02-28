using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;
public class QuickSlot
{
    public UseType useType { get; private set; } // 아이템 or 스킬
    public int id { get; private set; }
    public Action<int> OnUseSkill;
    public void Init()
    {
        useType = UseType.Null;
        id = 0;
    }
    public void SetSlot(UseType _type, int _id)
    {
        useType = _type;
        id = _id;
    }
    public void Use()
    {
        if (!IsAvailable()) return;
        switch (useType)
        {
            case UseType.Item:
                Debug.Log($"아이템{id}사용");
                if (InventoryData.Instance.GetItemCount(id) == 0) return; // 아이템이 없으면 사용 불가
                InventoryData.Instance.RemoveItem(id);
                BuffManager.Instance.ApplyBuff(id);
                break;
            case UseType.Skill:
                OnUseSkill(id);
                Debug.Log("스킬사용");
                break;
            case UseType.Null:
                Debug.Log("없음");
                break;
        }
    }
    public bool IsAvailable()
    {
        if (CooltimeManager.Instance.IsCooltime(id))
        {
            return false;
        }
        return true;
    }
    public void Reset()
    {
        useType = UseType.Null;
        id = 0;
    }
}
public class QuickSlotData : SingletonBehaviour<QuickSlotData>
{
    [SerializeField] QuickSlotUI[] quickSlotUIs;
    QuickSlot[] quickSlots;
    int[] quickSlotIds; // 중복저장 금지용
    protected override void Awake()
    {
        base.Awake();
        quickSlots = new QuickSlot[8];
        for (int i = 0; i < quickSlots.Length; i++)
        {
            quickSlots[i] = new QuickSlot();
            quickSlots[i].Init();
        }
        quickSlotIds = new int[quickSlots.Length];
    }
    private void Start()
    {
        foreach(QuickSlotUI slot in quickSlotUIs)
        {
            slot.Reset();
        }
    }
    private void Update()
    {
        SlotUpdate();
    }
    private void SlotUpdate()
    {
        for(int i = 0; i < quickSlots.Length; i++)
        {
            float fillAmount = CooltimeManager.Instance.GetCooltimeProgress(quickSlotIds[i]);
            quickSlotUIs[i].UpdateCooltimeBar(fillAmount);
            quickSlotUIs[i].UpdateCountText(quickSlots[i].useType, quickSlots[i].id);
        }
    }
    public void SetSlot(int _index, UseType _type, int _id)
    {
        if (!IsAvailable(_index)) return; // 쿨타임중인 슬롯은 변경 불가
        switch (_type)
        {
            case UseType.Item:
                SetSlotUseableItem(_index, _id);
                break;
            case UseType.Skill:
                SetSlotSkill(_index, _id);
                break;
            default:
                break;
        }
        quickSlotUIs[_index].SetSlot(_type, _id);
        quickSlotUIs[_index].UpdateCountText(_type, _id);
    }
    private void SetSlotUseableItem(int _index, int _id)
    {
        int tempIndex = Array.IndexOf(quickSlotIds, _id);
        if (Array.IndexOf(quickSlotIds,_id) != -1)
        { // 이미 해당 id의 소비템이 등록된 슬롯이 있으면 해당 슬롯 초기화
            quickSlotIds[tempIndex] = 0;
            ResetSlot(tempIndex);
        }
        quickSlotIds[_index] = _id;
        quickSlots[_index].SetSlot(UseType.Item, _id);
    }
    private void SetSlotSkill(int _index, int _id)
    {
        int tempIndex = Array.IndexOf(quickSlotIds, _id);
        if (Array.IndexOf(quickSlotIds, _id) != -1)
        { // 이미 해당 id의 스킬이 등록된 슬롯이 있으면 해당 슬롯 초기화
            quickSlotIds[tempIndex] = 0;
            ResetSlot(tempIndex);
        }
        quickSlotIds[_index] = _id;
        quickSlots[_index].SetSlot(UseType.Skill, _id);
    }
    public void ResetSlot(int _index)
    {
        quickSlots[_index].Reset();
        quickSlotUIs[_index].Reset();
    }
    public void ResetSlotToSkillLevelZero(int _id)
    {
        int index = -1;
        for(int i=0;i<quickSlotIds.Length;i++)
        {
            if(_id == quickSlotIds[i])
            {
                index = i; break;
            }
        }
        if (index == -1) return;
        ResetSlot(index);
    }
    public bool IsAvailable(int _index)
    {
        return quickSlots[_index].IsAvailable();
    }
    public void Use(int _index)
    {
        quickSlots[_index].Use();
    }
    public void SkillAddlistener(Action<int> _function)
    {
        foreach(QuickSlot slot in quickSlots)
        {
            slot.OnUseSkill += _function;
        }
    }
    public List<int> SaveQuickSlots()
    {
        return new List<int>(quickSlotIds);
    }
    public void LoadQuickSlots(SaveDataPlayer _data)
    {
        quickSlotIds = _data.quickSlotIds.ToArray();
        for (int i = 0; i < quickSlotIds.Length; i++)
        {
            UseType type = (int)Math.Log10(_data.quickSlotIds[i]) == 9 ? UseType.Item : UseType.Skill;
            SetSlot(i, type, quickSlotIds[i]);
        }
    }
}
