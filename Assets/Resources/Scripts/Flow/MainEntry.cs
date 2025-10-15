using UnityEngine;
using UnityEngine.SceneManagement;

public class MainEntry : MonoBehaviour
{
    void Awake()
    {
        SettingQuality();
        
        if (IsGameScene())
            GameFlow.instance.Initialize();
    }

    private void SettingQuality()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        Time.fixedDeltaTime = 1f / 60f;
    }

    void Start()
    {
        if (IsGameScene())
            GameFlow.instance.PostInitialize();
    }

    void Update()
    {
        if (IsGameScene())
            GameFlow.instance.Refresh(Time.deltaTime);
    }

    void FixedUpdate()
    {
        if (IsGameScene())
            GameFlow.instance.PhysicsRefresh(Time.fixedDeltaTime);
    }

    void LateUpdate()
    {
        if (IsGameScene())
            GameFlow.instance.LateRefresh(Time.deltaTime);
    }

    private void OnDestroy()
    {
        if (IsGameScene())
            GameFlow.instance.OnDemolish();
    }

    private bool IsGameScene()
    {
        return SceneManager.GetActiveScene().name == Loader.Scene.BEGINNING.ToString();
    }
}
