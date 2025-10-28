public class GameFlow : IManagable
{
    private static GameFlow Instance = null;
    public static GameFlow instance { get { return Instance == null ? Instance = new GameFlow() : Instance; } }

    public void Initialize()
    {
        CameraManager.instance.Initialize();
        PathManager.instance.Initialize();
        EnemyManager.instance.Initialize();
        BulletManager.instance.Initialize();
        PlayerManager.instance.Initialize();
        EntityManager.instance.Initialize();
        NpcManager.instance.Initialize();
        InputManager.instance.Initialize();
        PossessionManager.instance.Initialize();
    }

    public void PostInitialize()
    {
        CameraManager.instance.PostInitialize();
        PathManager.instance.PostInitialize();
        EnemyManager.instance.PostInitialize();
        BulletManager.instance.PostInitialize();
        PlayerManager.instance.PostInitialize();
        EntityManager.instance.PostInitialize();
        NpcManager.instance.PostInitialize();
        InputManager.instance.PostInitialize();
        PossessionManager.instance.PostInitialize();
    }

    public void Refresh(float deltaTime)
    {
        CameraManager.instance.Refresh(deltaTime);
        PathManager.instance.Refresh(deltaTime);
        EnemyManager.instance.Refresh(deltaTime);
        BulletManager.instance.Refresh(deltaTime);
        PlayerManager.instance.Refresh(deltaTime);
        EntityManager.instance.Refresh(deltaTime);
        NpcManager.instance.Refresh(deltaTime);
        InputManager.instance.Refresh(deltaTime);
        PossessionManager.instance.Refresh(deltaTime);
    }

    public void PhysicsRefresh(float fixedDeltaTime)
    {
        CameraManager.instance.PhysicsRefresh(fixedDeltaTime);
        PathManager.instance.PhysicsRefresh(fixedDeltaTime);
        EnemyManager.instance.PhysicsRefresh(fixedDeltaTime);
        BulletManager.instance.PhysicsRefresh(fixedDeltaTime);
        PlayerManager.instance.PhysicsRefresh(fixedDeltaTime);
        EntityManager.instance.PhysicsRefresh(fixedDeltaTime);
        NpcManager.instance.PhysicsRefresh(fixedDeltaTime);
        InputManager.instance.PhysicsRefresh(fixedDeltaTime);
        PossessionManager.instance.PhysicsRefresh(fixedDeltaTime);
    }

    public void LateRefresh(float deltaTime)
    {
        CameraManager.instance.LateRefresh(deltaTime);
        PathManager.instance.LateRefresh(deltaTime);
        EnemyManager.instance.LateRefresh(deltaTime);
        BulletManager.instance.LateRefresh(deltaTime);
        PlayerManager.instance.LateRefresh(deltaTime);
        EntityManager.instance.LateRefresh(deltaTime);
        NpcManager.instance.LateRefresh(deltaTime);
        InputManager.instance.LateRefresh(deltaTime);
        PossessionManager.instance.LateRefresh(deltaTime);
    }

    public void OnDemolish()
    {
        CameraManager.instance.OnDemolish(); 
        PathManager.instance.OnDemolish();
        EnemyManager.instance.OnDemolish();
        BulletManager.instance.OnDemolish();
        PlayerManager.instance.OnDemolish();
        EntityManager.instance.OnDemolish();
        NpcManager.instance.OnDemolish();
        InputManager.instance.OnDemolish();
        PossessionManager.instance.OnDemolish();
    }
}
