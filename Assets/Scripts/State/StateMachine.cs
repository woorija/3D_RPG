using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] BaseState currentState;
    Dictionary<StateType, BaseState> States;
    public StateType currentStateType;
    private void Awake()
    {
        States = new Dictionary<StateType, BaseState>();
    }
    public void Init(StateType _type)
    {
        if (States.ContainsKey(_type))
        {
            currentState = States[_type];
            currentStateType = _type;
        }
    }
    public void ChangeState(StateType _newState)
    {
        if (States.ContainsKey(_newState) && currentState != States[_newState])
        {
            currentState.StateExit();
            currentState = States[_newState];
            currentStateType = _newState;
            currentState.StateEnter();
        }
    }
    public bool CanChangeState(StateType _newState)
    {
        if (currentState.priority > States[_newState].priority)
        {
            return false;
        }
        return true;
    }
    public void SetState(StateType _type, BaseState _state)
    {
        States.Add(_type, _state);
    }
    public void StateUpdate()
    {
        currentState.StateUpdate();
    }
    public StateType GetCurrentStateType()
    {
        return currentStateType;
    }
}
