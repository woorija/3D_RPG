using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class CustomUtility
{
    static Collider[] playerCollider = new Collider[1];
    public static int GetDigitCount(int _num)
    {
        if (_num == 0) return 1;
        if (_num < 0)
        {
            _num = -_num;
        }
        return (int)Mathf.Log10(_num) + 1;
    }
    public static bool CheckSqrDistance(Vector3 _pos1, Vector3 _pos2, float _sqrDistance)
    {
        Vector3 pos1 = _pos1;
        Vector3 pos2 = _pos2;
        pos1.y = 0;
        pos2.y = 0;
        return (pos1 - pos2).sqrMagnitude < _sqrDistance;
    }
    public static bool CheckHeightDifference(float _ypos1, float _ypos2, float _limit)
    {
        return Mathf.Abs(_ypos1 - _ypos2) <= _limit;
    }
    public static float GetAngle(Vector3 _forward, Vector3 _pos1, Vector3 _pos2) // 기준: 정면 0도 후방 180도
    {
        Vector3 dir = _pos2 - _pos1;
        return Vector3.SignedAngle(new Vector3(_forward.x, 0, _forward.z), new Vector3(dir.x, 0, dir.z), Vector3.up);
    }
    public static bool CheckNormalAngle(float _angle, Vector3 _forward, Vector3 _pos1, Vector3 _pos2)
    {
        return Mathf.Abs(GetAngle(_forward, _pos1, _pos2)) <= _angle;
    }
    public static bool CheckAngle(float _angle1, float _angle2, Vector3 _forward, Vector3 _pos1, Vector3 _pos2)
    {
        float angle = GetAngle(_forward, _pos1, _pos2);
        return _angle1 <= angle && angle <= _angle2;
    }

    public static float CalculateLevelBonusDamage(int _monsterlvl, int _playerlvl)
    {
        int levelDifference = _monsterlvl - _playerlvl;
        float temp = levelDifference switch
        {
            >= 0 => 1f - (levelDifference * 0.05f),
            -1 => 1.04f,
            -2 => 1.08f,
            -3 => 1.12f,
            -4 => 1.16f,
            <= -5 => 1.2f,
        };
        return temp;
    }

    public static int CalculateMonsterDamage(int _damage, int _playerDef)
    {
        float temp = _damage * (1f - (_playerDef / (_playerDef + 300f)));
        return (int)temp;
    }
    public static int CalculatePlayerDamage(int _damage, int _monsterLevel, int _playerLevel)
    {
        float levelBonus = CalculateLevelBonusDamage(_monsterLevel, _playerLevel);
        int temp = (int)(_damage * levelBonus);
        return temp;
    }
    /* 부채꼴범위
     * 플레이어용 = Angle
     * 몬스터용 = Range
     */
    public static bool IsInCircularSectorAngle(Vector3 _forward, Vector3 _controlPos, Vector3 _otherPos, float _halfSector, float _yposLimit)
    { 
        if (!CheckHeightDifference(_controlPos.y, _otherPos.y, _yposLimit)) return false; // ypos차이
        if (!CheckNormalAngle(_halfSector, _forward, _controlPos, _otherPos)) return false;  // 각도
        return true;
    }
    public static bool IsInCircularSectorRange(Vector3 _forward, Vector3 _controlPos, Vector3 _otherPos, float _sqrDistance, float _halfSector, float _yposLimit)
    {
        if (!CheckHeightDifference(_controlPos.y, _otherPos.y, _yposLimit)) return false; // ypos차이
        if (!CheckSqrDistance(_controlPos, _otherPos, _sqrDistance)) return false;  // 거리
        if (!CheckNormalAngle(_halfSector, _forward, _controlPos, _otherPos)) return false;  // 각도
        return true;
    }
    public static bool IsInCircleRange(Vector3 _controlPos, Vector3 _otherPos, float _sqrDistance, float _yposLimit)
    {
        if (!CheckHeightDifference(_controlPos.y, _otherPos.y, _yposLimit)) return false;
        if (!CheckSqrDistance(_controlPos, _otherPos, _sqrDistance)) return false;
        return true;
    }
    public static bool IsInBoxRangeToPlayer(Vector3 _centerPos, Vector3 _half, Quaternion _rotate)
    {
        int isInPlayer = Physics.OverlapBoxNonAlloc(_centerPos, _half, playerCollider, _rotate, LayerMask.GetMask("Player"));
        if (isInPlayer == 0) return false;
        return true;
    }
    public static int InBoxRangeToMonsterCount(Vector3 _centerPos, Vector3 _half, Quaternion _rotate, ref Collider[] colliders)
    {
        int isInMonster = Physics.OverlapBoxNonAlloc(_centerPos, _half, colliders, _rotate, LayerMask.GetMask("Monster"));
        return isInMonster;
    }
    public static int InCircleRangeToMonsterCount(Vector3 _centerPos, float _radius, ref Collider[] colliders, float _yposLimit)
    {
        int totalColliders = Physics.OverlapSphereNonAlloc(_centerPos, _radius, colliders, LayerMask.GetMask("Monster"));
        int count = 0;

        for (int i = 0; i < totalColliders; i++)
        {
            Vector3 otherPos = colliders[i].transform.position;
            if (CheckHeightDifference(_centerPos.y, otherPos.y, _yposLimit))
            {
                colliders[count++] = colliders[i];
            }
        }

        return count;
    }
    public static int InCircularSectorRangeToMonsterCount(Vector3 _centerPos, Vector3 _forward, float _radius, float _halfSectorAngle, ref Collider[] colliders, float _yposLimit)
    {
        int totalColliders = Physics.OverlapSphereNonAlloc(_centerPos, _radius, colliders, LayerMask.GetMask("Monster"));
        int count = 0;

        for (int i = 0; i < totalColliders; i++)
        {
            Vector3 otherPos = colliders[i].transform.position;
            if (IsInCircularSectorAngle(_forward, _centerPos, otherPos, _halfSectorAngle, _yposLimit))
            {
                colliders[count++] = colliders[i];
            }
        }

        return count;
    }
}
