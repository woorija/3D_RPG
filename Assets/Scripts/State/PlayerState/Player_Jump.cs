using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Jump : Player_Base
{
    public override void StateEnter()
    {
        base.StateEnter();
        animator.SetTrigger("Jump");
        priority = 5;
        controller.MoveSpeedMultiplier = 0.6f;
        controller.Jump();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        if (controller.IsFall())
        {
            FSM.ChangeState(StateType.Fall);
        }
    }
    public override void StateExit()
    {
        priority = 2;
        animator.ResetTrigger("Jump");
        base.StateExit();
    }
}
