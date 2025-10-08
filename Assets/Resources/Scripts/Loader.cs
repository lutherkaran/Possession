using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        MainMenuScene,
        GameScene,
        LoadingScene
    }

    private static Scene targetScene;
    private static AsyncOperation asyncLoad;

    public static string loadText { get; private set; } = string.Empty;

    public static float loadProgress { get; private set; } = 0f;

    public static void Load(Scene target)
    {
        targetScene = target;
        SceneManager.LoadSceneAsync(Scene.LoadingScene.ToString());
    }

    public static IEnumerator LoadTargetSceneAsync()
    {
        asyncLoad = SceneManager.LoadSceneAsync(targetScene.ToString());
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            loadProgress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            loadText = targetScene.ToString();

            if (asyncLoad.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.3f);
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
        loadProgress = 1f;
        yield return new WaitForSeconds(0.2f);
    }
}
