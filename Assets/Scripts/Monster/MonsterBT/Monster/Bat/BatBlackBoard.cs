using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBlackBoard : BaseBlackBoard
{
    public override void NormalAttack()
    {
        if (CustomUtility.IsInCircularSectorRange(transform.forward, transform.position, player.transform.position, blackBoardData.attackRange * blackBoardData.attackRange, 22.5f, 1.2f))
        {
            currentAttackCooltime = blackBoardData.attackCooltime;
            int damage = CustomUtility.CalculateMonsterDamage(blackBoardData.attackDamage, player.finalDefensePower);
            player.Hit(damage);
            DamageManager.Instance.PopupPlayerDamage(damage, player.transform.position);
        }
    }
}
