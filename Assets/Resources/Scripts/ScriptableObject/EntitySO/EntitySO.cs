using UnityEngine;

[CreateAssetMenu(fileName = "EntitySO")]
public class EntitySO : ScriptableObject
{
    [Header("EntitySettings")]
    public float entityPossessionTimerMax = 50;
    public float possessionCooldownTimerMax = 1;
                    
    public LayerMask PossessableLayerMask;
                     
    public float speed = 2f;
    public float maxSpeed = 5f;

    [Header("Camera Settings")]
    public Vector2 cameraHeightAndDistance;

    public float cameraAngle;
    public float cameraFOV;
}
