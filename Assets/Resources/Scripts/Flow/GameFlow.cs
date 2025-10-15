public class GameFlow : IManagable
{
    private static GameFlow Instance = null;
    public static GameFlow instance { get { return Instance == null ? Instance = new GameFlow() : Instance; } }

    public void Initialize()
    {
        PlayerManager.instance.Initialize();
        CameraManager.instance.Initialize();
        PossessionManager.instance.Initialize();
        PathManager.instance.Initialize();
        EnemyManager.instance.Initialize();
        InputManager.instance.Initialize();
        BulletManager.instance.Initialize();
        EntityManager.instance.Initialize();
    }

    public void PostInitialize()
    {
        PlayerManager.instance.PostInitialize();
        CameraManager.instance.PostInitialize();
        PossessionManager.instance.PostInitialize();
        PathManager.instance.PostInitialize();
        EnemyManager.instance.PostInitialize();
        InputManager.instance.PostInitialize();
        BulletManager.instance.PostInitialize();
        EntityManager.instance.PostInitialize();
    }

    public void Refresh(float deltaTime)
    {
        PlayerManager.instance.Refresh(deltaTime);
        CameraManager.instance.Refresh(deltaTime);
        PossessionManager.instance.Refresh(deltaTime);
        PathManager.instance.Refresh(deltaTime);
        EnemyManager.instance.Refresh(deltaTime);
        InputManager.instance.Refresh(deltaTime);
        BulletManager.instance.Refresh(deltaTime);
        EntityManager.instance.Refresh(deltaTime);
    }

    public void PhysicsRefresh(float fixedDeltaTime)
    {
        PlayerManager.instance.PhysicsRefresh(fixedDeltaTime);
        CameraManager.instance.PhysicsRefresh(fixedDeltaTime);
        PossessionManager.instance.PhysicsRefresh(fixedDeltaTime);
        PathManager.instance.PhysicsRefresh(fixedDeltaTime);
        EnemyManager.instance.PhysicsRefresh(fixedDeltaTime);
        InputManager.instance.PhysicsRefresh(fixedDeltaTime);
        BulletManager.instance.PhysicsRefresh(fixedDeltaTime);
        EntityManager.instance.PhysicsRefresh(fixedDeltaTime);
    }

    public void LateRefresh(float deltaTime)
    {
        PlayerManager.instance.LateRefresh(deltaTime);
        CameraManager.instance.LateRefresh(deltaTime);
        PossessionManager.instance.LateRefresh(deltaTime);
        PathManager.instance.LateRefresh(deltaTime);
        EnemyManager.instance.LateRefresh(deltaTime);
        InputManager.instance.LateRefresh(deltaTime);
        BulletManager.instance.LateRefresh(deltaTime);
        EntityManager.instance.LateRefresh(deltaTime);
    }

    public void OnDemolish()
    {
        PlayerManager.instance.OnDemolish();
        CameraManager.instance.OnDemolish(); 
        PossessionManager.instance.OnDemolish();
        PathManager.instance.OnDemolish();
        EnemyManager.instance.OnDemolish();
        InputManager.instance.OnDemolish();
        BulletManager.instance.OnDemolish();
        EntityManager.instance.OnDemolish();
    }
}
