using UnityEngine;

public class LoaderCallback : MonoBehaviour
{   
    bool isFirstFrame = true;

    private void Update()
    {
        if (isFirstFrame)
        {
            Time.timeScale = 1.0f;
            isFirstFrame = false;
            StartCoroutine(Loader.LoadTargetSceneAsync());
        }
    }
}
