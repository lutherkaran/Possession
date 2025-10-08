public class GameFlow : IManagable
{
    private static GameFlow Instance = null;
    public static GameFlow instance { get { return Instance == null ? Instance = new GameFlow() : Instance; } }

    public void Initialize()
    {
        PlayerManager.instance.Initialize();
        PossessionManager.instance.Initialize();
        BulletManager.instance.Initialize();
        EnemyManager.instance.Initialize();
        InputManager.instance.Initialize();
    }

    public void PostInitialize()
    {
        PlayerManager.instance.PostInitialize();
        PossessionManager.instance.PostInitialize();
        BulletManager.instance.PostInitialize();
        EnemyManager.instance.PostInitialize();
        InputManager.instance.PostInitialize();
    }

    public void Refresh(float deltaTime)
    {
        PlayerManager.instance.Refresh(deltaTime);
        PossessionManager.instance.Refresh(deltaTime);
        BulletManager.instance.Refresh(deltaTime);
        EnemyManager.instance.Refresh(deltaTime);
        InputManager.instance.Refresh(deltaTime);
    }

    public void PhysicsRefresh(float fixedDeltaTime)
    {
        PlayerManager.instance.PhysicsRefresh(fixedDeltaTime);
        PossessionManager.instance.PhysicsRefresh(fixedDeltaTime);
        BulletManager.instance.PhysicsRefresh(fixedDeltaTime);
        EnemyManager.instance.PhysicsRefresh(fixedDeltaTime);
        InputManager.instance.PhysicsRefresh(fixedDeltaTime);
    }

    public void LateRefresh(float deltaTime)
    {
        PlayerManager.instance.LateRefresh(deltaTime);
        PossessionManager.instance.LateRefresh(deltaTime);
        BulletManager.instance.LateRefresh(deltaTime);
        EnemyManager.instance.LateRefresh(deltaTime);
        InputManager.instance.LateRefresh(deltaTime);
    }

    public void OnDemolish()
    {
        PlayerManager.instance.OnDemolish();
        PossessionManager.instance.OnDemolish();
        BulletManager.instance.OnDemolish();
        EnemyManager.instance.OnDemolish();
        InputManager.instance.OnDemolish();
    }
}
