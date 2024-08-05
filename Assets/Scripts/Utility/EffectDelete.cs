using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDelete : MonoBehaviour
{
    [SerializeField] int deleteMiliseconds;
    private async void OnEnable()
    {
        await delete();
    }
    async UniTask delete()
    {
        await UniTask.Delay(deleteMiliseconds);
        gameObject.SetActive(false);
    }
}
