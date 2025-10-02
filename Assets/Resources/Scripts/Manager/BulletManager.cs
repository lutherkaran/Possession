using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager instance { get; private set; }

    [SerializeField] private float bulletSpeed = 25f;

    private Entity attackingEntity = null;

    private Rigidbody rb;
    private GameObject bulletPrefab;

    private Vector3 spreadDirection = Vector3.zero;

    private void Awake()
    {
        bulletPrefab = Resources.Load("Prefabs/Others/Bullet") as GameObject;

        if (instance == null)
        {
            instance = this;
        }
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

        rb = bulletGameObject.GetComponentInChildren<Rigidbody>();
        rb.velocity = spreadDirection * bulletSpeed;
    }

    private GameObject InstantiateBullet(Transform gunBarrel)
    {
        return GameObject.Instantiate(bulletPrefab, gunBarrel.position, Quaternion.identity);
    }
}