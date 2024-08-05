using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CooltimeData
{
    public float currentCooltime;
    public float cooltime;
    public CooltimeData(float _cooltime)
    {
        cooltime = _cooltime;
        currentCooltime = _cooltime;
    }
    public void Update()
    {
        currentCooltime -= Time.deltaTime;
    }
    public bool IsNotCooltime()
    {
        return currentCooltime <= 0;
    }
}
public class CooltimeManager : SingletonBehaviour<CooltimeManager>
{
    private Dictionary<int, CooltimeData> cooltimeDatas = new Dictionary<int, CooltimeData>();
    private Queue<int> removeKeyQueue = new Queue<int>();
    List<int> keys = new List<int>(32);
    private void Update()
    {
        keys.Clear();
        keys.AddRange(cooltimeDatas.Keys);
        foreach(var key in keys)
        {
            var data = cooltimeDatas[key];
            data.Update();
            cooltimeDatas[key] = data;
            if (cooltimeDatas[key].IsNotCooltime())
            {
                removeKeyQueue.Enqueue(key);
            }
        }
        while(removeKeyQueue.Count > 0)
        {
            var key = removeKeyQueue.Dequeue();
            cooltimeDatas.Remove(key);
        }
    }
    public void AddCooltime(int _id, float _cooltime)
    {
        cooltimeDatas.Add(_id, new (_cooltime));
    }
    public bool IsCooltime(int _id)
    {
        return cooltimeDatas.ContainsKey(_id);
    }
    public float GetCooltimeProgress(int _id)
    {
        if (cooltimeDatas.TryGetValue(_id, out CooltimeData data))
        {
            return Mathf.Clamp01(data.currentCooltime / data.cooltime);
        }
        return 0f;
    }
}
