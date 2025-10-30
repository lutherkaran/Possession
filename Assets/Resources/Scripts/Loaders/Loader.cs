using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        MENU,
        BEGINNING,
        LOADING,
        SURVIVAL
    }

    private static Scene targetScene;
    private static AsyncOperation asyncLoad;

    public static string loadText { get; private set; } = string.Empty;

    public static float loadProgress { get; private set; } = 0f;

    public static void Load(Scene target)
    {
        targetScene = target;
        SceneManager.LoadSceneAsync(Scene.LOADING.ToString());
    }

    public static IEnumerator LoadTargetSceneAsync()
    {
        loadProgress = 0;

        asyncLoad = SceneManager.LoadSceneAsync(targetScene.ToString());
        asyncLoad.allowSceneActivation = false;

        loadText = "ASSETS";

        yield return new WaitForSeconds(1);

        while (!asyncLoad.isDone)
        {
            loadProgress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            if (loadProgress >= .5f)
            {
                loadText = "ENVIRONMENT";
                yield return new WaitForSeconds(1);
            }

            if (asyncLoad.progress >= 0.9f)
            {
                loadText = targetScene.ToString();

                yield return new WaitForSeconds(1);
                asyncLoad.allowSceneActivation = true;
            }
        }

        loadProgress = 1f;
    }
}