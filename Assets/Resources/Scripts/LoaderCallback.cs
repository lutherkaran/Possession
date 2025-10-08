using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoaderCallback : MonoBehaviour
{
    [SerializeField] Image progressBar;
    [SerializeField] TextMeshProUGUI progressText;

    bool startedLoading = false;

    private void Update()
    {
        if (!startedLoading)
        {
            Time.timeScale = 1.0f;
            startedLoading = true;
            StartCoroutine(Loader.LoadTargetSceneAsync());
        }

        if (progressText != null)
            progressText.SetText("" + Loader.loadText, true);

        if (progressBar != null)
            progressBar.fillAmount = Loader.loadProgress;
    }
}
