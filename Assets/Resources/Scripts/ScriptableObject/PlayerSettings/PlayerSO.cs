using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO")]
public class PlayerSO : ScriptableObject
{
    public float RaycastHitDistance = 40.0f;
    public float maxHealth = 100f;
    public float health;
}
