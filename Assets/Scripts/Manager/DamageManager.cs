using UnityEngine;

public class DamageManager : SingletonBehaviour<DamageManager>
{
    public void PopupPlayerDamage(int _damage, Vector3 _pos)
    {
        var temp = PoolManager.Instance.playerDamagePool.Get();
        temp.gameObject.SetActive(true);
        temp.SetPos(_pos);
        temp.SetText(_damage);
    }
    public void PopupMonsterDamage(int _damage, Vector3 _pos)
    {
        var temp = PoolManager.Instance.monsterDamagePool.Get();
        temp.gameObject.SetActive(true);
        temp.SetPos(_pos);
        temp.SetText(_damage);
    }
}
