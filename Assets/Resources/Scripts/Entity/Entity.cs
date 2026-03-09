using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected IPossessable possessedByPlayer { get; set; }

    [SerializeField] protected EntitySO entitySO;
    [SerializeField] protected Transform cameraAttachPoint;
    [SerializeField] protected EntityAnimation entityAnimation;
    
    protected Rigidbody rb;

    protected Vector3 velocity = Vector3.zero;
    protected Vector3 moveDir = Vector3.zero;

    protected bool sprinting = false;

    protected float currentSpeed = 0;

    public void ToggleSprint()
    {
        sprinting = !sprinting;
    }

    public virtual void MoveWhenPossessed(Vector2 input)
    {
        //if (!GetRigidBody()) return; // not using RigidBody for now.

        moveDir = new Vector3(input.x, 0, input.y).normalized;
        currentSpeed = sprinting ? entitySO.maxSpeed : entitySO.speed;
        //GetRigidBody().MovePosition(moveDir * currentSpeed * Time.fixedDeltaTime);

        transform.Translate(moveDir * currentSpeed * Time.fixedDeltaTime);
    }

    public abstract Transform GetCameraAttachPoint();
    public abstract EntityAnimation GetEntityAnimation();
    public abstract Rigidbody GetRigidBody();

    public abstract float GetEntityPossessionTimerMax();
    public abstract float GetPossessionCooldownTimerMax();
}