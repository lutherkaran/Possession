using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected IPossessable possessedByPlayer { get; set; }

    [SerializeField] protected EntitySO entitySO;
    [SerializeField] protected Transform cameraAttachPoint;
    [SerializeField] protected EntityAnimation entityAnimation;

    protected Vector3 velocity = Vector3.zero; 
    protected bool sprinting = false;

    public virtual void Sprint()
    {
        sprinting = !sprinting;

        entitySO.speed = sprinting ? entitySO.maxSpeed : entitySO.speed;
    }

    public void MoveWhenPossessed(Vector2 input)
    {
        Vector3 moveDir = new Vector3(input.x, 0, input.y);

        transform.Translate(moveDir * entitySO.speed * Time.fixedDeltaTime);

        float actualSpeed = moveDir.magnitude;
        entityAnimation.SetSpeed(actualSpeed);
    }

    public abstract Transform GetCameraAttachPoint();
    public abstract EntityAnimation GetEntityAnimation();

    public abstract float GetEntityPossessionTimerMax();
    public abstract float GetPossessionCooldownTimerMax();
}