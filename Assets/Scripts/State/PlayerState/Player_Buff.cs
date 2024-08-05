using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Buff : Player_Base
{
    public override void StateEnter()
    {
        base.StateEnter();
        animator.SetTrigger("Buff");
        if (controller.IsMove())
        {
            controller.RotateToWalk();
        }
        else
        {
            controller.Rotate();
        }
        controller.MoveSpeedMultiplier = 0f;
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