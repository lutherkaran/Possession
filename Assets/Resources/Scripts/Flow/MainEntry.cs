using UnityEngine;
using UnityEngine.SceneManagement;

public class MainEntry : MonoBehaviour
{
    void Awake()
    {
        if (IsGameScene())
            GameFlow.instance.Initialize();
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
        return SceneManager.GetActiveScene().name == Loader.Scene.GameScene.ToString();
    }
}
