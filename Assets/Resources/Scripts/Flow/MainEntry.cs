using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainEntry : MonoBehaviour
{
    private GameFlow gameFlow;

    void Awake()
    {
        SettingQuality();

        if (IsGameScene())
        {
            var managers = new List<IManagable>
            {
                CameraManager.instance,
                PathManager.instance,
                EnemyManager.instance,
                BulletManager.instance,
                EntityManager.instance,
                PlayerManager.instance,
                NpcManager.instance,
                InputManager.instance,
                PossessionManager.instance
            };

            gameFlow = new GameFlow(managers);
            gameFlow.Initialize();
        }
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
            gameFlow.PostInitialize();
    }

    void Update()
    {
        if (IsGameScene())
            gameFlow.Refresh(Time.deltaTime);
    }

    void FixedUpdate()
    {
        if (IsGameScene())
            gameFlow.PhysicsRefresh(Time.fixedDeltaTime);
    }

    void LateUpdate()
    {
        if (IsGameScene())
            gameFlow.LateRefresh(Time.deltaTime);
    }

    private void OnDestroy()
    {
        if (IsGameScene())
            gameFlow.OnDemolish();
    }

    private bool IsGameScene()
    {
        return SceneManager.GetActiveScene().name == Loader.Scene.BEGINNING.ToString();
    }
}
