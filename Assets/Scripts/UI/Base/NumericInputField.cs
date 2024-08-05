using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumericInputField : MonoBehaviour
{
    private TMP_InputField inputField;
    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.onValueChanged.AddListener(OnValueChanged);
    }
    private void OnEnable()
    {
        ResetValue();
    }

    private void OnValueChanged(string value)
    {
        string filteredValue = Regex.Replace(value, @"[^0-9]", "");
        inputField.text = filteredValue; // 필터링된 값을 InputField에 적용
    }
    public int GetValue()
    {
        return string.IsNullOrEmpty(inputField.text) ? 0 : int.Parse(inputField.text);
    }
    public void ResetValue()
    {
        inputField.text = string.Empty;
    }
}
