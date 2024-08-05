using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class NpcCamera : MonoBehaviour
{
    CinemachineVirtualCamera npcCamera;
    private void Awake()
    {
        npcCamera = GetComponent<CinemachineVirtualCamera>();
    }
    public void SetCameraPos(Transform npcTransform)
    {
        npcCamera.enabled = true;
        Vector3 pos = npcTransform.position + 2f * npcTransform.forward;
        npcCamera.transform.position = pos;
        npcCamera.LookAt = npcTransform;
    }
    public void CloseNpcCam()
    {
        Debug.Log("closeCam");
        npcCamera.enabled = false;
    }
}
