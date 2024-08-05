using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_ChangeIdlePosition : BT_ActionNode
{
    public override BTResult Execute()
    {
        BaseBlackBoard blackBoard = BT.GetBlackBoard();
        blackBoard.ChangeMovePoint();
        blackBoard.ReleaseHUD();
        return BTResult.Success;
    }
}
