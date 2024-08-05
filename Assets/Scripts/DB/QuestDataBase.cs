using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class QuestDataBase : MonoBehaviour, ICSVRead
{
    public static Dictionary<int, QuestData> QuestDB = new Dictionary<int, QuestData>();
    public static Dictionary<int, QuestRewards> RewardDB = new Dictionary<int, QuestRewards>();
    public static Dictionary<int, QuestInformation> InfoDB = new Dictionary<int, QuestInformation>();
    int isComplete = 0;
    public async UniTask ReadCSV()
    {
        AddressableManager.Instance.LoadAsset<TextAsset>("QuestDB", QuestCSVLoad);
        AddressableManager.Instance.LoadAsset<TextAsset>("QuestRewardDB", RewardCSVLoad);
        await UniTask.WaitUntil(() => isComplete == 2);
    }
    void QuestCSVLoad(TextAsset _csv)
    {
        var lines = Regex.Split(_csv.text, CSVReader.LINE_SPLIT_RE);

        List<int> ids;
        List<int> counts;

        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], CSVReader.SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;
            ids = new List<int>();
            counts = new List<int>();
            for (int j = 9; j < values.Length - 1; j += 2)
            {
                if (values[j] == "") break;
                ids.Add(CSVReader.GetIntData(values[j]));
                counts.Add(CSVReader.GetIntData(values[j+1]));
            }
            InfoDB.Add(CSVReader.GetIntData(values[0]), new QuestInformation(CSVReader.GetStringData(values[1]), CSVReader.GetStringData(values[2]), CSVReader.GetStringData(values[3]), CSVReader.GetStringData(values[4])));
            QuestDB.Add(CSVReader.GetIntData(values[0]), new QuestData(CSVReader.GetIntData(values[0]), (QuestType)CSVReader.GetIntData(values[5]), CSVReader.GetIntData(values[6]), CSVReader.GetIntData(values[7]), CSVReader.GetIntData(values[8]), ids, counts));
        }
        AddressableManager.Instance.ReleaseAsset("QuestDB");
        isComplete++;
    }

    void RewardCSVLoad(TextAsset _csv)
    {
        var lines = Regex.Split(_csv.text, CSVReader.LINE_SPLIT_RE);

        List<int> ids;
        List<int> counts;

        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], CSVReader.SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;
            ids = new List<int>();
            counts = new List<int>();
            for (int j = 3; j < values.Length - 1; j += 2)
            {
                if (values[j] == "") break;
                ids.Add(CSVReader.GetIntData(values[j]));
                counts.Add(CSVReader.GetIntData(values[j + 1]));
            }
            RewardDB.Add(CSVReader.GetIntData(values[0]), new QuestRewards(CSVReader.GetIntData(values[1]), CSVReader.GetIntData(values[2]), ids, counts));
        }
        AddressableManager.Instance.ReleaseAsset("QuestRewardDB");
        isComplete++;
    }
}
