using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Base : BaseState
{
    protected PlayerStatus status;
    protected PlayerController controller;
    public override void Awake()
    {
        base.Awake();
        status = GetComponent<PlayerStatus>();
        controller = GetComponent<PlayerController>();
    }
    public override void StateEnter()
    {
        base.StateEnter();
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
