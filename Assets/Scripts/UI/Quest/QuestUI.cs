using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [SerializeField] QuestSlot[] slots;
    [SerializeField] QuestInformationUI informationUI;
    int pageIndex;
    QuestProgress progressType = QuestProgress.NotStarted;
    List<QuestData> questList = new List<QuestData>();
    private void OnEnable()
    {
        SetTap(progressType);
        InformationClose();
    }
    public void SetTap(QuestProgress _progressType)
    {
        progressType = _progressType;
        switch (progressType)
        {
            case QuestProgress.NotStarted:
                questList = QuestManager.Instance.startableQuests;
                break;
            case QuestProgress.InProgress:
                questList = QuestManager.Instance.activeQuests;
                break;
            case QuestProgress.Completed:
                questList = QuestManager.Instance.completeQuests;
                break;
        }
        pageIndex = 0;
        SetSlots();
    }
    public void SetSlots()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            int index = pageIndex * slots.Length + i;
            if (index < questList.Count)
            {
                slots[i].SetSlot(questList[index].questId);
            }
            else
            {
                slots[i].DisableSlot();
            }
        }
    }
    public void InformationOpen(int _id)
    {
        informationUI.OpenUI(progressType, _id);
    }
    public void InformationClose()
    {
        informationUI.CloseUI();
    }
    public void SetPrevPage()
    {
        pageIndex = pageIndex-- > 0 ? pageIndex-- : 0;
        SetSlots();
    }
    public void SetNextPage()
    {
        pageIndex = pageIndex++ < QuestManager.Instance.startableQuests.Count / slots.Length ? pageIndex++ : QuestManager.Instance.startableQuests.Count / slots.Length;
        SetSlots();
    }
}
