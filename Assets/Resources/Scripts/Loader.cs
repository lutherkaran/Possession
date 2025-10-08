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

    public static void Load(Scene target)
    {
        targetScene = target;
        SceneManager.LoadSceneAsync(Scene.LoadingScene.ToString());
    }

    public static IEnumerator LoadTargetSceneAsync()
    {
        yield return new WaitForSeconds(0.2f);

        asyncLoad = SceneManager.LoadSceneAsync(targetScene.ToString());
        asyncLoad.allowSceneActivation = false;

        yield return new WaitForSeconds(0.3f);

        asyncLoad.allowSceneActivation = true;
    }
}
