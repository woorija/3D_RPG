using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitUtility : MonoBehaviour
{
    [SerializeField] PlayerStatus status;
    [SerializeField] PlayerController controller;

    private Collider[] monsterColliders = new Collider[10];
    public void SetSkillDamage(int _attackIndex)
    {
        int multiplier = SkillData.Instance.GetSkillMultiplier(controller.currentPlaySkillId, _attackIndex);
        status.SetDamage(multiplier);
    }
    public void CircularSectorHit(Vector3 _centerPos, Vector3 _forward, float _radius, float _halfSectorAngle, float _yposLimit, int _hitCount, float _staggerTime)
    {
        int count = CustomUtility.InCircularSectorRangeToMonsterCount(_centerPos, _forward, _radius, _halfSectorAngle, ref monsterColliders, _yposLimit);
        int maxCount = count > _hitCount ? _hitCount : count;
        for (int i = 0; i < maxCount; i++)
        {
            BaseBlackBoard blackBoard = monsterColliders[i].GetComponent<BaseBlackBoard>();
            blackBoard.Hit(status.damage, status.Level, status.ArmorBreakLevel, _staggerTime);
            blackBoard.GetHUD();
        }
    }
    public void CircularHit(Vector3 _centerPos, float _radius, float _yposLimit, int _hitCount, float _staggerTime)
    {
        int count = CustomUtility.InCircleRangeToMonsterCount(_centerPos, _radius, ref monsterColliders, _yposLimit);
        int maxCount = count > _hitCount ? _hitCount : count;
        for (int i = 0; i < maxCount; i++)
        {
            BaseBlackBoard blackBoard = monsterColliders[i].GetComponent<BaseBlackBoard>();
            blackBoard.Hit(status.damage, status.Level, status.ArmorBreakLevel, _staggerTime);
            blackBoard.GetHUD();
        }
    }
    public void BoxHit(Vector3 _centerPos, Vector3 _half, Quaternion _rotate, int _hitCount, float _staggerTime)
    {
        int count = CustomUtility.InBoxRangeToMonsterCount(_centerPos, _half, _rotate, ref monsterColliders);
        int maxCount = count > _hitCount ? _hitCount : count;
        for (int i = 0; i < maxCount; i++)
        {
            BaseBlackBoard blackBoard = monsterColliders[i].GetComponent<BaseBlackBoard>();
            blackBoard.Hit(status.damage, status.Level, status.ArmorBreakLevel, _staggerTime);
            blackBoard.GetHUD();
        }
    }
}
