using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager : SingletonBehaviour<AddressableManager>
{
    public Dictionary<string, AsyncOperationHandle> loadedHandles = new Dictionary<string, AsyncOperationHandle>();
    public Dictionary<string, AsyncOperationHandle> currentSceneLoadedHandles = new Dictionary<string, AsyncOperationHandle>();
    public void LoadAsset<T>(string _key,Action<T> _onComplete)
    {
        if (loadedHandles.TryGetValue(_key, out AsyncOperationHandle handle))
        {
            _onComplete?.Invoke((T)handle.Result);
            return;
        }
        Addressables.LoadAssetAsync<T>(_key).Completed += handle =>
        {
            if(handle.Status == AsyncOperationStatus.Succeeded)
            {
                if (!loadedHandles.ContainsKey(_key))
                {
                    loadedHandles.Add(_key, handle);
                }
                _onComplete?.Invoke(handle.Result);
            }
            else
            {
                Debug.Log($"{_key}:로딩실패");
            }
        };
    }
    public void ReleaseAsset(string _key)
    {
        if (loadedHandles.TryGetValue(_key, out AsyncOperationHandle handle))
        {
            Debug.Log($"Release:{_key}");
            Addressables.ReleaseInstance(handle);
            loadedHandles.Remove(_key);
        }
    }
}
