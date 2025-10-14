using UnityEngine;

public class BulletManager : IManagable
{
    private static BulletManager Instance = null;

    public static BulletManager instance { get { return Instance == null ? Instance = new BulletManager() : Instance; } }

    [SerializeField] private float bulletSpeed = 25f;

    private Entity attackingEntity = null;

    private Rigidbody rb;
    private GameObject bulletPrefab;
    private Transform bulletParent;

    private Vector3 spreadDirection = Vector3.zero;

    public void Initialize()
    {
        bulletParent = new GameObject("BulletParent").transform;

        bulletPrefab = Resources.Load("Prefabs/Others/Bullet") as GameObject;
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

    public void Shoot(Entity entity, Transform gunBarrel, Vector3 direction)
    {
        attackingEntity = entity;
        SpawnBullet(gunBarrel, direction);
    }

    private void SpawnBullet(Transform gunBarrel, Vector3 direction)
    {
        GameObject bulletGameObject = InstantiateBullet(gunBarrel);

        spreadDirection = Quaternion.AngleAxis(Random.Range(-3f, 3f), Vector3.up) * direction.normalized;

        rb = bulletGameObject.GetComponent<Rigidbody>();
        rb.linearVelocity = spreadDirection * bulletSpeed;
    }

    private GameObject InstantiateBullet(Transform gunBarrel)
    {
        return GameObject.Instantiate(bulletPrefab, gunBarrel.position, Quaternion.identity, bulletParent);
    }

    public void OnDemolish()
    {
        Instance = null;
    }
}