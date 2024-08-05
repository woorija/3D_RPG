using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : SingletonBehaviour<QuestManager>
{
    #region 퀘스트리스트
    List<QuestData> notStartQuests = new List<QuestData>();
    public List<QuestData> startableQuests { get; private set; } = new List<QuestData>();
    public List<QuestData> inProgressQuests { get; private set; } = new List<QuestData>();
    public List<QuestData> completableQuests { get; private set; } = new List<QuestData>();
    public List<QuestData> completeQuests { get; private set; } = new List<QuestData>();
    public Dictionary<int, Dictionary<int, bool>> talkDatas { get; private set; } = new Dictionary<int, Dictionary<int, bool>>();
    public Dictionary<int, Dictionary<int,int>> huntDatas { get; private set; }  = new Dictionary<int, Dictionary<int, int>>();
    // 1번 딕셔너리 키 = 퀘스트id
    // 2번 딕셔너리 키 = 몬스터id
    // 2번 딕셔너리 값 = 몬스터 사냥수

    public List<QuestData> activeQuests { get; private set; } = new List<QuestData>();
    #endregion

    #region NPC 퀘스트 리스트
    //아래 3개 리스트는 npc와 대화 시 적용시킬 퀘스트 id 리스트
    public List<int> currentCompletableQuestIds { get; private set; } = new List<int>();
    public List<int> currentInprogressableQuestIds { get; private set; } = new List<int>();
    public List<int> currentStartableQuestIds { get; private set; } = new List<int>();
    #endregion

    [SerializeField] PlayerStatus playerStatus;
    public void LoadData(SaveDataQuest _data)
    {
        LoadQuestData(_data);
        SetQuestData();
    }
    public void SaveQuestData(SaveDataQuest _data)
    {
        for(int i = 0; i < completeQuests.Count; i++)
        {
            _data.completeIds.Add(completeQuests[i].questId);
        }
        for(int i=0;i<completableQuests.Count;i++)
        {
            _data.completableIds.Add(completableQuests[i].questId);
        }
        for(int i = 0;i < inProgressQuests.Count; i++)
        {
            _data.inProgressIds.Add(inProgressQuests[i].questId);
        }
        foreach(var huntData in huntDatas)
        {
            _data.huntQuestDatas.Add(new HuntQuestData(huntData.Key, huntData.Value));
        }
        foreach(var talkData in talkDatas)
        {
            _data.talkQuestDatas.Add(new TalkQuestData(talkData.Key, talkData.Value));
        }
    }
    public void LoadQuestData(SaveDataQuest _data)
    {
        for(int i=0; i < _data.completeIds.Count;i++)
        {
            completeQuests.Add(QuestDataBase.QuestDB[_data.completeIds[i]]);
        }
        for(int i = 0; i < _data.completableIds.Count; i++)
        {
            completableQuests.Add(QuestDataBase.QuestDB[_data.completableIds[i]]);
            activeQuests.Add(QuestDataBase.QuestDB[_data.completableIds[i]]);
        }
        for(int i=0;i< _data.inProgressIds.Count; i++)
        {
            inProgressQuests.Add(QuestDataBase.QuestDB[_data.inProgressIds[i]]);
            activeQuests.Add(QuestDataBase.QuestDB[_data.inProgressIds[i]]);
        }
        foreach (var huntQuest in _data.huntQuestDatas)
        {
            Dictionary<int, int> huntData = new Dictionary<int, int>();
            foreach (var hunt in huntQuest.huntDatas)
            {
                huntData.Add(hunt.id, hunt.count);
            }
            huntDatas.Add(huntQuest.questId, huntData);
        }
        foreach(var talkQuest in _data.talkQuestDatas)
        {
            Dictionary<int, bool> talkData = new Dictionary<int, bool>();
            foreach(var talk in talkQuest.talkDatas)
            {
                talkData.Add(talk.id, talk.isTalk);
            }
            talkDatas.Add(talkQuest.questId, talkData);
        }
    }
    public void SetQuestData()
    {
        foreach(var quest in QuestDataBase.QuestDB.Values.Where(quest => !inProgressQuests.Contains(quest) && !completableQuests.Contains(quest) && !completeQuests.Contains(quest)))
        {
            if (IsQuestStartable(quest))
            {
                startableQuests.Add(quest);
            }
            else
            {
                notStartQuests.Add(quest);
            }
        }
    }
    public void SetStartableQuest() // 퀘스트 클리어 or 레벨업시 적용
    {
        for(int i=notStartQuests.Count-1;i>=0;i--)
        {
            if (IsQuestStartable(notStartQuests[i]))
            {
                startableQuests.Add(notStartQuests[i]);
                notStartQuests.Remove(notStartQuests[i]);
            }
        }
    }
    public void SetStartableQuestId(int _npcId)
    {
        currentStartableQuestIds.Clear();
        foreach(var questData in startableQuests)
        {
            if(questData.questId/100 == _npcId)
            {
                currentStartableQuestIds.Add(questData.questId);
            }
        }
    }
    public void SetInprogressableQuestId(int _npcId)
    {
        currentInprogressableQuestIds.Clear();
        foreach(var questData in inProgressQuests)
        {
            Debug.Log(questData.questId + "/" + questData.ids[0]);
            if (questData.ids.Contains(_npcId) && !talkDatas[questData.questId][_npcId])
            {
                currentInprogressableQuestIds.Add(questData.questId);
            }
        }
    }
    public void SetCompletableQuestId(int _npcId)
    {
        IsQuestCompletable(_npcId);
        currentCompletableQuestIds.Clear();
        foreach(var questData in completableQuests)
        {
            if(questData.completeNpcId == _npcId)
            {
                currentCompletableQuestIds.Add(questData.questId);
            }
        }
    }
    public bool IsQuestStartable(QuestData _quest)
    {
        if (_quest.constraintLevel > playerStatus.Level) return false;
        if (_quest.preQuestId != 0 && completeQuests.FindIndex(q => q.questId == _quest.preQuestId) == -1) return false;
        return true;
    }
    void IsQuestCompletable(int _npcid)
    {
        for(int i=inProgressQuests.Count-1; i>=0; i--)
        {
            if (inProgressQuests[i].completeNpcId == _npcid)
            {
                if (IsQuestCompletable(inProgressQuests[i]))
                {
                    completableQuests.Add(inProgressQuests[i]);
                    inProgressQuests.Remove(inProgressQuests[i]);
                }
            }
        }
    }
    public bool IsQuestCompletable(QuestData _quest)
    {
        for (int i = 0; i < _quest.ids.Count; i++)
        {
            switch (_quest.ids[i].ToString().Length)
            {
                case 1:
                    if (!talkDatas[_quest.questId][_quest.ids[i]])
                    {
                        return false;
                    }
                    break;
                case 5:
                    if (huntDatas[_quest.questId][_quest.ids[i]] < _quest.counts[i])
                    {
                        return false;
                    }
                    break;
                case 9:
                    if (InventoryData.Instance.GetItemCount(_quest.ids[i]) < _quest.counts[i])
                    {
                        return false;
                    }
                    break;
            }
        }
        return true;
    }
    public void StartQuest(int _questId)
    {
        QuestData quest = QuestDataBase.QuestDB[_questId];
        startableQuests.Remove(quest);
        Debug.Log(quest.questType);
        switch (quest.questType)
        {
            case QuestType.Talk:
                for (int i = 0; i < quest.ids.Count; i++)
                {
                    Dictionary<int, bool> talkdata = new Dictionary<int, bool>
                    {
                        { quest.ids[i], false }
                    };
                    talkDatas.Add(_questId, talkdata);
                }
                break;
            case QuestType.Hunt:
                for (int i = 0; i < quest.ids.Count; i++)
                {
                    Dictionary<int, int> huntData = new Dictionary<int, int>
                    {
                        { quest.ids[i], 0 }
                    };
                    huntDatas.Add(_questId, huntData);
                }
                break;
            case QuestType.Mix:
                for (int i = 0; i < quest.ids.Count; i++)
                {
                    if (quest.ids[i] / 100000000 == 0)
                    {
                        Dictionary<int, int> huntData = new Dictionary<int, int>
                        {
                            { quest.ids[i], 0 }
                        };
                        huntDatas.Add(_questId, huntData);
                    }
                }
                break;
        }
        inProgressQuests.Add(quest);
        activeQuests.Add(quest);
    }
    public void CompleteQuest(int _questId)
    {
        for (int i = huntDatas.Count - 1; i >= 0; i--)
        {
            if (huntDatas.ContainsKey(_questId))
            {
                huntDatas.Remove(_questId);
            }
        }
        for (int i = talkDatas.Count - 1; i >= 0; i--)
        {
            if (talkDatas.ContainsKey(_questId))
            {
                talkDatas.Remove(_questId);
            }
        }

        completableQuests.Remove(QuestDataBase.QuestDB[_questId]);
        activeQuests.Remove(QuestDataBase.QuestDB[_questId]);
        completeQuests.Add(QuestDataBase.QuestDB[_questId]);

        SetStartableQuest();
        GetQuestRewards(_questId);
    }
    void GetQuestRewards(int _questId)
    {
        QuestRewards rewards = QuestDataBase.RewardDB[_questId];
        playerStatus.Exp += rewards.exp;
        InventoryData.Instance.GetGold(rewards.gold);
        for(int i = 0; i < rewards.itemIds.Count; i++)
        {
            InventoryData.Instance.GetItem(rewards.itemIds[i], rewards.itemAmounts[i]);
        }
    }
    public void Hunt(int _monsterId)
    {
        foreach(var huntData in huntDatas)
        {
            if(huntData.Value.ContainsKey(_monsterId))
            {
                huntData.Value[_monsterId]++;
            }
        }
    }
    public void Talk(int _questId, int _npcId)
    {
        talkDatas[_questId][_npcId] = true;
    }
}
