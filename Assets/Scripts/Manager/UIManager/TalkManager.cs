using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TalkManager : SingletonBehaviour<TalkManager>
{
    [SerializeField] TalkUI talkUi;
    [SerializeField] NpcCamera npcCamera;

    List<InteractButton> interactButtons;
    [SerializeField] InteractButton questButton;
    [SerializeField] InteractButton shopButton;
    [SerializeField] InteractButton craftButton;
    [SerializeField] InteractButton teleportButton;

    [SerializeField] Button exitButton;

    [SerializeField] TeleportInteractsUI teleportUI;
    [SerializeField] QuestInteractsUI questUI;
    private PlayerInput playerInput;

    bool isLastInteract = false;
    public int npcId {  get; private set; } // 상점 오픈을 위해 사용
    int questId = 0;
    int questProgress = 0;
    int talkIndex;
    bool isSubIntetract;


    byte npcData;
    Transform npcTransfrom;
    protected override void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    private void Start()
    {
        interactButtons = new List<InteractButton>();
        InputInit();
    }
    private void InputInit()
    {
        playerInput.actions["Interact"].performed += PerformedInteract;
        for(int i = 0; i < 4; i++)
        {
            int index = i;
            playerInput.actions[$"SubInteract{i+1}"].performed += ctx => PerformedSubInteract(ctx, index);
        }
    }
    public void SetNpc(int _npcId, byte _data, Transform _npcTransform)
    {
        npcId = _npcId;
        npcData = _data;
        npcTransfrom = _npcTransform;
    }
    public void SetNpc(int _npcId, byte _data, Transform _npcTransform, TeleportData _teleportData)
    {
        SetNpc(_npcId, _data, _npcTransform);
        teleportUI.SetUI(_teleportData);
    }
    private void PerformedInteract(InputAction.CallbackContext context)
    {
        if (!talkUi.GetObjectActiveSelf() && npcId != 0) // 대화 시작
        {
            talkIndex = 0;
            isSubIntetract = false;
            talkUi.OpenUI();
            talkUi.TypingStart((npcId, questId, questProgress, talkIndex));
            npcCamera.SetCameraPos(npcTransfrom);
        }
        else if (talkUi.IsTyping()) // 대사 출력중이면 바로 완료
        {
            talkUi.TypingEnd();
            SetNextTalkId();
        }
        else if (TalkDataBase.TalkDB.ContainsKey((npcId, questId, questProgress, talkIndex)) && !isSubIntetract) // 다음대사가 존재하면 타이핑 시작
        {
            Debug.Log("대사시작" + (npcId, questId, questProgress, talkIndex));
            talkUi.TypingStart((npcId, questId, questProgress, talkIndex));
        }
        else if (isSubIntetract)
        {
            //퀘스트 선택 및 이동 선택에서 아무것도 하지 않기 위함
        }
        else // 대화 종료
        {
            CloseUI();
        }
    }
    private void PerformedSubInteract(InputAction.CallbackContext context, int _index)
    {
        if (isLastInteract)
        {
            if (_index < interactButtons.Count)
            {
                if (interactButtons[_index].gameObject.activeSelf)
                {
                    interactButtons[_index].onClick();
                }
            }
        }
    }
    public void SetNextTalkId() // TalkUI의 유니티액션에 등록
    {
        talkIndex++;
        if (!TalkDataBase.TalkDB.ContainsKey((npcId, questId, questProgress, talkIndex)) && questId == 0) //마지막 일반대사에서 호출
        {
            if ((npcData & 1) != 0)
            {
                questButton.gameObject.SetActive(true);
                interactButtons.Add(questButton);
            }
            if ((npcData & 2) != 0)
            {
                shopButton.gameObject.SetActive(true);
                interactButtons.Add(shopButton);
            }
            if ((npcData & 4) != 0)
            {
                craftButton.gameObject.SetActive(true);
                interactButtons.Add(craftButton);
            }
            if ((npcData & 8) != 0)
            {
                teleportButton.gameObject.SetActive(true);
                interactButtons.Add(teleportButton);
            }
            exitButton.gameObject.SetActive(true);
            isLastInteract = true;
        }
    }
    public void SetQuestTalkId()
    {
        questId = 0;
        talkIndex = 1;
        isSubIntetract = true;
        talkUi.TypingStart((0, questId, questProgress, talkIndex));
        CloseInteractButtons();
        interactButtons.Clear();
        questUI.SetUI(npcId);
    }
    public void SetTeleportTalkId()
    {
        questId = 0;
        talkIndex = 3;
        isSubIntetract = true;
        talkUi.TypingStart((0, questId, questProgress, talkIndex));
        CloseInteractButtons();
        interactButtons.Clear();
        teleportUI.OpenUI();
    }
    public void SetQuestTalk(int _questId, int _questProgress)
    {
        questId = _questId;
        questProgress = _questProgress;
        talkIndex = 0;
        isSubIntetract = false;
        talkUi.TypingEnd();
        talkUi.TypingStart((npcId, questId, questProgress, talkIndex));
        interactButtons.Clear();
        questUI.CloseUI();
    }
    public void TalkToQuest(int _questId)
    {
        QuestManager.Instance.Talk(_questId, npcId);
    }
    public void CloseUI()
    {
        CloseInteractButtons();
        questId = 0;
        questProgress = 0;
        interactButtons.Clear();
        teleportUI.CloseUI();
        questUI.CloseUI();
        talkUi.CloseUI();
        npcCamera.CloseNpcCam();
        GameManager.Instance.GameModeChange(GameMode.ControllMode);
    }
    void CloseInteractButtons()
    {
        questButton.gameObject.SetActive(false);
        shopButton.gameObject.SetActive(false);
        craftButton.gameObject.SetActive(false);
        teleportButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
    }
}
