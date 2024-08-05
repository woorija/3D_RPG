using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] int _id;
    [SerializeField] int _amount;

    public void GetItem()
    {
        InventoryData.Instance.GetItem( _id, _amount);
    }
}
