using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemDeleteFaleUI : MonoBehaviour
{
    string deleteInforText = "현재 갯수를 초과하여\r\n버릴수 없습니다.";
    string saleInforText = "현재 갯수를 초과하여\r\n판매할 수 없습니다.";
    [SerializeField] TMP_Text text;
    public void SetText(bool _isSale)
    {
        text.text = _isSale ? saleInforText : deleteInforText;
    }
}
