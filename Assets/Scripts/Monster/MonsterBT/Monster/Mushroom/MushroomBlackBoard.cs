using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomBlackBoard : BaseBlackBoard
{
    public float currentDashSkillCooltime { get; protected set; }
    public float currentJumpSkillCooltime { get; protected set; }

    protected override void Init()
    {
        base.Init();
        currentDashSkillCooltime = 0;
        currentJumpSkillCooltime = 0;
    }
    protected override void Update()
    {
        base.Update();
        currentDashSkillCooltime -= Time.deltaTime;
        currentJumpSkillCooltime -= Time.deltaTime;
    }
    public override void NormalAttack()
    {
        if (CustomUtility.IsInCircularSectorRange(transform.forward, transform.position, player.transform.position, blackBoardData.attackRange * blackBoardData.attackRange, 22.5f, 2f))
        {
            int damage = CustomUtility.CalculateMonsterDamage(blackBoardData.attackDamage, player.finalDefensePower);
            player.Hit(damage);
            DamageManager.Instance.PopupPlayerDamage(damage, player.transform.position);
        }
    }
    public void ResetDashSkillCooltime()
    {
        MushroomBlackBoardSO MushroomBlackBoardData = (MushroomBlackBoardSO)blackBoardData;
        currentDashSkillCooltime = MushroomBlackBoardData.dashskillCooltime;
    }
    public void ResetJumpSkillCooltime()
    {
        MushroomBlackBoardSO MushroomBlackBoardData = (MushroomBlackBoardSO)blackBoardData;
        currentJumpSkillCooltime = MushroomBlackBoardData.jumpSkillCooltime;
    }
    public void DashSkillAttack()
    {
        MushroomBlackBoardSO MushroomBlackBoardData = (MushroomBlackBoardSO)blackBoardData;
        if (CustomUtility.IsInBoxRangeToPlayer(transform.position + (transform.forward * MushroomBlackBoardData.dashskillRange.z), MushroomBlackBoardData.dashskillRange, transform.localRotation))
        {
            int damage = CustomUtility.CalculateMonsterDamage(MushroomBlackBoardData.dashskillDamage, player.finalDefensePower);
            player.Hit(damage);
            DamageManager.Instance.PopupPlayerDamage(damage, player.transform.position);
        }


    }
    public void JumpSkillAttack()
    {
        MushroomBlackBoardSO MushroomBlackBoardData = (MushroomBlackBoardSO)blackBoardData;
        if (CheckDistance(MushroomBlackBoardData.jumpSkillInnerRange))
        {
            int damage = CustomUtility.CalculateMonsterDamage(MushroomBlackBoardData.jumpSkillInnerDamage, player.finalDefensePower);
            player.Hit(damage);
            DamageManager.Instance.PopupPlayerDamage(damage, player.transform.position);
        }
        else if (CheckDistance(MushroomBlackBoardData.jumpSkillOuterRange))
        {
            int damage = CustomUtility.CalculateMonsterDamage(MushroomBlackBoardData.jumpSkillOuterDamage, player.finalDefensePower);
            player.Hit(damage);
            DamageManager.Instance.PopupPlayerDamage(damage, player.transform.position);
        }
    }
}
