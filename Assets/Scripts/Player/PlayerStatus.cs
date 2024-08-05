using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStatus : MonoBehaviour
{
    /*
     *  기본스텟 = 1레벨 기본 스텟 + 레벨당 추가 스텟
     *  추가스텟 = 기본스텟을 제외한 나머지 모든 스텟
     *  최종스텟 = 기본스텟 + 추가스텟
     */
    #region 딕셔너리 키
    string attackPoint = "AttackPoint";
    string defensePoint = "DefensePoint";
    string maxHp = "MaxHp";
    string maxMp = "MaxMp";
    #endregion
    #region 캐릭터정보
    public int gender  = 1; // 1남 2여
    public int playerClass  = 1; // 나중에 세팅
    public int classRank  = 0; // 0~9차
    public Action onClassRankUpEvent;
    #endregion
    #region 기본스텟
    int level = 1;
    public int Level
    {
        get => level;
        set
        {
            level = value;
            onStatusChangeEvent.Invoke(StatusType.Level);
        }
    }
    int baseMaxHp;
    public int BaseMaxHp
    {
        get => baseMaxHp;
        set
        {
            baseMaxHp = value;
            CalculateFinalStatus(StatusType.Hp);
        }
    }
    int baseMaxMp;
    public int BaseMaxMp
    {
        get => baseMaxMp;
        set
        {
            baseMaxMp = value;
            CalculateFinalStatus(StatusType.Mp);
        }
    }
    int baseAttackPower;
    public int BaseAttackPower
    {
        get => baseAttackPower;
        set
        {
            baseAttackPower = value;
            CalculateFinalStatus(StatusType.AttackPoint);
        }
    }
    int baseDefensePower;
    public int BaseDefensePower
    {
        get => baseDefensePower;
        set
        {
            baseDefensePower = value;
            CalculateFinalStatus(StatusType.DefensePoint);
        }
    }
    #endregion
    #region 기본변동스텟
    int hp;
    public int Hp 
    { 
        get 
        { 
            return hp; 
        }
        set
        {
            hp = Mathf.Clamp(value, 0, finalMaxHp);
            if(hp == 0)
            {
                isDie = true;
                OnDie.Invoke();
            }
            hpChangeEvent.Invoke();
            onStatusChangeEvent.Invoke(StatusType.Hp);
        }
    }
    int mp;
    public int Mp
    {
        get
        {
            return mp;
        }
        set
        {
            mp = Mathf.Clamp(value, 0, finalMaxMp);
            mpChangeEvent.Invoke();
            onStatusChangeEvent.Invoke(StatusType.Mp);
        }
    }
    int exp;
    public int Exp
    {
        get
        {
            return exp;
        }
        set
        {
            exp = value;
            if(exp >= MaxExp)
            {
                LevelUp();
                exp -= MaxExp;
                //MaxExp = 다음필요경험치
            }
            expChangeEvent.Invoke();
        }
    }
    public int damage { get; private set; }
    public void SetDamage(int _multiplier)
    {
        damage = finalAttackPower * _multiplier / 100;
    }
    public int ArmorBreakLevel { get; set; }
    public UnityEvent hpChangeEvent;
    public UnityEvent mpChangeEvent;
    public UnityEvent expChangeEvent;
    #endregion
    #region 변동스텟
    public Dictionary<string,int> bonusStats = new Dictionary<string,int>();
    UnityEvent<StatusType> onStatusChangeEvent = new UnityEvent<StatusType>();
    private void Awake()
    {
        bonusStats.Add(attackPoint, 0);
        bonusStats.Add(defensePoint, 0);
        bonusStats.Add(maxHp, 0);
        bonusStats.Add(maxMp, 0);
    }
    public void AddStatusEvent(UnityAction<StatusType> _event)
    {
        onStatusChangeEvent.AddListener(_event);
    }
    public void GetAllEquipmentStatus()
    {
        bonusStats[attackPoint] += EquipmentData.Instance.GetEquipmentStatus(StatusType.AttackPoint);
        bonusStats[defensePoint] += EquipmentData.Instance.GetEquipmentStatus(StatusType.DefensePoint);
        bonusStats[maxHp] += EquipmentData.Instance.GetEquipmentStatus(StatusType.Hp);
        bonusStats[maxMp] += EquipmentData.Instance.GetEquipmentStatus(StatusType.Mp);
    }
    public void GetBonusStatus(StatusType _type, int _value)
    {
        switch (_type)
        {
            case StatusType.AttackPoint:
                bonusStats[attackPoint] += _value;
                break;
            case StatusType.DefensePoint:
                bonusStats[defensePoint] += _value;
                break;
            case StatusType.Hp:
                bonusStats[maxHp] += _value;
                break;
            case StatusType.Mp:
                bonusStats[maxMp] += _value;
                break;
        }
        CalculateFinalStatus(_type);
    }
    #endregion
    #region 최종스텟
    public int finalAttackPower { get; set; }
    public int finalDefensePower { get; set; }
    int finalMaxHp;
    public int FinalMaxHp
    {
        get
        {
            return finalMaxHp;
        }
        set
        {
            finalMaxHp = value;
            maxHpChangeEvent.Invoke();
            onStatusChangeEvent.Invoke(StatusType.Hp);
        }
    }
    int finalMaxMp;
    public int FinalMaxMp
    {
        get
        {
            return finalMaxMp;
        }
        set
        {
            finalMaxMp = value;
            maxMpChangeEvent.Invoke();
            onStatusChangeEvent.Invoke(StatusType.Mp);
        }
    }
    int maxExp = 1;
    public int MaxExp
    {
        get
        {
            return maxExp;
        }
        set
        {
            maxExp = value;
            maxExpChangeEvent.Invoke();
        }
    }
    public UnityEvent maxHpChangeEvent;
    public UnityEvent maxMpChangeEvent;
    public UnityEvent maxExpChangeEvent;
    void CalculateFinalStatus(StatusType _type)
    {
        switch (_type)
        {
            case StatusType.AttackPoint:
                finalAttackPower = BaseAttackPower + bonusStats[attackPoint];
                break;
            case StatusType.DefensePoint:
                finalDefensePower = BaseDefensePower + bonusStats[defensePoint];
                break;
            case StatusType.Hp:
                FinalMaxHp = BaseMaxHp + bonusStats[maxHp];
                break;
            case StatusType.Mp:
                FinalMaxMp = BaseMaxMp + bonusStats[maxMp];
                break;
        }
        onStatusChangeEvent.Invoke(_type);
    }
    #endregion
    #region 상태스텟
    public Action OnDie;
    public bool isDie { get; set; } = false;
    public bool isDash { get; set; } = false;
    public bool isInvincible { get; set; } = false;
    public bool isSuperArmor { get; set; } = false;
    public float gravity { get; set; } = -9.81f;
    #endregion
    #region 이동스텟
    public float moveSpeed { get; set; } = 3f;

    public float runSpeed { get; set; } = 2f;

    public float jumpForce { get; set; } = 4.5f;
    #endregion
    public void Init()
    {
        gender = 1;
        playerClass = 0;
        classRank = 0;
        level = 1;
        BaseAttackPower = 5;
        BaseDefensePower = 5;
        BaseMaxHp = 100;
        BaseMaxMp = 100;
        Hp = 100;
        Mp = 100;
        MaxExp = 10;
        Exp = 3;

        ArmorBreakLevel = 1;
    }
    public Action OnHit;
    public void Hit(int _damage)
    {
        Hp -= _damage;
        if(!isSuperArmor && !isDie)
        {
            OnHit.Invoke();
        }
    }
    void LevelUp()
    {
        level++;
        BaseAttackPower += 2;
        BaseDefensePower += 2;
        BaseMaxHp += 30;
        Hp = FinalMaxHp;
        BaseMaxMp += 30;
        Mp = FinalMaxMp;
        SkillData.Instance.GetLevelUpSP();
        QuestManager.Instance.SetStartableQuest();
    }
    public void ClassChange(int _class, int _rank)
    {
        playerClass = _class;
        classRank = _rank;
        SkillData.Instance.GetRankUpSP();
        SkillData.Instance.SetAcquiredSkills(playerClass, classRank);
        onClassRankUpEvent.Invoke();
    }
    public void ClassRankUp(int _rank)
    {
        classRank = _rank;
        SkillData.Instance.GetRankUpSP();
        SkillData.Instance.SetAcquiredSkills(playerClass, classRank);
        onClassRankUpEvent.Invoke();
    }
    #region 저장,불러오기
    public void LoadData(SaveDataPlayer _data)
    {
        Init();
        gender = _data.gender;
        playerClass = _data.playerClass;
        classRank = _data.classRank;
        level = _data.level;
        SetBaseStatus();
        GetAllEquipmentStatus();
        Hp = _data.currentHp;
        Mp = _data.currentMp;
        Exp = _data.currentExp;
    }
    void SetBaseStatus()
    {
        BaseAttackPower = level * 2 + 3;
        BaseDefensePower = level * 2 + 3;
        BaseMaxHp = level * 30 + 100;
        BaseMaxMp = level * 30 + 100;
    }
    #endregion
}
