using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatusInformation : MonoBehaviour
{
    [SerializeField] TMP_Text statusInforMation;
    public void SetText(int _value)
    {
        statusInforMation.text = _value.ToString();
    }
    public void SetNormalStatusText(int _value, int _value2)
    {
        if(_value2 == 0)
        {
            SetText(_value);
            return;
        }
        int sum = _value + _value2;
        statusInforMation.text = string.Format("{0} ({1} + {2})", sum, _value, _value2);
    }
    public void SetPointStatusText(int _value, int _value2)
    {
        statusInforMation.text = string.Format("{0} / {1}",_value, _value2);
    }
}
