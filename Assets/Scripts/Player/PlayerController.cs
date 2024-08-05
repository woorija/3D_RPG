using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private StateMachine FSM;
    private Animator animator;
    private PlayerStatus status;
    private CharacterController playerController;
    private PlayerInput playerInput;

    private Vector2 inputVector;
    private Vector3 moveDirection;
    private Vector3 gravityDirection = Vector3.zero;

    public bool isRun { get; private set; } = false;
    private float moveSpeedMultiplier = 1f;
    public float MoveSpeedMultiplier
    {
        get
        {
            return moveSpeedMultiplier;
        }
        set
        {
            if (moveSpeedMultiplier != value)
            {
                moveSpeedMultiplier = value;
            }
        }
    }
    public int currentPlaySkillId { get; private set; }
    Action[] onUseQuickSlots;
    private void Awake()
    {
        FSM = GetComponent<StateMachine>();
        animator = GetComponentInChildren<Animator>();
        status = GetComponent<PlayerStatus>();
        playerController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        onUseQuickSlots = new Action[8];
    }
    private void Start()
    {
        FSMInit();
        InputInit();
        EventInit();
    }
    private void InputInit()
    {
        playerInput.actions["Movement"].performed += PerformedMovement;
        playerInput.actions["Movement"].canceled += CanceledMovement;
        playerInput.actions["RunModifier"].performed += PerformedRun;
        playerInput.actions["RunModifier"].canceled += CanceledRun;
        playerInput.actions["Jump"].performed += PerformedJump;
        playerInput.actions["Attack"].performed += PerformedAttack;
        for(int i=0;i< onUseQuickSlots.Length;i++)
        {
            int index = i;
            playerInput.actions[$"QuickSlot{i + 1}"].performed += ctx => PerformedUseQuickSlot(ctx, index);
        }
    }
    private void EventInit()
    {
        for (int i = 0; i < onUseQuickSlots.Length; i++)
        {
            int index = i;
            onUseQuickSlots[index] += () => QuickSlotData.Instance.Use(index);
        }
        QuickSlotData.Instance.SkillAddlistener(UseSkill);
        CustomSceneManager.Instance.playerTeleportEvent += Teleport;
        status.OnHit += ChangeHitState;
        status.OnDie += ChangeDieState;
    }
    private void Update()
    {
        if (!(GameManager.Instance.gameMode != GameMode.ControllMode))
        {
            FSM.StateUpdate();
            if(inputVector != Vector2.zero)
            {
                Move();
            }
        }
        ApplyGravity();
    }
    #region FSM
    private void FSMInit()
    {
        SetAllStates();
        FSM.Init(StateType.Idle);
    }
    private void SetAllStates()
    {
        BaseState temp;
        temp = GetComponent<Player_Idle>();
        FSM.SetState(StateType.Idle, temp);
        temp = GetComponent<Player_Walk>();
        FSM.SetState(StateType.Walk, temp);
        temp = GetComponent<Player_Jump>();
        FSM.SetState(StateType.Jump, temp);
        temp = GetComponent<Player_Fall>();
        FSM.SetState(StateType.Fall, temp);
        temp = GetComponent<Player_Land>();
        FSM.SetState(StateType.Land, temp);
        temp = GetComponent<Player_Attack>();
        FSM.SetState(StateType.Attack, temp);
        temp = GetComponent<Player_Buff>();
        FSM.SetState(StateType.Buff, temp);
        temp = GetComponent<Player_ActiveSkill>();
        FSM.SetState(StateType.ActiveSkill, temp);
        temp = GetComponent<Player_Hit>();
        FSM.SetState(StateType.Hit, temp);
        temp = GetComponent<Player_Die>();
        FSM.SetState(StateType.Die, temp);
    }
    #endregion
    #region InputSystem
    private void PerformedMovement(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
        if (FSM.CanChangeState(StateType.Walk))
        {
            FSM.ChangeState(StateType.Walk);
        }
        animator.SetBool("Move", true);
        animator.SetFloat("Dir_X", inputVector.x);
        animator.SetFloat("Dir_Y", inputVector.y);
    }
    private void CanceledMovement(InputAction.CallbackContext context)
    {
        inputVector = Vector2.zero;
        animator.SetBool("Move", false);
        if (FSM.GetCurrentStateType() == StateType.Walk)
        {
            FSM.ChangeState(StateType.Idle);
        }
    }
    private void PerformedRun(InputAction.CallbackContext context)
    {
        isRun = true;
        animator.SetBool("IsRun", isRun);
    }
    private void CanceledRun(InputAction.CallbackContext context)
    {
        isRun = false;
        animator.SetBool("IsRun", isRun);
    }
    private void PerformedJump(InputAction.CallbackContext context)
    {
        if (FSM.CanChangeState(StateType.Jump))
        {
            FSM.ChangeState(StateType.Jump);
        }
    }
    private void PerformedAttack(InputAction.CallbackContext context)
    {
        if (!animator.GetBool("IsPlayingSkill") && FSM.CanChangeState(StateType.Attack))
        {
            FSM.ChangeState(StateType.Attack);
        }
    }
    private void PerformedUseQuickSlot(InputAction.CallbackContext context,int _index)
    {
        onUseQuickSlots[_index].Invoke();
    }
    private void UseSkill(int _id)
    {
        if (CooltimeManager.Instance.IsCooltime(_id)) return;
            Debug.Log($"사용스킬ID:{_id}");
        if (BuffDataBase.skillBuffDB.ContainsKey(_id))
        {
            BuffManager.Instance.ApplyBuff(_id);
            UseBuffSkill();
        }
        else
        {
            UseActiveSkill(_id);
            Debug.Log("액티브");
        }
    }
    void UseActiveSkill(int _id)
    {
        currentPlaySkillId = _id;
        if (!animator.GetBool("IsPlayingSkill") && FSM.CanChangeState(StateType.ActiveSkill))
        {
            CooltimeManager.Instance.AddCooltime(_id, SkillDataBase.SkillDB[_id].coolTime);
            animator.SetInteger("SkillId", _id);
            FSM.ChangeState(StateType.ActiveSkill);
        }
    }
    void UseBuffSkill()
    {
        if (FSM.CanChangeState(StateType.Buff))
        {
            FSM.ChangeState(StateType.Buff);
        }
    }
    #endregion
    public void Jump()
    {
        gravityDirection.y = status.jumpForce;
    }
    private void ApplyGravity()
    {
        if (!playerController.isGrounded)
        {
            gravityDirection.y += status.gravity * Time.deltaTime;
        }
        playerController.Move(gravityDirection * Time.deltaTime);
    }
    public bool IsMove()
    {
        if (inputVector == Vector2.zero)
        {
            return false;
        }
        return true;
    }
    public bool IsFall()
    {
        if (!playerController.isGrounded && gravityDirection.y < 0f)
        {
            return true;
        }
        return false;
    }
    public bool IsGround()
    {
        if (playerController.isGrounded)
        {
            return true;
        }
        return false;
    }
    public void ChangeHitState()
    {
        if (FSM.CanChangeState(StateType.Hit))
        {
            FSM.ChangeState(StateType.Hit);
        }
    }
    public void ChangeDieState()
    {
        if (FSM.CanChangeState(StateType.Die))
        {
            FSM.ChangeState(StateType.Die);
        }
    }
    public void Rotate()
    {
        if (GameManager.Instance.gameMode != GameMode.ControllMode) return;
        transform.rotation = Quaternion.LookRotation(moveDirection);
    }
    public void RotateToWalk()
    {
        if (GameManager.Instance.gameMode != GameMode.ControllMode) return;
        CalculateMoveDirection();
        transform.rotation = Quaternion.LookRotation(moveDirection);
    }
    public void LookForward()
    {
        if (GameManager.Instance.gameMode != GameMode.ControllMode) return;
        CalculateMoveDirection();
        Vector3 lookVector = Camera.main.transform.forward;
        lookVector.y = 0f;
        transform.rotation = Quaternion.LookRotation(lookVector);
    }
    void CalculateMoveDirection()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;
        Quaternion cameraRotation = Quaternion.LookRotation(-cameraForward);

        moveDirection = cameraRotation * new Vector3(-inputVector.x, 0, -inputVector.y);
    }
    public void Move()
    {
        if (isRun)
        {
            playerController.Move(moveDirection * (status.moveSpeed + status.runSpeed) * moveSpeedMultiplier * Time.deltaTime);
        }
        else
        {
            playerController.Move(moveDirection * status.moveSpeed * moveSpeedMultiplier * Time.deltaTime);
        }
    }
    public void AnimationEnd()
    {
        animator.SetTrigger("AnimationEnd");
        FSM.ChangeState(StateType.Idle);
    }
    public void Teleport(Vector3 _pos)
    {
        playerController.Move(_pos - transform.position);
    }
}
