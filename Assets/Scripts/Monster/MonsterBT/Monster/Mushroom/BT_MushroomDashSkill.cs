using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_MushroomDashSkill : BT_ActionNode
{
    public override BTResult Execute()
    {
        if (BT.IsAnimationEnd("DashSkill"))
        {
            BT.ChangeAnimatorTrigger("AnimationEnd");
            BT.CheckDeleteRunningNode(1);
            return BTResult.Success;
        }
        BT.ChangeAnimatorTrigger("DashSkill");
        BT.GetRunningNode(this);
        return BTResult.Running;
    }
}
