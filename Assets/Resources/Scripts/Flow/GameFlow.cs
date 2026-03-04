using System.Collections.Generic;

public class GameFlow : IManagable
{
    private readonly List<IManagable> managers;

    public GameFlow(List<IManagable> managers)
    {
        this.managers = managers;
    }

    public void Initialize()
    {
        foreach (var manager in managers) 
        {
            manager.Initialize();
        }
    }

    public void PostInitialize()
    {
        foreach (var manager in managers)
        {
            manager.PostInitialize();
        }
    }

    public void Refresh(float deltaTime)
    {
        foreach (var manager in managers)
        {
            manager.Refresh(deltaTime);
        }
    }

    public void PhysicsRefresh(float fixedDeltaTime)
    {
        foreach (var manager in managers)
        {
            manager.PhysicsRefresh(fixedDeltaTime);
        }
    }

    public void LateRefresh(float deltaTime)
    {
        foreach (var manager in managers)
        {
            manager.LateRefresh(deltaTime);
        }
    }

    public void OnDemolish()
    {

        foreach (var manager in managers)
        {
            manager.OnDemolish();
        }
    }
}
