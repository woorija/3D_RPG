using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] SkillIcon icon;
    [SerializeField] TMP_Text skillName;
    [SerializeField] TMP_Text skillLevel;

    [SerializeField] GameObject levelUpButton;
    [SerializeField] GameObject levelDownButton;

    int slotSkillId;
    int currentLevel;
    Skill skill;
    public Action OnLevelChangeEvent;
    public void SetSlot(int _id)
    {
        gameObject.SetActive(true);
        slotSkillId = _id;
        icon.SetIcon(slotSkillId);
        currentLevel = SkillData.Instance.acquiredSkillLevels[slotSkillId];
        skill = SkillDataBase.SkillDB[slotSkillId];
        skillName.text = SkillDataBase.InfoDB[slotSkillId].skillName;
        SetSkillLevelText();
        ButtonUpdate();
    }
    void SetSkillLevelText()
    {
        if (skill.prevSkillId != 0 && SkillData.Instance.acquiredSkillLevels[skill.prevSkillId] < skill.prevSkillLevel)
        {
            skillLevel.text = $"{SkillDataBase.InfoDB[skill.prevSkillId].skillName} Lv.{skill.prevSkillLevel}▲";
        }
        else
        {
            skillLevel.text = $"( {currentLevel} / {skill.masterLevel} )";
        }
    }
    public void ResetSlot()
    {
        gameObject.SetActive(false);
    }


    //아래는 버튼적용 함수
    public void SkillLevelUp()
    {
        SkillData.Instance.IncreaseSkillLevel(slotSkillId);
        OnLevelChangeEvent?.Invoke();
        ButtonUpdate();
    }
    public void SkillLevelDown()
    {
        SkillData.Instance.DecreaseSkillLevel(slotSkillId);
        OnLevelChangeEvent?.Invoke();
        ButtonUpdate();
    }

    void ButtonUpdate()
    {
        levelUpButton.SetActive(IsLevelUpActive());
        levelDownButton.SetActive(IsLevelDownActive());
    }
    bool IsLevelUpActive()
    {
        if (skill.prevSkillId != 0 && SkillData.Instance.acquiredSkillLevels[skill.prevSkillId] < skill.prevSkillLevel) return false;
        if (currentLevel == skill.masterLevel) return false;
        return true;
    }
    bool IsLevelDownActive()
    {
        if (currentLevel == 0) return false;
        if (SkillData.Instance.prevSkillLevels.ContainsKey(slotSkillId) && SkillData.Instance.prevSkillLevels[slotSkillId] >= SkillData.Instance.acquiredSkillLevels[slotSkillId]) return false;
        return true;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!DragManager.Instance.isClick)
        {
            SkillInformationUI.Instance.SetInformation(slotSkillId);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SkillInformationUI.Instance.InformationClose();
    }
}
