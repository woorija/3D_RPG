using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Hit : Player_Base
{
    public override void Awake()
    {
        base.Awake();
        controller.MoveSpeedMultiplier = 0f;
    }
    public override void StateEnter()
    {
        base.StateEnter();
        animator.SetTrigger("IsHit");
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
