using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO")]
public class EnemySO : ScriptableObject
{
    [Header("Sight Properties")]
    public float sightDistance = 20f;
    public float eyeHeight;
    public LayerMask targetLayerMask;
    public float bodyFieldOfView = 90f;
    public float animalFieldOfView = 90f; 

    [Range(1f, 1.8f)]
    [SerializeField] private float targetHeight;

    [Header("Health Properties")]
    public float maxHealth = 100f;
    public float currentHealth;
}
