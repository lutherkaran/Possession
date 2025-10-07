public class GameFlow : IManagable
{
    private static GameFlow Instance = null;
    public  static GameFlow instance { get { return Instance == null ? Instance = new GameFlow() : Instance; } }

    public void Initialize()
    {
        BulletManager.instance.Initialize();    
    }

    public void PostInitialize()
    {
        BulletManager.instance.PostInitialize();
    }

    public void Refresh(float deltaTime)
    {
        BulletManager.instance.Refresh(deltaTime);
    }

    public void PhysicsRefresh(float fixedDeltaTime)
    {
        BulletManager.instance.PhysicsRefresh(fixedDeltaTime);
    }

    public void LateRefresh(float deltaTime)
    {
        BulletManager.instance.LateRefresh(deltaTime);
    }
}
