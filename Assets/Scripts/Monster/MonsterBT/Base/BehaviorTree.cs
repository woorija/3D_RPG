using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree : MonoBehaviour
{
    //각 몬스터의 BT는 해당 클래스를 베이스로 상속받음
    [SerializeField] protected BT_Node RootNode;
    [SerializeField] protected Animator _animator; // 몬스터의 애니메이터

    [SerializeField] protected BT_Node RunningNode; // 러닝상태인 액션노드가 있을때 루트노드 대신 동작할 노드
    [SerializeField] protected BaseBlackBoard blackBoard;
    //블랙보드는 각 몬스터 BT내에서 선언할것
    public virtual BaseBlackBoard GetBlackBoard() { return blackBoard; }     // 블랙보드를 가져오는 함수
    public virtual void GetRootNode(BT_Node _node)
    {
        RootNode = _node;
    }
    public virtual void GetRunningNode(BT_Node _node) // 러닝상태의 노드를 가져오는 함수
    {
        if (RunningNode == _node) return;
        if (RunningNode != null)
        {
            if (RunningNode.Priority >= _node.Priority) return;
        }
        RunningNode = _node;
    }
    public virtual void CheckDeleteRunningNode(int _priority) // 러닝상태의 노드를 제거하는 함수
    {
        if (RunningNode != null)
        {
            if (_priority > RunningNode.Priority)
            {
                RunningNode = null;
            }
        }
    }
    public void ChangeAnimatorBool(string _path, bool _value)
    {
        if (_animator.GetBool(_path) == _value) return;
        _animator.SetBool(_path, _value);
    }
    public void ChangeAnimatorInt(string _path, int _value)
    {
        if (_animator.GetInteger(_path) == _value) return;
        _animator.SetInteger(_path, _value);
    }
    public void ChangeAnimatorTrigger(string _path)
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName(_path)) return;
        _animator.SetTrigger(_path);
    }
    public bool IsAnimationEnd(string _path)
    {
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName(_path)) return false;
        return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f;
    }
    public bool IsCurrentAnimatorStateName(string _path)
    {
        return _path.Equals(_animator.GetCurrentAnimatorStateInfo(0).IsName(_path));
    }
    public float GetCurrentAnimationTime(string _path)
    {
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName(_path)) return -1f;
        return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
    public void PauseAnimation()
    {
        _animator.speed = 0f;
    }
    public void PlayAnimation()
    {
        _animator.speed = 1f;
    }
    public void ReplayAnimation()
    {
        PlayAnimation();
        _animator.Play(_animator.GetCurrentAnimatorStateInfo(0).shortNameHash, 0, 0);
    }
    public virtual void MeshSetActiveFalse()
    {
    }
    public virtual void MeshSetActiveTrue()
    {
    }
}
