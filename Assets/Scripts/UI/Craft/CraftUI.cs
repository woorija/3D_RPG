using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftUI : MonoBehaviour
{
    [SerializeField] CraftList craftList;
    [SerializeField] CraftMaterialsUI craftMaterialsUI;
    [SerializeField] CraftItemInformation resultInformation;
    [SerializeField] CraftButton craftButton;
    private void Start()
    {
        craftList.SetList();
        craftMaterialsUI.Init();
        resultInformation.SetNull();
        craftButton.gameObject.SetActive(false);
    }
    public void OpenUI()
    {
        GameManager.Instance.GameModeChange(GameMode.ForcedUIMode);
        gameObject.SetActive(true);
    }
    public void SetCraftUI(int _index)
    {
        craftMaterialsUI.SetInformation(_index);
        resultInformation.SetData(CraftDataBase.CraftDB[_index].resultItem);
        resultInformation.SetText(CraftType.Result);
        craftButton.gameObject.SetActive(true);
        craftButton.SetOnClickButton(_index);
    }
}
