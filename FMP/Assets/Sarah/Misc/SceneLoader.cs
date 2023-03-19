using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    public GameObject LoadingScreen;

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
        StartCoroutine(LoadScene(NewScene));

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

        if (LoadingScreen.GetComponent<CanvasGroup>().alpha > 1)
        {
            yield return null;
        }

        AsyncOperation loadScene = SceneManager.LoadSceneAsync(SceneName);

        while (!loadScene.isDone)
        {
            yield return null;
        }

        yield return new WaitForEndOfFrame();
    }
}
