using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_CheckReturn : BT_ActionNode
{
    public override BTResult Execute()
    {
        BaseBlackBoard blackBoard = BT.GetBlackBoard();
        if (blackBoard.isReturn)
        {
            blackBoard.ReleaseHUD();
            return BTResult.Success;
        }
        else
        {
            return BTResult.Failure;
        }
    }

}
