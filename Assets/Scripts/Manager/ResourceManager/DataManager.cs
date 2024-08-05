using JetBrains.Annotations;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
[SerializeField]
public class SaveDataWorld
{
    public Vector3 playerPos;
    public string curruntMapName;
}
[SerializeField]
public class SaveDataInventory
{
    public List<int> equipmentIds = new List<int>();
    public List<int> useableIds = new List<int>();
    public List<int> useableCounts = new List<int>();
    public List<int> miscIds = new List<int>();
    public List<int> miscCounts = new List<int>();
    public long gold;
}
[SerializeField]
public class SaveDataPlayer
{
    public List<int> equipmentIds = new List<int>();
    public List<int> quickSlotIds = new List<int>();
    public int gender;
    public int playerClass;
    public int classRank;
    public int level;
    public int currentHp;
    public int currentMp;
    public int currentExp;
}
[SerializeField]
public class HuntData
{
    public int id;
    public int count;
    public HuntData(int _id, int _count)
    {
        id = _id;
        count = _count;
    }
}
[SerializeField]
public class HuntQuestData
{
    public int questId;
    public List<HuntData> huntDatas;
    public HuntQuestData(int _questId, Dictionary<int, int> _huntData)
    {
        questId = _questId;
        huntDatas = new List<HuntData>();
        foreach(var data in _huntData)
        {
            huntDatas.Add(new HuntData(data.Key, data.Value));
        }
    }
}
[SerializeField]
public class NpcTalkData
{
    public int id;
    public bool isTalk;
    public NpcTalkData(int _id, bool _isTalk)
    {
        id = _id;
        isTalk = _isTalk;
    }
}
[SerializeField]
public class TalkQuestData
{
    public int questId;
    public List<NpcTalkData> talkDatas;
    public TalkQuestData(int _questId, Dictionary<int,bool> _talkdata)
    {
        questId = _questId;
        talkDatas = new List<NpcTalkData>();
        foreach(var data in _talkdata)
        {
            talkDatas.Add(new NpcTalkData(data.Key, data.Value));
        }
    }
}
[SerializeField]
public class SaveDataQuest
{
    public List<int> completeIds = new List<int>();
    public List<int> completableIds = new List<int>();
    public List<int> inProgressIds = new List<int>();
    public List<HuntQuestData> huntQuestDatas = new List<HuntQuestData>();
    public List<TalkQuestData> talkQuestDatas = new List<TalkQuestData>();
}
[SerializeField]
public class SaveDataSkill
{
    public int skillPoint;
    public List<int> acquiredSkillIds = new List<int>();
    public List<int> acquiredSkillLevels = new List<int>();
}
public class DataManager : SingletonBehaviour<DataManager>
{
    string inventoryPath;
    string playerPath;
    string worldPath;
    string questPath;
    string skillPath;
    public SaveDataInventory inventoryData { get; private set; }
    public SaveDataPlayer playerData { get; private set; }
    public SaveDataWorld worldData { get; private set; }
    public SaveDataQuest questData { get; private set; }
    public SaveDataSkill skillData { get; private set; }
    [SerializeField] PlayerStatus playerStatus;
    protected override void Awake()
    {
        inventoryPath = $"{Application.persistentDataPath}/gamedata1.dat";
        playerPath = $"{Application.persistentDataPath}/gamedata2.dat";
        worldPath = $"{Application.persistentDataPath}/gamedata3.dat";
        questPath = $"{Application.persistentDataPath}/gamedata4.dat";
        skillPath = $"{Application.persistentDataPath}/gamedata5.dat";
        inventoryData = new SaveDataInventory();
        playerData = new SaveDataPlayer();
        worldData = new SaveDataWorld();
        questData = new SaveDataQuest();
        skillData = new SaveDataSkill();
    }
    private void SaveData<T>(T _data)
    {
        string json = JsonUtility.ToJson(_data);
        string encrypt = EncryptionUtility.Encrypt(json);

        using (StreamWriter writer = new StreamWriter(inventoryPath, false, Encoding.UTF8))
        {
            writer.Write(encrypt);
        }
    }
    public void SaveGame()
    {
        SaveInventory();
        SavePlayer();
        SaveQuest();
        SaveSkill();
        SaveWorld();
    }
    public void SaveInventory()
    {
        inventoryData.equipmentIds = InventoryData.Instance.SaveInventoryIdList(ItemType.Equipment);
        inventoryData.useableIds = InventoryData.Instance.SaveInventoryIdList(ItemType.Useable);
        inventoryData.useableCounts = InventoryData.Instance.SaveInventoryCountList(ItemType.Useable);
        inventoryData.miscIds = InventoryData.Instance.SaveInventoryIdList(ItemType.Misc);
        inventoryData.miscCounts = InventoryData.Instance.SaveInventoryCountList(ItemType.Misc);
        inventoryData.gold = InventoryData.Instance.gold;

        SaveData(inventoryData);
    }
    public void SavePlayer()
    {
        playerData.equipmentIds = EquipmentData.Instance.SaveEquipmentItems();
        playerData.quickSlotIds = QuickSlotData.Instance.SaveQuickSlots();
        playerData.gender = playerStatus.gender;
        playerData.playerClass = playerStatus.playerClass;
        playerData.classRank = playerStatus.classRank;
        playerData.level = playerStatus.Level;
        playerData.currentHp = playerStatus.Hp;
        playerData.currentMp = playerStatus.Mp;
        playerData.currentExp = playerStatus.Exp;
        SaveData(playerData);
    }
    public void SaveQuest()
    {
        QuestManager.Instance.SaveQuestData(questData);
        SaveData(questData);
    }
    public void SaveWorld()
    {
        worldData.curruntMapName = CustomSceneManager.Instance.currentMapName;
        worldData.playerPos = playerStatus.transform.position;
        SaveData(worldData);
    }
    public void SaveSkill()
    {
        skillData.skillPoint = SkillData.Instance.SkillPoint;
        skillData.acquiredSkillIds = SkillData.Instance.SaveSkillIds();
        skillData.acquiredSkillLevels = SkillData.Instance.SaveSkillLevels();
        SaveData(skillData);
    }
    public void LoadGame()
    {
        LoadInventory();
        LoadPlayer();
        LoadQuest();
        LoadWorld();
        LoadSkill();
    }
    public void LoadInventory()
    {
        if (File.Exists(inventoryPath))
        {
            string encrypt;
            using (StreamReader reader = new StreamReader(inventoryPath, Encoding.UTF8))
            {
                encrypt = reader.ReadToEnd();
            }
            string json = EncryptionUtility.Decrypt(encrypt);
            inventoryData = JsonUtility.FromJson<SaveDataInventory>(json);

            InventoryData.Instance.LoadInventoryData(inventoryData);
        }
        else
        {
            InventoryData.Instance.Init();
        }
    }
    public void LoadPlayer()
    {
        if (File.Exists(playerPath))
        {
            string encrypt;
            using (StreamReader reader = new StreamReader(playerPath, Encoding.UTF8))
            {
                encrypt = reader.ReadToEnd();
            }
            string json = EncryptionUtility.Decrypt(encrypt);
            playerData = JsonUtility.FromJson<SaveDataPlayer>(json);

            EquipmentData.Instance.LoadEquipmentItems(playerData);
            QuickSlotData.Instance.LoadQuickSlots(playerData);
            playerStatus.LoadData(playerData);
        }
        else
        {
            EquipmentData.Instance.Init();
            playerStatus.Init();
            playerStatus.ClassChange(2, 1);
        }
    }
    public async void LoadWorld()
    {
        if (File.Exists(worldPath))
        {
            string encrypt;
            using (StreamReader reader = new StreamReader(worldPath, Encoding.UTF8))
            {
                encrypt = reader.ReadToEnd();
            }
            string json = EncryptionUtility.Decrypt(encrypt);
            worldData = JsonUtility.FromJson<SaveDataWorld>(json);

            await CustomSceneManager.Instance.LoadScene(worldData.curruntMapName, worldData.playerPos);
        }
        else
        {
            await CustomSceneManager.Instance.LoadScene("Map1Scene", Vector3.zero);
        }
    }
    public void LoadQuest()
    {
        if (File.Exists(questPath))
        {
            string encrypt;
            using (StreamReader reader = new StreamReader(questPath, Encoding.UTF8))
            {
                encrypt = reader.ReadToEnd();
            }
            string json = EncryptionUtility.Decrypt(encrypt);
            questData = JsonUtility.FromJson<SaveDataQuest>(json);
            QuestManager.Instance.LoadData(questData);
        }
        else
        {
            QuestManager.Instance.SetQuestData();
        }
    }
    public void LoadSkill()
    {
        if (File.Exists(skillPath))
        {
            string encrypt;
            using (StreamReader reader = new StreamReader(skillPath, Encoding.UTF8))
            {
                encrypt = reader.ReadToEnd();
            }
            string json = EncryptionUtility.Decrypt(encrypt);
            skillData = JsonUtility.FromJson<SaveDataSkill>(json);
            SkillData.Instance.LoadData(skillData);
        }
        else
        {
            SkillData.Instance.Init();
        }
    }
}
