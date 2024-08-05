using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using System.Linq;

public class DownloadManager : MonoBehaviour
{
    [SerializeField] List<string> Labels = new List<string>();
    [SerializeField] DownloadUI downloadUI;
    [SerializeField] DownloadCheckUI checkUI;
    [SerializeField] GameObject startButton;

    long patchSize;
    Dictionary<string,long> patchDictionary = new Dictionary<string,long>();
    // Start is called before the first frame update
    async void Start()
    {
        startButton.SetActive(false);
        downloadUI.Close();
        checkUI.Close();
        await InitAddressable();
    }
    async UniTask InitAddressable()
    {
        Debug.Log("Init");
        var init = Addressables.InitializeAsync();
        await init;
        startButton.SetActive(true);
        Debug.Log("Init comp");
    }
    public void GameStart()
    {
        GetCheckDownloadSize();
    }
    async void GetCheckDownloadSize()
    {
        patchSize = 0;

        foreach(var label in Labels)
        {
            Debug.Log(label);
            var handle = Addressables.GetDownloadSizeAsync(label);
            await handle;
            patchSize += handle.Result;
            Debug.Log(patchSize.ToString());
        }

        if(patchSize > decimal.Zero)
        {
            checkUI.Open();
            checkUI.SetSizeText(patchSize);
        }
        else
        {
            Debug.Log("게임 시작");
            CustomSceneManager.Instance.LoadStartLoadingScene();
        }
    }
    public void DownloadStart()
    {
        checkUI.Close();
        downloadUI.Open();
        PatchFiles();
    }
    public static string SetFileSizeText(long _size)
    {
        string text;
        if (_size >= 1073741824.0)
        {
            text = $"{string.Format("{0:##.## GB}", _size / 1073741824.0)}";
        }
        else if (_size >= 1048576.0)
        {
            text = $"{string.Format("{0:##.## MB}", _size / 1048576.0)}";
        }
        else if (_size >= 1024.0)
        {
            text = string.Format("{0:##.## KB}", _size / 1024.0);
        }
        else
        {
            text = $"{_size} Bytes";
        }
        return text;
    }
    async void PatchFiles()
    {
        foreach (var label in Labels)
        {
            var handle = Addressables.GetDownloadSizeAsync(label);
            await handle;

            if (handle.Result > decimal.Zero)
            {
                DownLoadLabel(label);
            }
        }
        CheckDownLoad();
    }
    async void DownLoadLabel(string _label)
    {
        patchDictionary.Add(_label, 0);

        var handle = Addressables.DownloadDependenciesAsync(_label, false);

        while(!handle.IsDone)
        {
            patchDictionary[_label] = handle.GetDownloadStatus().DownloadedBytes;
            await UniTask.Yield();
        }

        patchDictionary[_label] = handle.GetDownloadStatus().TotalBytes;
        Addressables.Release(handle);
    }
    async void CheckDownLoad()
    {
        bool isComplete = false;
        float total = 0f;
        downloadUI.SetPercentageText(0);
        downloadUI.SetTotalSize(patchSize);

        while (!isComplete)
        {
            total += patchDictionary.Sum(value => value.Value);

            float percentage = total / patchSize;
            downloadUI.SetSlider(percentage);
            downloadUI.SetPercentageText((int)(percentage * 100));
            downloadUI.SetSizeInfoText(total);

            if(total == patchSize)
            {
                isComplete = true;
            }
            total = 0f;
            await UniTask.Yield();
        }
        //패치완료
        CustomSceneManager.Instance.LoadStartLoadingScene();
    }
    #region CustomEditor
    public void AddLable(string _label)
    {
        Labels.Add(_label);
    }
    public void ClearLable()
    {
        Labels.Clear();
    }
    #endregion
}
