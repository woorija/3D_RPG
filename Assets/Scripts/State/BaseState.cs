using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState : MonoBehaviour
{
    [field: SerializeField] public int priority { get; protected set; }
    protected Animator animator;
    protected StateMachine FSM;
    public virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        FSM = GetComponent<StateMachine>();
    }
    public virtual void StateEnter()
    {
        //하위상태가 시작될 시 초기화 해줄 항목
    }

    public virtual void StateExit()
    {
        //하위상태가 종료될 시 초기화 해줄 항목
    }

    public virtual void StateUpdate()
    {
        //하위상태에서 업데이트될 항목
    }
}
