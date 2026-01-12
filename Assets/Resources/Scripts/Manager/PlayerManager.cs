using UnityEngine;

public class PlayerManager : IManagable
{
    private static PlayerManager Instance;
    public static PlayerManager instance { get { return Instance == null ? Instance = new PlayerManager() : Instance; } }

    private PlayerController player;

    public void Initialize()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        GameObject newPlayer = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Entity/Player"));
        player = newPlayer.GetComponent<PlayerController>();

        player.transform.position = EntityManager.instance.playerSpawnLocation; //Vector3.zero;
        player.transform.rotation = Quaternion.identity;

        player.Initialize();
    }

    public void LateRefresh(float deltaTime)
    {
        if (player.isAlive)
        {
            player.LateRefresh(deltaTime);
        }
    }

    public void OnDemolish()
    {
        player?.OnDemolish();
        Instance = null;
    }

    public void PhysicsRefresh(float fixedDeltaTime)
    {
        if (player.isAlive)
        {
            player.PhysicsRefresh(fixedDeltaTime);
        }
    }

    public void PostInitialize()
    {
        player.PostInitialize();
    }

    public void Refresh(float deltaTime)
    {
        player.Refresh(deltaTime);
    }

    public PlayerController GetPlayer()
    {
        return player;
    }
}
