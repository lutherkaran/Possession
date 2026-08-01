using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected IPossessable possessedByPlayer { get; set; }

    [SerializeField] protected EntitySO entitySO;
    [SerializeField] protected Transform cameraAttachPoint;
    [SerializeField] protected Transform targetLockerPoint;
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
        Vector3 localMoveDir = new Vector3(input.x, 0, input.y).normalized;
        moveDir = transform.TransformDirection(localMoveDir);
        currentSpeed = sprinting ? entitySO.maxSpeed : entitySO.speed;

        if (rb != null && !rb.isKinematic)
        {
            rb.MovePosition(rb.position + moveDir * currentSpeed * Time.fixedDeltaTime);

            // Only cancel velocity when there's no input, to stop lingering post-collision
            // drift once the player lets go of movement. Doing this unconditionally on every
            // FixedUpdate (even while actively moving) was fighting MovePosition's own physics
            // step every single frame -- that's what caused the visible vibration while moving,
            // especially noticeable strafing (no forward momentum to mask it).
            if (localMoveDir.sqrMagnitude < 0.0001f)
            {
                Vector3 v = rb.linearVelocity;
                rb.linearVelocity = new Vector3(0f, v.y, 0f);
            }
        }
        else
        {
            transform.Translate(localMoveDir * currentSpeed * Time.fixedDeltaTime, Space.Self);
        }
    }

    public virtual void StopPhysicsMovement()
    {
        if (rb != null && !rb.isKinematic)
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        }
    }

    public abstract Transform GetCameraAttachPoint();
    public abstract Transform GetTargetLockTransform();
    public abstract EntityAnimation GetEntityAnimation();
    public abstract Rigidbody GetRigidBody();

    public abstract float GetEntityPossessionTimerMax();
    public abstract float GetPossessionCooldownTimerMax();
}