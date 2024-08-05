using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoad : MonoBehaviour
{
    [SerializeField] CraftDataBase craftDatabase;
    [SerializeField] ItemDataBase itemDatabase;
    [SerializeField] ShopDataBase shopDatabase;
    [SerializeField] QuestDataBase questDatabase;
    [SerializeField] SkillDataBase skillDataBase;
    [SerializeField] BuffDataBase buffDatabase;


    private async void Start()
    {
        Debug.Log("CSV load Start");
        await UniTask.WhenAll(craftDatabase.ReadCSV(), itemDatabase.ReadCSV(), shopDatabase.ReadCSV(), questDatabase.ReadCSV(), skillDataBase.ReadCSV(), buffDatabase.ReadCSV());
        Debug.Log("CSV load comp");
        DataManager.Instance.LoadGame();
    }
}
