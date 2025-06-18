using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public IPossessable possessedByPlayer { get; set; }

    protected PlayerController playerController;

    protected Vector3 moveDirection = Vector3.zero;
    protected Vector3 velocity = Vector3.zero;

    protected float jumpHeight = 1.5f;
    protected float gravity = -9.8f;
    protected float speed = 5f;

    protected bool sprinting = false;
    protected bool isGrounded = true;


    [SerializeField] protected float entityPossessionTimerMax;
    [SerializeField] protected float possessionCooldownTimerMax;

    [SerializeField] protected Transform cameraAttachPoint;
    [SerializeField] protected LayerMask PossessableLayerMask;


    public void SetPlayer(PlayerController player)
    {
        playerController = player;
    }

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
        speed = sprinting ? 10f : 5f;
    }

    public virtual void ProcessMove(Vector2 input)
    {
        moveDirection.x = input.x;
        moveDirection.z = input.y;
    }

    public abstract void Attack();
    public abstract bool IsAlive();
    public abstract Transform GetCameraAttachPoint();
    public abstract Entity GetEntity();
    public abstract float GetEntityPossessionTimerMax();
    public abstract float GetPossessionCooldownTimerMax();
}
//TODO Any enitity should be able to use these methods
//TODO Depends which entity is currently possessed, get it's GameObject's type and then make it perform movement's actions.