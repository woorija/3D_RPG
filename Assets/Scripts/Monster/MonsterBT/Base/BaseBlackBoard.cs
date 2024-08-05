using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public abstract class BaseBlackBoard : MonoBehaviour
{
    //모든 몬스터 블랙보드는 해당 클래스를 베이스로 상속받음
    [field: SerializeField] public NavMeshAgent agent { get; protected set; }

    [SerializeField] DropTableSO dropTable;
    [field: SerializeField] public MonsterBlackBoardSO blackBoardData { get; protected set; }
    public PlayerStatus player { get; protected set; }

    [SerializeField] Transform HUDTransform;
    [SerializeField] GameObject minimapMark;
    MonsterHUD monsterHUD;
    CapsuleCollider monsterCollider;

    [Tooltip("스폰 위치 랜덤성 부여")][field: SerializeField] public float randomSpawnRange { get; protected set; } //자율이동범위
    

    public bool isReturn { get; protected set; }
    public float currentIdleTime { get; protected set; }
    public float currentAttackCooltime { get; protected set; }
    public float currentRespawnTime { get; protected set; }

    int hp;
    public float staggerTime {  get; protected set; }
    int armorLevel;
    public bool isDie { get; protected set; }
    public Vector3 movePoint { get; protected set; } //자율이동 좌표
    public Vector3 spawnPointCenter { get; protected set; }

    [SerializeField] UnityEvent<int> HitAndDieEvent; // BT캔슬 함수 등록
    UnityEvent<int> OnHpChangedEvent; // hud changehp 연동
    protected void Awake()
    {
        player = FindObjectOfType<PlayerStatus>();
        OnHpChangedEvent = new UnityEvent<int>();
        monsterCollider = GetComponent<CapsuleCollider>();
        spawnPointCenter = transform.position;
        Init();
        ChangeMovePoint();
    }
    protected virtual void Update()
    {
        staggerTime -= Time.deltaTime;
        currentAttackCooltime -= Time.deltaTime;
        currentIdleTime -= Time.deltaTime;
        currentRespawnTime -= Time.deltaTime;
    }
    protected virtual void Init()
    {
        currentAttackCooltime = 0;
        currentIdleTime = 0;
        isDie = false;
        hp = blackBoardData.maxHp;
        monsterCollider.enabled = true;
        minimapMark.SetActive(true);
    }
    public void Hit(int _damage, int _playerLevel, int _armorBreakLevel, float _staggerTime)
    {
        if (isDie) return; // 죽은상태면 처리 안함
        if (_armorBreakLevel >= armorLevel) // 아머브레이크시
        {
            staggerTime = Mathf.Max(staggerTime, _staggerTime);
            HitAndDieEvent.Invoke(90);
        }
        int damage = CustomUtility.CalculatePlayerDamage(_damage, blackBoardData.level, _playerLevel);
        hp -= damage;
        OnHpChangedEvent.Invoke(hp);
        DamageManager.Instance.PopupMonsterDamage(damage, transform.position);
        IsDie();
    }
    void IsDie()
    {
        if (hp <= 0)
        {
            monsterCollider.enabled = false;
            minimapMark.SetActive(false);
            isDie = true;
            QuestManager.Instance.Hunt(blackBoardData.id);
            HitAndDieEvent.Invoke(99);
        }
    }
    public void ChangeArmorLevel(int _level)
    {
        armorLevel = _level;
    }

    public void ChangeMovePoint()
    {
        ChangeIdleTime();
        Vector2 randomInCircle = Random.insideUnitCircle;
        movePoint = new Vector3(randomInCircle.x * randomSpawnRange, transform.position.y, randomInCircle.y * randomSpawnRange) + spawnPointCenter;
    }
    public void ChangeReturn(bool _bool)
    {
        isReturn = _bool;
    }
    public void ResetAttackCooltime()
    {
        currentAttackCooltime = blackBoardData.attackCooltime;
    }
    public void ResetRespawnTime()
    {
        currentRespawnTime = blackBoardData.respawnTime;
    }
    void ChangeIdleTime()
    {
        currentIdleTime = Random.Range(blackBoardData.minIdleTime, blackBoardData.maxIdleTime);
    }
    public virtual void NormalAttack()
    {

    }
    public bool CheckDistance(float _sqrDistance)
    {
        return CustomUtility.CheckSqrDistance(transform.position, player.transform.position, _sqrDistance);
    }
    public bool CheckAngle(Vector3 _pos, float _angle)
    {
        return CustomUtility.CheckNormalAngle(_angle, transform.forward, _pos, player.transform.position);
    }
    public bool CheckHeightDifference(float _ypos)
    {
        return CustomUtility.CheckHeightDifference(_ypos, player.transform.position.y, blackBoardData.limitTrackingHeight);
    }
    public void Respawn()
    {
        Init();
    }

    public void DropItem()
    {
        player.Exp += dropTable.exp;
        InventoryData.Instance.GetGold(Random.Range(dropTable.minGold, dropTable.maxGold + 1));
        for(int i=0;i < dropTable.dropTables.Length;i++)
        {
            if(Random.Range(0f,100f) <= dropTable.dropTables[i].probability)
            {
                InventoryData.Instance.GetItem(dropTable.dropTables[i].itemNumber, Random.Range(dropTable.dropTables[i].minAmount, dropTable.dropTables[i].maxAmount + 1));
            }
        }
    }
    public void GetHUD()
    {
        if (monsterHUD == null && currentRespawnTime <= 0f)
        {
            monsterHUD = PoolManager.Instance.monsterHUDPool.Get();
            monsterHUD.SetTransform(HUDTransform);
            monsterHUD.SetSlider(hp, blackBoardData.maxHp);
            AddHUDListener(monsterHUD.ChangeHp);
        }
    }
    public void ReleaseHUD()
    {
        if (monsterHUD == null) return;

        RemoveAllListener();
        PoolManager.Instance.monsterHUDPool.Release(monsterHUD);
        monsterHUD = null;
    }
    public void AddHUDListener(UnityAction<int> _action)
    {
        OnHpChangedEvent.AddListener(_action);
    }
    public void RemoveAllListener()
    {
        OnHpChangedEvent.RemoveAllListeners();
    }
}
