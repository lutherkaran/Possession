using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Transform hitTransform = collision.transform;
        string tag = hitTransform.transform.gameObject.tag;

        if(hitTransform.TryGetComponent<IDamageable>(out IDamageable damagable))
        {
            damagable.HealthChanged(UnityEngine.Random.Range(-5f, -10f));
        }

        //if (hitTransform.transform.CompareTag("Npc"))
        //{
        //    //Debug.Log("Hiittt to NPC: ");
        //}
        //else if ((hitTransform.transform.CompareTag("Enemy")))
        //{
        //    if (hitTransform.GetComponent<IDamageable>() is Enemy enemy)
        //        enemy.HealthChanged(UnityEngine.Random.Range(-5f, -10f));
        //}

        //if (hitTransform.CompareTag("Player"))
        //{
        //    if (hitTransform.GetComponent<IDamageable>() is PlayerController player)
        //        player.HealthChanged(UnityEngine.Random.Range(-5f, -10f));
        //}

        Destroy(this.gameObject);
    }
}
