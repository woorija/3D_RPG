using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] PlayerStatus playerStatus;
    [SerializeField] Slider HpSlider, MpSlider, ExpSlider;
    [SerializeField] ExpHUD expHUD;
    public void ChangeStatus_MaxHp()
    {
        HpSlider.maxValue = playerStatus.FinalMaxHp;
    }
    public void ChangeStatus_Hp()
    {
        HpSlider.value = playerStatus.Hp;
    }
    public void ChangeStatus_MaxMp()
    {
        MpSlider.maxValue = playerStatus.FinalMaxMp;
    }
    public void ChangeStatus_Mp()
    {
        MpSlider.value = playerStatus.Mp;
    }
    public void ChangeStatus_MaxExp()
    {
        ExpSlider.maxValue = playerStatus.MaxExp;
        expHUD.SetExpText(playerStatus.Exp, playerStatus.MaxExp);
    }
    public void ChangeStatus_Exp()
    {
        ExpSlider.value = playerStatus.Exp;
        expHUD.SetExpText(playerStatus.Exp, playerStatus.MaxExp);
    }
}
