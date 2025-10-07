using UnityEngine;

public class LoaderCallback : MonoBehaviour
{   
    bool isFirstFrame = true;

    private void Awake()
    {
        
    }

    private void Update()
    {
        if (isFirstFrame)
        {
            isFirstFrame = false;
            Loader.LoaderCallback();
        }
    }
}
