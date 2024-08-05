using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CustomSceneManager : SingletonBehaviour<CustomSceneManager>
{
    string loadingSceneName = "LoadingScene";
    public string currentMapName { get; private set; } = "";
    [SerializeField] Slider loadingBar;
    [SerializeField] Image fadeScreenImage;
    float fadeScreenImageAlpha = 0f;


    public UnityAction<Vector3> playerTeleportEvent;
    public void LoadStartLoadingScene()
    {
        SceneManager.LoadScene("StartLoadingScene");
    }
    public async UniTask LoadScene(string _scenename, Vector3 _pos)
    {
        if (GetSceneName().Equals(_scenename))
        {
            await FadeOutScreen();
            playerTeleportEvent.Invoke(_pos);
            await FadeInScreen();
        }
        else
        {
            GameManager.Instance.GameModeChange(GameMode.NotControllable);
            await UniTask.WhenAll(FadeOutScreen(), SceneManager.LoadSceneAsync(loadingSceneName).ToUniTask());
            loadingBar.gameObject.SetActive(true);
            var sceneLoadOperation = SceneManager.LoadSceneAsync(_scenename);
            currentMapName = _scenename;
            sceneLoadOperation.allowSceneActivation = false;

            while (!sceneLoadOperation.isDone)
            {
                float progress = Mathf.Clamp01(sceneLoadOperation.progress / 0.9f);
                loadingBar.value = progress;

                if (progress >= 1f)
                {
                    sceneLoadOperation.allowSceneActivation = true;
                }

                await UniTask.Yield(); // Yield the frame to avoid blocking
            }
            loadingBar.gameObject.SetActive(false);
            GameManager.Instance.GameModeChange(GameMode.ControllMode);
            playerTeleportEvent.Invoke(_pos);
            await FadeInScreen();
        }
    }
    public string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
    async UniTask FadeInScreen()
    {
        while(fadeScreenImageAlpha > 0f)
        {
            fadeScreenImageAlpha -= Time.deltaTime;
            fadeScreenImage.color = new Color(0, 0, 0, fadeScreenImageAlpha);
            await UniTask.Yield();
        }
    }
    async UniTask FadeOutScreen()
    {
        while (fadeScreenImageAlpha < 1f)
        {
            fadeScreenImageAlpha += Time.deltaTime;
            fadeScreenImage.color = new Color(0, 0, 0, fadeScreenImageAlpha);
            await UniTask.Yield();
        }
    }
}
