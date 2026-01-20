using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public IPossessable possessedByPlayer { get; set; }

    protected PlayerController player;

    protected Vector3 moveDirection = Vector3.zero;
    protected Vector3 velocity = Vector3.zero;

    [SerializeField] protected EntitySO entitySO;

    protected float jumpHeight = 1.5f;
    protected float gravity = -9.8f;

    protected bool sprinting = false;
    protected bool isGrounded = true;
    
    [SerializeField] protected Transform cameraAttachPoint;

    public virtual void ProcessJump()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity);
        }
    }

    public virtual void Sprint()
    {
        sprinting = !sprinting;
        entitySO.speed = sprinting ? entitySO.maxSpeed : entitySO.speed;
    }

    public virtual void MoveWhenPossessed(Vector2 input)
    {
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        moveDirection.y = 0;
    }

    public abstract void Attack();

    protected abstract bool IsAlive();

    public abstract Transform GetCameraAttachPoint();
    public abstract float GetEntityPossessionTimerMax();
    public abstract float GetPossessionCooldownTimerMax();
}