using System.Collections.Generic;
using UnityEngine;

public class EntityManager : IManagable
{
    private static EntityManager Instance;
    public static EntityManager instance { get { return Instance == null ? Instance = new EntityManager() : Instance; } }

    private List<Entity> entityList = new List<Entity>();
    private Transform entityManagerTransform;

    public Vector3 playerSpawnLocation { get; private set; } = Vector3.zero;

    public void Initialize()
    {
        entityManagerTransform = new GameObject("EntityManager").transform;

        GameObject spawnMarker = GameObject.Find("PlayerSpawnLocation");
        if (spawnMarker != null)
        {
            playerSpawnLocation = spawnMarker.transform.position;
        }
        else
        {
            // Was an unguarded GameObject.Find(...).transform.position -- a renamed/disabled/
            // missing marker in the scene would NRE on the very first frame with no useful
            // error message. Now fails loud with a clear cause instead of a bare NRE.
            Debug.LogError("EntityManager: 'PlayerSpawnLocation' GameObject not found in the scene -- defaulting player spawn to Vector3.zero. Add a GameObject named exactly 'PlayerSpawnLocation' to the scene.");
            playerSpawnLocation = Vector3.zero;
        }
    }

    public void LateRefresh(float deltaTime)
    {

    }

    public void OnDemolish()
    {
        Instance = null;
    }

    public void PhysicsRefresh(float fixedDeltaTime)
    {

    }

    public void PostInitialize()
    {
        SetEntityParent();
    }

    public void Refresh(float deltaTime)
    {

    }

    private void SetEntityParent()
    {
        entityList.AddRange(GameObject.FindObjectsByType<Entity>(FindObjectsSortMode.None));

        foreach (Entity entity in entityList)
        {
            entity.transform.SetParent(entityManagerTransform);
        }
    }
}