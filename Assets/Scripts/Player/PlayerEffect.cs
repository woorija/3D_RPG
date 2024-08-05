using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem[] EffectList;
    public void PlayEffect(int _index)
    {
        EffectList[_index].gameObject.SetActive(true);
        EffectList[_index].Simulate(0, true, true);
        EffectList[_index].Play();
    }
}
