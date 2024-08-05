using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftList : MonoBehaviour
{
    [SerializeField] Transform equipTypeParent;
    [SerializeField] Transform UseableTypeParent;
    [SerializeField] Transform MiscTypeParent;

    List<int> craftId;
    public void SetList()
    {
        craftId = new List<int>(CraftDataBase.CraftDB.Keys);
        CraftSettingButton temp;

        foreach (int id in craftId)
        {
            temp = PoolManager.Instance.craftButtonPool.Get();
            temp.SetIndex(id);
            switch (temp.GetItemId())
            {
                case 1:
                    temp.gameObject.transform.SetParent(equipTypeParent);
                    break;
                case 2:
                    temp.gameObject.transform.SetParent(UseableTypeParent);
                    break;
                case 3:
                    temp.gameObject.transform.SetParent(MiscTypeParent);
                    break;
            }
            temp.SetData();
        }

    }
}
