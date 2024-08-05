using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TalkData
{
    public int npcId;
    public string talk;
    public TalkData(int _npcId, string _talk)
    {
        npcId = _npcId;
        talk = _talk;
    }
}
public class TalkDataBase : MonoBehaviour
{
    /*(npcId, questId, progress, talkIndex)
     * progress
     * 0: 퀘스트 수락
     * 1: 퀘스트 완료
     * 2: 퀘스트 진행중
     */
    public static readonly Dictionary<(int, int, int, int), TalkData> TalkDB = new Dictionary<(int, int, int, int), TalkData>()
    {
         {(0, 0, 0, 1), new TalkData(-1, "수락 할 퀘스트를 선택하세요.") }, // 퀘스트 표기용
         {(0, 0, 0, 3), new TalkData(-1, "이동하고 싶은 장소를 선택하세요.") }, // 텔레포트시 표기용
         {(1, 0, 0, 0), new TalkData(1, "대사1")},
         {(1, 0, 0, 1), new TalkData(2, "대사2")},
         {(1, 0, 0, 2), new TalkData(1, "대사3")},
         {(1, 101, 0, 0), new TalkData(1, "퀘스트용 대사1")},
         {(1, 101, 0, 1), new TalkData(1, "퀘스트용 대사2")},
         {(1, 101, 2, 0), new TalkData(2, "퀘스트가 진행된다?")},
         {(1, 101, 2, 1), new TalkData(2, "대화 퀘스트 테스트")},
         {(1, 102, 0, 0), new TalkData(1, "잘왔네")},
         {(1, 102, 0, 1), new TalkData(1, "박쥐를 좀 잡아주게")},
         {(1, 102, 1, 0), new TalkData(1, "3마리를 전부 잡아왔군")},
         {(1, 102, 1, 1), new TalkData(1, "보상을 주겠네")},
         {(2, 0, 0, 0), new TalkData(2, "대사11313")},
         {(2, 0, 0, 1), new TalkData(3, "대사21313")},
         {(2, 0, 0, 2), new TalkData(1, "대사31313")},
         {(2, 101, 1, 0), new TalkData(2, "대화 퀘스트 완료 텍스트1")},
         {(2, 101, 1, 1), new TalkData(2, "대화 퀘스트 완료 텍스트2")},
         {(2, 201, 0, 0), new TalkData(2, "구해야 하는 아이템이 있네")},
         {(2, 201, 0, 1), new TalkData(2, "구해오면 보상을 주겠네")},
         {(2, 201, 1, 0), new TalkData(2, "오! 전부 구해왔군")},
         {(2, 201, 1, 1), new TalkData(2, "보상을 주겠네")},
         {(3, 0, 0, 0), new TalkData(3, "대사12222")},
         {(3, 0, 0, 1), new TalkData(3, "대사22222")},
         {(3, 0, 0, 2), new TalkData(3, "대사32222")},
    };
}
