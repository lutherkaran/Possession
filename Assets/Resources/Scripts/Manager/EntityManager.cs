using System.Collections.Generic;
using UnityEngine;

public class EntityManager : IManagable
{
    private static EntityManager Instance;
    public static EntityManager instance { get { return Instance == null ? Instance = new EntityManager() : Instance; } }

    private List<Entity> entityList = new List<Entity>();
    private Transform entityManagerTransform;

    public Vector3 playerSpawnLocation {  get; private set; } = Vector3.zero;

    public void Initialize()
    {
        entityManagerTransform = new GameObject("EntityManager").transform;
        playerSpawnLocation = GameObject.Find("PlayerSpawnLocation").transform.position;
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
