using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
public class Skill
{
    public int id; // 8자리
    public int prevSkillId;
    public int prevSkillLevel;
    public int skillType;
    public int initialAcquisitionLevel;
    public int increaseAcquisitionLevel;
    public int masterLevel;
    public int acquisitionSp;
    public float coolTime;
    public float duration;
    public float interval;
    public List<int> initialSkillMultiplier;
    public List<int> increaseSkillMultiplier;
    public Skill(int _id, int _prevSkillId, int _prevSkillLevel, int _skillType, int _initialAcquisitionLevel, int _increaseAcquisitionLevel, int _masterLevel, int _acquisitionSp, float _coolTime, float _duration, float _interval, List<int> _initialSkillMultipliers, List<int> _increaseSkillMultipliers)
    {
        id = _id;
        prevSkillId = _prevSkillId;
        prevSkillLevel = _prevSkillLevel;
        skillType = _skillType;
        initialAcquisitionLevel = _initialAcquisitionLevel;
        increaseAcquisitionLevel = _increaseAcquisitionLevel;
        masterLevel = _masterLevel;
        acquisitionSp = _acquisitionSp;
        coolTime = _coolTime;
        duration = _duration;
        interval = _interval;
        initialSkillMultiplier = _initialSkillMultipliers;
        increaseSkillMultiplier = _increaseSkillMultipliers;
    }
}
public class SkillInfomation
{
    public string skillName;
    public string skillDescription;
    public List<string> skillInformations;
    public SkillInfomation(string _skillName,string _skillDescription, List<string> _skillInformations)
    {
        skillName = _skillName;
        skillDescription = _skillDescription;
        skillInformations = _skillInformations;
    }
}

public class SkillDataBase : MonoBehaviour, ICSVRead
{
    public static Dictionary<int, Skill> SkillDB = new Dictionary<int, Skill>(20);
    public static Dictionary<int, SkillInfomation> InfoDB = new Dictionary<int, SkillInfomation>(20);
    int isComplete = 0;
    public async UniTask ReadCSV()
    {
        AddressableManager.Instance.LoadAsset<TextAsset>("SkillDB", SkillCSVLoad);
        AddressableManager.Instance.LoadAsset<TextAsset>("SkillInformationDB", InformationCSVLoad);
        await UniTask.WaitUntil(() => isComplete == 2);
    }

    void SkillCSVLoad(TextAsset _csv)
    {
        var lines = Regex.Split(_csv.text, CSVReader.LINE_SPLIT_RE);

        List<int> initialSkillMultipliers;
        List<int> increaseSkillMultipliers;

        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], CSVReader.SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;
            initialSkillMultipliers = new List<int>();
            increaseSkillMultipliers = new List<int>();
            for (int j = 11; j < values.Length - 1; j += 2)
            {
                if (values[j] == "") break;
                initialSkillMultipliers.Add(CSVReader.GetIntData(values[j]));
                increaseSkillMultipliers.Add(CSVReader.GetIntData(values[j + 1]));
            }
            SkillDB.Add(CSVReader.GetIntData(values[0]), new Skill(CSVReader.GetIntData(values[0]), CSVReader.GetIntData(values[1]), CSVReader.GetIntData(values[2]), CSVReader.GetIntData(values[3]), CSVReader.GetIntData(values[4]), CSVReader.GetIntData(values[5]), CSVReader.GetIntData(values[6]), CSVReader.GetIntData(values[7]), CSVReader.GetFloatData(values[8]), CSVReader.GetFloatData(values[9]), CSVReader.GetFloatData(values[10]), initialSkillMultipliers, increaseSkillMultipliers));
        }
        AddressableManager.Instance.ReleaseAsset("SkillDB");
        isComplete++;
    }

    void InformationCSVLoad(TextAsset _csv)
    {
        var lines = Regex.Split(_csv.text, CSVReader.LINE_SPLIT_RE);

        List<string> informations;
        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], CSVReader.SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;
            informations = new List<string>();
            for (int j = 3; j < values.Length; j++)
            {
                if (values[j] == "") break;
                informations.Add(CSVReader.GetStringData(values[j]));
            }
            InfoDB.Add(CSVReader.GetIntData(values[0]), new SkillInfomation(CSVReader.GetStringData(values[1]), CSVReader.GetStringData(values[2]), informations));
        }
        AddressableManager.Instance.ReleaseAsset("SkillInformationDB");
        isComplete++;
    }
}
