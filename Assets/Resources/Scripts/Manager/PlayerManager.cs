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

        SnapToGround(player.transform);

        player.Initialize();
    }

    // The Player previously moved via transform.Translate with no physics involved at all, so
    // gravity never applied and the exact height of the 'PlayerSpawnLocation' marker never
    // mattered. Now that Player has a real, gravity-affected Rigidbody, spawning even slightly
    // above the floor means it visibly falls/settles away from the marker the instant Play
    // starts. This casts down from the marker and snaps the player's collider bottom exactly
    // onto whatever floor is beneath it, so the marker's exact height is no longer critical.
    private void SnapToGround(Transform playerTransform)
    {
        Collider col = playerTransform.GetComponentInChildren<Collider>();
        float bottomOffset = col != null ? (playerTransform.position.y - col.bounds.min.y) : 0f;

        Vector3 castOrigin = playerTransform.position + Vector3.up * 2f;

        if (Physics.Raycast(castOrigin, Vector3.down, out RaycastHit hit, 10f))
        {
            Vector3 pos = playerTransform.position;
            pos.y = hit.point.y + bottomOffset;
            playerTransform.position = pos;
        }
        else
        {
            Debug.LogWarning("PlayerManager: no ground found beneath 'PlayerSpawnLocation' within 10m -- spawning at the marker's exact position instead. Check the marker's placement and that the floor has a (non-trigger) collider.");
        }
    }

    public void LateRefresh(float deltaTime)
    {
        player.LateRefresh(deltaTime);
    }

    public void OnDemolish()
    {
        player?.OnDemolish();
        Instance = null;
    }

    public void PhysicsRefresh(float fixedDeltaTime)
    {
        player.PhysicsRefresh(fixedDeltaTime);
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