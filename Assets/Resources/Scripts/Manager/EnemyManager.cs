using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : IManagable
{
    private static EnemyManager Instance;
    public static EnemyManager instance { get { return Instance == null ? Instance = new EnemyManager() : Instance; } }


    private List<Enemy> enemies = new List<Enemy>();
    private GameObject enemyPrefab;

    private float enemiesToSpawn = 1;

    public void Initialize()
    {
        enemyPrefab = Resources.Load<GameObject>("Prefabs/Entity/Enemy");

        InstantiateEnemies();

        foreach (Enemy enemy in enemies)
        {
            enemy.Initialize();
        }
    }

    private void InstantiateEnemies()
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            GameObject newEnemy = GameObject.Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
            enemies.Add(newEnemy.GetComponent<Enemy>());
        }
    }

    public void LateRefresh(float deltaTime)
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.LateRefresh(deltaTime);
        }
    }

    public void OnDemolish()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.OnDemolish();
        }
    }

    public void PhysicsRefresh(float fixedDeltaTime)
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.PhysicsRefresh(fixedDeltaTime);
        }
    }

    public void PostInitialize()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.PostInitialize();
        }
    }

    public void Refresh(float deltaTime)
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.Refresh(deltaTime);
        }
    }
}
