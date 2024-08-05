using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] string sceneName;
    [SerializeField] Vector3 teleportPosition;
    private async void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            await CustomSceneManager.Instance.LoadScene(sceneName,teleportPosition);
        }
    }
}
