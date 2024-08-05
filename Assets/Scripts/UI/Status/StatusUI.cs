using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusUI : MonoBehaviour
{
    [SerializeField] StatusInformation[] Infor;
    [SerializeField] PlayerStatus status;
    private void Start()
    {
        gameObject.SetActive(false);
        status.GetAllEquipmentStatus();
        SetAllInformation();
        status.AddStatusEvent(SetInformation);
    }
    public void SetInformation(StatusType _index)
    {
        switch (_index)
        {
            case StatusType.Level:
                Infor[(int)_index].SetText(status.Level);
                break;
            case StatusType.AttackPoint:
                Infor[(int)_index].SetNormalStatusText(status.BaseAttackPower, status.finalAttackPower - status.BaseAttackPower);
                break;
            case StatusType.DefensePoint:
                Infor[(int)_index].SetNormalStatusText(status.BaseDefensePower, status.finalDefensePower - status.BaseDefensePower);
                break;
            case StatusType.Hp:
                Infor[(int)_index].SetPointStatusText(status.Hp, status.FinalMaxHp);
                break;
            case StatusType.Mp:
                Infor[(int)_index].SetPointStatusText(status.Mp, status.FinalMaxMp);
                break;
        }
    }
    void SetAllInformation()
    {
        foreach(StatusType _type in Enum.GetValues(typeof(StatusType)))
        {
            SetInformation(_type);
        }
    }
}
