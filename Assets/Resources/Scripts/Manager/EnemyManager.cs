using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyManager : IManagable
{
    private static EnemyManager Instance;

    public static EnemyManager instance
    {
        get { return Instance == null ? Instance = new EnemyManager() : Instance; }
    }

    private List<Enemy> enemies = new List<Enemy>();
    private GameObject enemyPrefab;

    private float enemiesToSpawn = 4;

    public Dictionary<Enemy, EnemyPath> enemyPathEnemyDictionary { get; private set; } =
        new Dictionary<Enemy, EnemyPath>();

    public List<EnemyPath> paths { get; private set; } = new List<EnemyPath>();

    public void Initialize()
    {
        enemyPrefab = Resources.Load<GameObject>("Prefabs/Entity/Enemy");

        InstantiateEnemies();
        MapEnemiesWithPath();
        SetLocation();

        foreach (Enemy enemy in enemies)
        {
            enemy.Initialize();
        }
    }

    private void InstantiateEnemies()
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            GameObject newEnemy = GameObject.Instantiate(enemyPrefab);
            enemies.Add(newEnemy.GetComponent<Enemy>());
        }
    }

    private void MapEnemiesWithPath()
    {
        paths = PathManager.instance.pathList;
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            bool isMapped = (enemyPathEnemyDictionary.TryAdd(enemies[i], paths[i]));

            if (!isMapped)
            {
                Debug.LogError("Unable to map: " + enemies[i] + " to path: " + paths[i]);
            }
        }
    }

    private void SetLocation()
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            enemies[i].transform.position = paths[i].GetRandomPathPosition();
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

        Instance = null;
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