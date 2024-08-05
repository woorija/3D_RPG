using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICloseButton : MonoBehaviour
{
    [SerializeField] GameObject UIObject;
    public void UIClose()
    {
        UIObject.SetActive(false);
    }
}
