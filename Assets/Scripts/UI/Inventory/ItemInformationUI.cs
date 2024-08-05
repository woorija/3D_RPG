using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using System.Text;

public class ItemInformationUI : MonoBehaviour
{
    public static ItemInformationUI Instance;
    ItemBase item;

    [SerializeField] GameObject informationUI;
    [SerializeField] Image itemIcon;
    [SerializeField] TMP_Text itemName;
    [SerializeField] TMP_Text EquipmentItemInformation;
    [SerializeField] TMP_Text otherItemInformation;
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
    public void SetInformation(ItemBase _item)
    {
        InformationOpen();
        item = _item;
        switch (item.itemId / 100000000)
        {
            case 1:
                SetEquipmentInformation();
                SetEmptyInformation(true);
                break;
            default:
                SetOtherInformation();
                SetEmptyInformation(false);
                break;
        }
        SetNameAndImage();
    }
    void SetNameAndImage()
    {
        AddressableManager.Instance.LoadAsset<Sprite>($"ItemIcon/I{item.itemId}.png", SetSprite);
        itemName.text = item.name;
    }
    void SetSprite(Sprite _sprite)
    {
        itemIcon.sprite = _sprite;
    }
    void SetEmptyInformation(bool _isEquipment)
    {
        if (_isEquipment)
        {
            otherItemInformation.text = string.Empty;
        }
        else
        {
            EquipmentItemInformation.text = string.Empty;
        }
    }
    void SetEquipmentInformation()
    {
        EquipmentItem _item = (EquipmentItem)item;
        StringBuilder stringBuilder = new StringBuilder(_item.description, 300);
        if(_item.attackPower != 0)
        {
            stringBuilder.AppendFormat("\n공격력 + {0}", _item.attackPower);
        }
        if (_item.defensePower != 0)
        {
            stringBuilder.AppendFormat("\n방어력 + {0}", _item.defensePower);
        }
        if (_item.hp != 0)
        {
            stringBuilder.AppendFormat("\nHP + {0}", _item.hp);
        }
        if (_item.mp != 0)
        {
            stringBuilder.AppendFormat("\nMP + {0}", _item.mp);
        }
        EquipmentItemInformation.text = stringBuilder.ToString();
    }
    void SetOtherInformation()
    {
        otherItemInformation.text = string.Format("{0}", item.description);
    }
}
