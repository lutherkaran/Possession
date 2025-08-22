using UnityEngine;

public class Bullet : MonoBehaviour
{
    private static float bulletSpeed = 25f;
    private static float shotTimer = .5f;
    private static float fireRate = 3f;

    private static Rigidbody rb;
    private static Entity attackingEntity;
    private static GameObject bulletPrefab;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public static void Shoot(Entity entity, Transform gunBarrel, Vector3 direction)
    {
        bulletPrefab = Resources.Load("Prefabs/Others/Bullet") as GameObject;
        shotTimer += Time.deltaTime;

        if (shotTimer > fireRate)
        {
            Vector3 spreadDirection = Quaternion.AngleAxis(Random.Range(-3f, 3f), Vector3.up) * direction.normalized;

            GameObject.Instantiate(bulletPrefab, gunBarrel.position, Quaternion.identity);
            rb.velocity = spreadDirection * bulletSpeed;
            Debug.DrawRay(gunBarrel.position, direction * bulletSpeed, Color.red, 2f);
            shotTimer = 1;
        }
    }

    public static void Shoot(Entity entity, RaycastHit hit, Transform gunBarrel, Vector3 direction)
    {
        bulletPrefab = Resources.Load("Prefabs/Others/Bullet") as GameObject;

        attackingEntity = entity;

        Vector3 spreadDirection = Quaternion.AngleAxis(Random.Range(-3f, 3f), Vector3.up) * direction;

        GameObject.Instantiate(bulletPrefab, gunBarrel.position, Quaternion.LookRotation(spreadDirection));
        rb.velocity = /*Quaternion.AngleAxis(Random.Range(-3f, 3f), Vector3.up) **/ direction.normalized * bulletSpeed;
        Debug.DrawRay(gunBarrel.position, direction * bulletSpeed, Color.red, 2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Transform hitTransform = collision.transform;
        Debug.Log(hitTransform.name);

        if (attackingEntity is PlayerController)
        {
            string tag = hitTransform.transform.gameObject.tag;

            if (hitTransform.transform.CompareTag("Npc"))
            {
                Debug.Log("Hiittt to NPC: ");
            }
            else if (hitTransform.transform.CompareTag("Player"))
            {
                Debug.Log("Unable to Hit");
            }
            else if (hitTransform.transform.CompareTag("Enemy"))
            {
                hitTransform.transform.GetComponent<Enemy>()?.HealthChanged(UnityEngine.Random.Range(-10f, -20f));
            }
        }

        else
        {
            if (hitTransform.CompareTag("Player"))
            {
                if (hitTransform.GetComponent<IDamageable>() is PlayerController player)
                    player.HealthChanged(UnityEngine.Random.Range(-5f, -10f));
            }
        }

        Destroy(gameObject);
        attackingEntity = null;
    }
}
