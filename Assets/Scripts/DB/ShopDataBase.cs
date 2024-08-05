using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ShopItemData
{
    public int itemId {  get; private set; }
    public int price { get; private set; }
    public ShopItemData(int _id, int _price)
    {
        itemId = _id;
        price = _price;
    }
}
public class ShopData
{
    public List<ShopItemData> itemData { get; private set; }
    public ShopData(List<ShopItemData> _data)
    {
        itemData = _data;
    }
}
public class ShopDataBase : MonoBehaviour, ICSVRead
{
    public static Dictionary<int, ShopData> ShopDB = new Dictionary<int, ShopData>();

    bool isComplete = false;
    public async UniTask ReadCSV()
    {
        AddressableManager.Instance.LoadAsset<TextAsset>("ShopDB", CSVLoad);
        await UniTask.WaitUntil(() => isComplete);
    }
    void CSVLoad(TextAsset _csv)
    {
        var lines = Regex.Split(_csv.text, CSVReader.LINE_SPLIT_RE);

        List<ShopItemData> shopData;

        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], CSVReader.SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;
            shopData = new List<ShopItemData>();
            for (int j = 1; j < values.Length - 1; j += 2)
            {
                if (values[j] == "") break;
                shopData.Add(new ShopItemData(CSVReader.GetIntData(values[j]), CSVReader.GetIntData(values[j + 1])));
            }
            ShopDB.Add(CSVReader.GetIntData(values[0]), new ShopData(shopData));
        }
        AddressableManager.Instance.ReleaseAsset("ShopDB");
        isComplete = true;
    }
}
