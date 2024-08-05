using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractNpc : MonoBehaviour
{
    NpcInformation information;
    [SerializeField] Transform camLookAtTransform;
    TeleportData teleportData;
    private void Awake()
    {
        information = GetComponent<NpcInformation>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            if(TryGetComponent(out teleportData))
            {
                TalkManager.Instance.SetNpc(information.npcId, information.npcData, camLookAtTransform.transform, teleportData);
            }
            else
            {
                TalkManager.Instance.SetNpc(information.npcId, information.npcData, camLookAtTransform.transform);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            TalkManager.Instance.SetNpc(0, 0, transform);
        }
    }
}
