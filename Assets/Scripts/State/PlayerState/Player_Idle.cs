using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Idle : Player_Base
{
    public override void StateEnter()
    {
        base.StateEnter();
        controller.MoveSpeedMultiplier = 0f;
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        if (controller.IsFall())
        {
            FSM.ChangeState(StateType.Fall);
        }
        else if (controller.IsMove())
        {
            FSM.ChangeState(StateType.Walk);
        }
    }
    public override void StateExit()
    {
        base.StateExit();
    }
}
