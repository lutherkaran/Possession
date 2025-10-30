public interface IManagable
{
    public void Initialize();
    public void PostInitialize();
    public void Refresh(float deltaTime);
    public void PhysicsRefresh(float fixedDeltaTime);
    public void LateRefresh(float deltaTime);
    public void OnDemolish();
}
