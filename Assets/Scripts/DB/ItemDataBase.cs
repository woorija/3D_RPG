using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ItemDataBase : MonoBehaviour, ICSVRead
{
    //장비아이템은 데이터상으로 1개
    //소비,기타 아이템은 데이터상으로 0개 시작
    public static Dictionary<int, EquipmentItem> EquipmentItemDB = new Dictionary<int, EquipmentItem>();
    public static Dictionary<int, UseableItem> UseableItemDB = new Dictionary<int, UseableItem>();
    public static Dictionary<int, MiscItem> MiscItemDB = new Dictionary<int, MiscItem>();
    int complete = 0;

    public async UniTask ReadCSV()
    {
        AddressableManager.Instance.LoadAsset<TextAsset>("EquipmentItemDB", EquipmentLoad);
        AddressableManager.Instance.LoadAsset<TextAsset>("UseableItemDB", UseableLoad);
        AddressableManager.Instance.LoadAsset<TextAsset>("MiscItemDB", MiscLoad);
        await UniTask.WaitUntil(() => complete == 3);
    }
    public static string GetItemName(int _id)
    {
        string itemName = string.Empty;
        switch (_id / 100000000)
        {
            case 1:
                itemName = EquipmentItemDB[_id].name;
                break;
            case 2:
                itemName = UseableItemDB[_id].name;
                break;
            case 3:
                itemName = MiscItemDB[_id].name;
                break;
        }
        return itemName;
    }
    void EquipmentLoad(TextAsset _csv)
    {
        var lines = Regex.Split(_csv.text, CSVReader.LINE_SPLIT_RE);

        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], CSVReader.SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            EquipmentItemDB.Add(CSVReader.GetIntData(values[0]), new EquipmentItem(CSVReader.GetIntData(values[0]), CSVReader.GetStringData(values[1]), CSVReader.GetStringData(values[2]), CSVReader.GetIntData(values[3]), CSVReader.GetIntData(values[4]), CSVReader.GetIntData(values[5]), CSVReader.GetIntData(values[6]), CSVReader.GetIntData(values[7])));
        }
        AddressableManager.Instance.ReleaseAsset("EquipmentItemDB");
        complete++;
    }
    void UseableLoad(TextAsset _csv)
    {
        var lines = Regex.Split(_csv.text, CSVReader.LINE_SPLIT_RE);

        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], CSVReader.SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            UseableItemDB.Add(CSVReader.GetIntData(values[0]), new UseableItem(CSVReader.GetIntData(values[0]), CSVReader.GetStringData(values[1]), CSVReader.GetStringData(values[2]), CSVReader.GetIntData(values[3]), CSVReader.GetIntData(values[4])));
        }
        AddressableManager.Instance.ReleaseAsset("UseableItemDB");
        complete++;
    }
    void MiscLoad(TextAsset _csv)
    {
        var lines = Regex.Split(_csv.text, CSVReader.LINE_SPLIT_RE);

        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], CSVReader.SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            MiscItemDB.Add(CSVReader.GetIntData(values[0]), new MiscItem(CSVReader.GetIntData(values[0]), CSVReader.GetStringData(values[1]), CSVReader.GetStringData(values[2]), CSVReader.GetIntData(values[3]), CSVReader.GetIntData(values[4])));
        }
        AddressableManager.Instance.ReleaseAsset("MiscItemDB");
        complete++;
    }
}
