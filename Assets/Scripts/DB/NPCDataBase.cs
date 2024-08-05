using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDataBase : MonoBehaviour
{
    public static readonly Dictionary<int, string> NPCDB = new Dictionary<int, string>()
    {
         {1, "테스트NPC1"},
         {2, "테스트NPC2"},
         {3, "테스트NPC3"}
    };
}
