using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcInformation : MonoBehaviour
{
    [field: SerializeField] public int npcId { get; private set; }
    [field: SerializeField] public byte npcData { get; private set; }
    /*
     * npcData는 비트자리수가 1이면 있고 0이면 없는 값
     * 1 퀘스트
     * 2 상점
     * 4 합성
     * 8 텔포
     * 16
     * 32
     * 64
     * 128
     */
}
