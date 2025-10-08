using System.Collections.Generic;
using UnityEngine;

public class EntityManager : IManagable
{
    private static EntityManager Instance;
    public static EntityManager instance { get { return Instance == null ? Instance = new EntityManager() : Instance; } }

    private List<Entity> entityList = new List<Entity>();
    private Transform entityManagerTransform;

    public void Initialize()
    {
        entityManagerTransform = new GameObject("EntityManager").transform;

        SetEntityParent();
    }

    public void LateRefresh(float deltaTime)
    {

    }

    public void OnDemolish()
    {

    }

    public void PhysicsRefresh(float fixedDeltaTime)
    {

    }

    public void PostInitialize()
    {

    }

    public void Refresh(float deltaTime)
    {

    }

    private void SetEntityParent()
    {
        entityList.AddRange(GameObject.FindObjectsOfType<Entity>());

        foreach (Entity entity in entityList)
        {
            //entity.transform.SetParent(entityManagerTransform);
        }

    }
}
