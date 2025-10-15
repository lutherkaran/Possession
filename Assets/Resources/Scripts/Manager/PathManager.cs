using System.Collections.Generic;
using UnityEngine;

public class PathManager : IManagable
{
    private static PathManager Instance;

    public static PathManager instance
    {
        get { return Instance == null ? Instance = new PathManager() : Instance; }
    }

    public List<EnemyPath> pathList { get; private set; }
    
    public void Initialize()
    {
        pathList = new List<EnemyPath>();

        Transform pathManager = new GameObject("PathManager").transform;
        pathList.AddRange(GameObject.FindObjectsByType<EnemyPath>(FindObjectsSortMode.None));
        
        foreach (EnemyPath path in pathList)
        {
            path.transform.SetParent(pathManager);
        }
    }

    public void PostInitialize()
    {
    }

    public void Refresh(float deltaTime)
    {
    }

    public void PhysicsRefresh(float fixedDeltaTime)
    {
    }

    public void LateRefresh(float deltaTime)
    {
    }

    public void OnDemolish()
    {
        Instance = null;
    }
}