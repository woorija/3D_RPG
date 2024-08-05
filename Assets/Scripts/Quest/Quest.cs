using System.Collections.Generic;
public class QuestData
{
    public int questId { get; private set; }
    public QuestType questType {  get; private set; }
    public int constraintLevel { get; private set; }
    public int preQuestId { get; private set; }
    public int completeNpcId { get; private set; }

    public List<int> ids { get; private set; }
    public List<int> counts { get; private set; }

    public QuestData(int _id, QuestType _type,int _constraintLevel,int _preQuestId,int _npcId, List<int> _ids, List<int> _counts)
    {
        questId = _id;
        questType = _type;
        constraintLevel = _constraintLevel;
        preQuestId = _preQuestId;
        completeNpcId = _npcId;
        ids = _ids;
        counts = _counts;
    }
}
