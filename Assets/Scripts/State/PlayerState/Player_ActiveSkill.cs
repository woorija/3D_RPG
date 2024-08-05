using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ActiveSkill : Player_Base
{
    public override void StateEnter()
    {
        base.StateEnter();
        switch (controller.currentPlaySkillId)
        {
            case 100001:
                SetRotate();
                SetMoveSpeed(0);
                break;
        }
    }
    public override void StateUpdate()
    {
        base.StateUpdate();
    }
    public override void StateExit()
    {
        animator.SetInteger("SkillId", 0);
        base.StateExit();
    }
    private void SetMoveSpeed(float _speed)
    {
        controller.MoveSpeedMultiplier = _speed;
    }
    private void SetRotate()
    {
        if (controller.IsMove())
        {
            controller.RotateToWalk();
        }
        else
        {
            controller.Rotate();
        }
    }
}
