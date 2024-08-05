using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Walk : Player_Base
{
    public override void StateEnter()
    {
        base.StateEnter();
        animator.SetBool("IsWalk", true);
        controller.MoveSpeedMultiplier = 1.0f;
    }

    public override void StateUpdate()
    {
        if (!controller.IsMove())
        {
            FSM.ChangeState(StateType.Idle);
        }
        controller.RotateToWalk();
        base.StateUpdate();
    }
    public override void StateExit()
    {
        animator.SetBool("IsWalk", false);
        base.StateExit();
    }
}
