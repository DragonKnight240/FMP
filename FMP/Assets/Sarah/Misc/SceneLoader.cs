using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    public GameObject LoadingScreen;
    public Slider LoadingBar;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadNewScene(string NewScene)
    {
        if (!LoadingScreen.activeInHierarchy)
        {
            StartCoroutine(LoadScene(NewScene));
        }

        //SceneManager.LoadScene(NewScene);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator LoadScene(string SceneName)
    {
        LoadingScreen.SetActive(true);
        LoadingScreen.GetComponent<UIFade>().ToFadeIn();

        if (LoadingScreen.GetComponent<CanvasGroup>().alpha < 1)
        {
            yield return new WaitForEndOfFrame();
        }

        AsyncOperation loadScene = SceneManager.LoadSceneAsync(SceneName);

        while (!loadScene.isDone)
        {
            float Progress = Mathf.Clamp01(loadScene.progress / 0.9f);
            //print(Progress);
            LoadingBar.value = Progress;

            yield return null;
        }

        yield return new WaitForEndOfFrame();
    }
}
