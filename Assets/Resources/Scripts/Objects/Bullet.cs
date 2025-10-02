using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Transform hitTransform = collision.transform;
        Debug.Log(hitTransform.name);
        string tag = hitTransform.transform.gameObject.tag;

        if (hitTransform.transform.CompareTag("Npc"))
        {
            Debug.Log("Hiittt to NPC: ");
        }
        else if (hitTransform.transform.CompareTag("Player"))
        {
            Debug.Log("Unable to Hit");
        }
        else
        {
            if (hitTransform.GetComponent<IDamageable>() is Enemy enemy)
                enemy.HealthChanged(UnityEngine.Random.Range(-5f, -10f));

            //hitTransform.transform.GetComponent<Enemy>()?.HealthChanged(UnityEngine.Random.Range(-10f, -20f));
        }

        if (hitTransform.CompareTag("Player"))
        {
            if (hitTransform.GetComponent<IDamageable>() is PlayerController player)
                player.HealthChanged(UnityEngine.Random.Range(-5f, -10f));
        }

        Destroy(this.gameObject);
    }
}
