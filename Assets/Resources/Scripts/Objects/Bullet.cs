using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Transform hitTransform = collision.transform;
        if (hitTransform.CompareTag("Player"))
        {
            if (hitTransform.GetComponent<IDamageable>() is PlayerController player)
                player.HealthChanged(UnityEngine.Random.Range(5f, 10f));
        }

        Destroy(gameObject);
    }
}
