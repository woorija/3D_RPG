using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Text;

public class SkillInformationUI : MonoBehaviour
{
    public static SkillInformationUI Instance;

    int skillId;

    [SerializeField] GameObject informationUI;
    [SerializeField] Image skillIcon;
    [SerializeField] TMP_Text skillName;
    [SerializeField] TMP_Text skillDescription;
    [SerializeField] TMP_Text skillInformation;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        InformationClose();
    }
    private void Update()
    {
        if (informationUI.activeSelf)
        {
            transform.position = Mouse.current.position.ReadValue();
        }
    }

    public void InformationOpen()
    {
        if (!informationUI.activeSelf)
        {
            informationUI.SetActive(true);
        }
    }
    public void InformationClose()
    {
        if (informationUI.activeSelf)
        {
            informationUI.SetActive(false);
        }
    }
    public void SetInformation(int _id)
    {
        InformationOpen();
        if (skillId != _id)
        {
            skillId = _id;
            SetNameAndImage();
            SetDescription();
            SetSkillInfomations();
        }
    }
    void SetNameAndImage()
    {
        if (skillId == 0) return;
        AddressableManager.Instance.LoadAsset<Sprite>($"SkillIcon/S{skillId}.png", SetSprite);
        skillName.text = SkillDataBase.InfoDB[skillId].skillName;
    }
    void SetSprite(Sprite _sprite)
    {
        skillIcon.sprite = _sprite;
    }
    void SetDescription()
    {
        skillDescription.text = SkillDataBase.InfoDB[skillId].skillDescription;
    }
    public void SetSkillInfomations()
    {
        StringBuilder sb = new StringBuilder(256);
        var infoDB = SkillDataBase.InfoDB[skillId];
        var skillDB = SkillDataBase.SkillDB[skillId];

        sb.Append(infoDB.skillInformations[0]);
        for(int i = 0;i < skillDB.initialSkillMultiplier.Count; i++)
        {
            sb.Append($" {skillDB.initialSkillMultiplier[i] + skillDB.increaseSkillMultiplier[i] * SkillData.Instance.acquiredSkillLevels[skillId]}{infoDB.skillInformations[i + 1]}");
        }
        skillInformation.text = sb.ToString();
    }
}
