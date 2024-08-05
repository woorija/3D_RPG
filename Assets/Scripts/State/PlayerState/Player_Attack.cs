using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : Player_Base
{
    public override void StateEnter()
    {
        base.StateEnter();
        animator.SetTrigger("Attack");
        if (controller.IsMove())
        {
            controller.RotateToWalk();
        }
        else
        {
            controller.Rotate();
        }
        controller.MoveSpeedMultiplier = 0f;
        status.SetDamage(100);
    }
    public override void StateUpdate()
    {
        base.StateUpdate();
    }
    public override void StateExit()
    {
        base.StateExit();
    }
}
