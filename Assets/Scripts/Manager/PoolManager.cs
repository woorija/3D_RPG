using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : SingletonBehaviour<PoolManager>
{
    int monsterHUDPoolCount = 16;
    int craftButtonPoolCount = 64;
    int shopSlotPoolCount = 32;
    int playerDamagePoolCount = 32;
    int monsterDamagePoolCount = 64;
    int buffPoolCount = 64;

    [Header("데미지")]
    [SerializeField] DamageText playerDamageTextPrefabs;
    [SerializeField] DamageText monsterDamageTextPrefabs;
    [Header("몬스터HUD")]
    [SerializeField] MonsterHUD monsterHUDPrefaps;
    [SerializeField] GameObject monsterHUDParent;
    [Header("합성UI")]
    [SerializeField] CraftSettingButton craftButtonPrefaps;
    [Header("상점UI")]
    [SerializeField] ShopSlot shopSlotPrefaps;

    public IObjectPool<DamageText> playerDamagePool { get; private set; }
    public IObjectPool<DamageText> monsterDamagePool { get; private set; }
    public IObjectPool<MonsterHUD> monsterHUDPool { get; private set; }
    public IObjectPool<CraftSettingButton> craftButtonPool { get; private set; }
    public IObjectPool<ShopSlot> shopSlotPool { get; private set; }
    public IObjectPool<Buff> buffPool { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        AllPoolInit();
    }
    void AllPoolInit()
    {
        monsterHUDPool = ObjectPoolInit(monsterHUDPrefaps, monsterHUDParent.transform, monsterHUDPoolCount);
        craftButtonPool = ObjectPoolInit(craftButtonPrefaps, craftButtonPoolCount);
        shopSlotPool = ObjectPoolInit(shopSlotPrefaps, shopSlotPoolCount);
        playerDamagePool = ObjectPoolInit(playerDamageTextPrefabs, transform, playerDamagePoolCount);
        monsterDamagePool = ObjectPoolInit(monsterDamageTextPrefabs, transform, monsterDamagePoolCount);
        buffPool = ClassPoolInit<Buff>(buffPoolCount);
    }
    IObjectPool<T> ClassPoolInit<T>(int _count) where T : ClassPool<T>, new()
    {
        IObjectPool<T> temp = null;
        temp = new ObjectPool<T>(
           () => OnCreateClass(temp),
           OnGetClass,
           OnReleaseClass,
           OnDestroyClass,
           defaultCapacity: _count,
           maxSize: _count
        );
        return temp;
    }
    IObjectPool<T> ObjectPoolInit<T>(T _prefaps, int _count) where T : PoolBehaviour<T>
    {
        IObjectPool<T> temp = null;
        temp = new ObjectPool<T>(
            () => OnCreateObject(_prefaps, temp),
            OnGetObject,
            OnReleaseObject,
            OnDestroyObject,
            defaultCapacity: _count,
            maxSize: _count
        );
        return temp;
    }
    IObjectPool<T> ObjectPoolInit<T>(T _prefaps, Transform _parent, int _count) where T : PoolBehaviour<T>
    {
        IObjectPool<T> temp = null;
        temp = new ObjectPool<T>(
            () => OnCreateObject(_prefaps, temp, _parent),
            OnGetObject,
            OnReleaseObject,
            OnDestroyObject,
            defaultCapacity: _count,
            maxSize: _count
        );
        return temp;
    }
    private T OnCreateClass<T>(IObjectPool<T> _pool) where T : ClassPool<T>, new()
    {
        T temp = new T();
        temp.SetPool(_pool);
        return temp;
    }
    private T OnCreateObject<T>(T _prefaps, IObjectPool<T> _pool) where T : PoolBehaviour<T>
    {
        T temp = Instantiate(_prefaps);
        temp.SetPool(_pool);
        return temp;
    }
    private T OnCreateObject<T>(T _prefaps, IObjectPool<T> _pool, Transform _parent) where T : PoolBehaviour<T>
    {
        T temp = Instantiate(_prefaps, _parent);
        temp.SetPool(_pool);
        return temp;
    }
    private void OnGetObject<T>(T _temp) where T : MonoBehaviour
    {
        _temp.gameObject.SetActive(true);
    }
    private void OnGetClass<T>(T _temp) where T : ClassPool<T>, new()
    {
        _temp.Init();
    }
    private void OnReleaseObject<T>(T _temp) where T : MonoBehaviour
    {
        _temp.gameObject.SetActive(false);
    }
    private void OnReleaseClass<T>(T _temp) where T : ClassPool<T>, new()
    {
        _temp.Reset();
    }
    private void OnDestroyObject<T>(T _temp) where T : MonoBehaviour
    {
        Destroy(_temp.gameObject);
    }
    private void OnDestroyClass<T>(T _temp) where T : ClassPool<T>, new()
    {
    }
}
