using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOpenButton : MonoBehaviour
{
    [SerializeField] GameObject UIObject;
    public void UIToggle()
    {
        UIObject.SetActive(!UIObject.activeSelf);
        if (UIObject.activeSelf)
        {
            GameManager.Instance.GameModeChange(GameMode.UIMode);
        }
        else
        {
            SkillInformationUI.Instance.InformationClose();
            ItemInformationUI.Instance.InformationClose();
        }
    }
}
