using TMPro;
using UnityEngine;

public class ItemBuyUI : MonoBehaviour
{
    [SerializeField] NumericInputField field;
    [SerializeField] TMP_Text inforText;
    [SerializeField] GameObject buyFailUI;

    ItemBase item;
    int price;
    ItemType type;

    string equipBuyInforText = "아이템을 구매하시겠습니까?";
    string otherBuyInforText = "구매할 갯수를 입력하세요.";
    public void BuyItem() // 구매버튼에 등록할것
    {
        switch (item.itemId / 100000000)
        {
            case 1:
                BuyEquipmentItem();
                break;
            default:
                BuyOtherItem();
                break;
        }
        DragManager.Instance.DragReset();
    }
    public void SetItem(ItemBase _item, int _price)
    {
        gameObject.SetActive(true);

        item = _item;
        price = _price;
        switch (item.itemId / 100000000)
        {
            case 1: // 장비아이템일 경우
                field.gameObject.SetActive(false);
                inforText.text = equipBuyInforText;
                break;
            default:
                field.gameObject.SetActive(true);
                inforText.text = otherBuyInforText;
                break;
        }
    }
    public void Cancel()
    {
        gameObject.SetActive(false);
    }
    void BuyEquipmentItem()
    {
        if (InventoryData.Instance.gold >= price)
        {
            InventoryData.Instance.GetItem(item.itemId);
            InventoryData.Instance.GetGold(-price);
            Cancel();
        }
        else
        {
            FailUIOpen();
        }
    }
    void BuyOtherItem()
    {
        int value = field.GetValue();
        if (value == 0) return;
        if (InventoryData.Instance.gold >= value * price)
        {
            InventoryData.Instance.GetItem(item.itemId, value);
            InventoryData.Instance.GetGold(-value * price);
            Cancel();
        }
        else
        {
            FailUIOpen();
        }
    }
    public void FailUIOpen()
    {
        gameObject.SetActive(false);
        buyFailUI.SetActive(true);
    }
}
