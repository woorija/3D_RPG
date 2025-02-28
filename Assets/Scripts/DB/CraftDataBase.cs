using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public struct CraftItemData
{
    public int itemId { get; private set; }
    public int itemAmount { get; private set; }
    public CraftItemData(int _id, int _amount)
    {
        itemId = _id;
        itemAmount = _amount;
    }
}

public class CraftData
{
    public CraftItemData resultItem { get; private set; }
    public List<CraftItemData> materialItems { get; private set; }
    public CraftData(CraftItemData _item,List<CraftItemData> _materialItems)
    {
        resultItem = _item;
        materialItems = _materialItems;
    }
}

public class CraftDataBase : MonoBehaviour, ICSVRead
{
    public static Dictionary<int, CraftData> CraftDB = new Dictionary<int, CraftData>();
    bool isComplete = false;
    public async UniTask ReadCSV()
    {
        AddressableManager.Instance.LoadAsset<TextAsset>("CraftDB", CSVLoad);
        await UniTask.WaitUntil(() => isComplete);
    }
    void CSVLoad(TextAsset _csv)
    {
        var lines = Regex.Split(_csv.text, CSVReader.LINE_SPLIT_RE);

        CraftItemData resultItem;
        List<CraftItemData> materialItems;

        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], CSVReader.SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;
            resultItem = new CraftItemData(CSVReader.GetIntData(values[1]), CSVReader.GetIntData(values[2]));
            materialItems = new List<CraftItemData>();
            for (int j = 3; j < values.Length - 1; j += 2)
            {
                if (values[j] == "") break;
                materialItems.Add(new CraftItemData(CSVReader.GetIntData(values[j]), CSVReader.GetIntData(values[j + 1])));
            }
            CraftDB.Add(CSVReader.GetIntData(values[0]), new CraftData(resultItem, materialItems));
        }
        AddressableManager.Instance.ReleaseAsset("CraftDB");
        isComplete = true;
    }
}
