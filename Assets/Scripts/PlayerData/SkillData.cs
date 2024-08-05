using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillData : SingletonBehaviour<SkillData>
{
    int skillPoint;
    public int SkillPoint
    {
        get
        {
            return skillPoint; 
        }
        private set
        {
            skillPoint = value;
            onSPChangedEvent?.Invoke(skillPoint); 
        } 
    }
    public Dictionary<int, int> acquiredSkillLevels { get; private set; } = new Dictionary<int, int>();
    public Dictionary<int, int> prevSkillLevels { get; private set; } = new Dictionary<int, int>(); // 선행스킬 레벨 감소 제한을 두기 위함
    public List<int> currentRankFilterSkills { get; private set; } = new List<int>();
    public Action<int> onSPChangedEvent;

    [SerializeField] SkillMainUI skillMainUI;
    public void GetLevelUpSP()
    {
        SkillPoint += 20;
    }
    public void GetRankUpSP()
    {
        SkillPoint += 100;
    }
    public void RankFilter(int _rank)
    {
        currentRankFilterSkills.Clear();
        foreach(var id in acquiredSkillLevels.Keys)
        {
            if (id / 10000 % 10 == _rank)
            {
                currentRankFilterSkills.Add(id);
            }
        }
    }

    public void ResetSkill(int _level, int _rank)
    {
        SkillPoint = (_level - 1) * 20 + _rank * 100;
        foreach(var key in acquiredSkillLevels.Keys)
        {
            acquiredSkillLevels[key] = 0;
        }
    }
    public void SetAcquiredSkills(int _class, int _rank)
    {
        int id = _class * 10 + _rank;
        foreach(var skill in SkillDataBase.SkillDB.Values)
        {
            if (id == skill.id / 10000)
            {
                if (!acquiredSkillLevels.ContainsKey(skill.id))
                {
                    acquiredSkillLevels.Add(skill.id, 0);
                }
            }
        }
    }
    public void IncreaseSkillLevel(int _id)
    {
        if (acquiredSkillLevels.ContainsKey(_id))
        {
            var skill = SkillDataBase.SkillDB[_id];
            if (acquiredSkillLevels[_id] < skill.masterLevel && skill.acquisitionSp <= SkillPoint)
            {
                SkillPoint -= skill.acquisitionSp;
                acquiredSkillLevels[_id]++;
                TryGetPassiveBuff(_id);
                if (acquiredSkillLevels[_id] == 1)
                {
                    SetPrevSkillLevel(skill.prevSkillId, skill.prevSkillLevel);
                }
                SkillInformationUI.Instance.SetSkillInfomations();
            }
        }
    }
    public void DecreaseSkillLevel(int _id)
    {
        if (acquiredSkillLevels.ContainsKey(_id))
        {
            if (acquiredSkillLevels[_id] >= 1)
            {
                var skill = SkillDataBase.SkillDB[_id];
                acquiredSkillLevels[_id]--;
                SkillPoint += skill.acquisitionSp;
                TryGetPassiveBuff(_id);
                if (acquiredSkillLevels[_id] == 0)
                {
                    RemovePrevSkillLevel(skill.prevSkillId);
                    QuickSlotData.Instance.ResetSlotToSkillLevelZero(_id);
                }
                SkillInformationUI.Instance.SetSkillInfomations();
            }
        }
    }

    public void SetPrevSkillLevel(int _id, int _level)
    {
        if (_id == 0) return;
        Debug.Log(_id + "/" + _level);
        prevSkillLevels.Add(_id, _level);
    }
    public void RemovePrevSkillLevel(int _id)
    {
        Debug.Log(_id);
        if (prevSkillLevels.ContainsKey(_id))
        {
            prevSkillLevels.Remove(_id);
        }
    }
    public int GetSkillMultiplier(int _id, int _index)
    {
        var skill = SkillDataBase.SkillDB[_id];
        int multiplier = skill.initialSkillMultiplier[_index] + skill.increaseSkillMultiplier[_index] * acquiredSkillLevels[_id];
        return multiplier;
    } 
    public void Init()
    {
        onSPChangedEvent += skillMainUI.SetSpText;
        SkillPoint = 1000;
        onSPChangedEvent.Invoke(SkillPoint);
        SetAcquiredSkills(1, 0);
    }
    public List<int> SaveSkillIds()
    {
        return new List<int>(acquiredSkillLevels.Keys);
    }
    public List<int> SaveSkillLevels()
    {
        return new List<int>(acquiredSkillLevels.Values);
    }
    public void LoadData(SaveDataSkill _data)
    {
        onSPChangedEvent += skillMainUI.SetSpText;
        SkillPoint = _data.skillPoint;
        onSPChangedEvent.Invoke(SkillPoint);
        for (int i = 0; i < _data.acquiredSkillIds.Count; i++)
        {
            acquiredSkillLevels.Add(_data.acquiredSkillIds[i], _data.acquiredSkillLevels[i]);
        }
        foreach(int id in acquiredSkillLevels.Keys)
        {
            TryGetPassiveBuff(id);
        }
    }
    private void OnDestroy()
    {
        onSPChangedEvent -= skillMainUI.SetSpText;
    }
    void TryGetPassiveBuff(int _id)
    {
        if(SkillDataBase.SkillDB[_id].skillType == 1)
        {
            BuffManager.Instance.ApplyPassiveSkillBuff(_id);
        }
    }
}
