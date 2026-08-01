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

    // Called from InputManager.PhysicsRefresh (FixedUpdate) for whichever entity is currently
    // possessed. Movement is relative to the entity's CURRENT facing (transform.forward/right) --
    // matching the existing design where MouseAim.HandleLook rotates the possessed entity's own
    // transform directly via mouse X. Using raw world-space input here (as an earlier pass did)
    // desyncs movement from what mouse-look is showing on screen -- this restores the original
    // Space.Self relationship, now going through the Rigidbody so collisions are respected.
    public virtual void MoveWhenPossessed(Vector2 input)
    {
        Vector3 localMoveDir = new Vector3(input.x, 0, input.y).normalized;
        moveDir = transform.TransformDirection(localMoveDir);
        currentSpeed = sprinting ? entitySO.maxSpeed : entitySO.speed;

        if (rb != null && !rb.isKinematic)
        {
            // MovePosition on a non-kinematic Rigidbody still gets full collision response
            // (this is Unity's documented way to script-drive a dynamic Rigidbody), unlike
            // setting linearVelocity directly, which doesn't interact as cleanly with rotation
            // being driven separately (by mouse look) on the same transform.
            rb.MovePosition(rb.position + moveDir * currentSpeed * Time.fixedDeltaTime);
        }
        else
        {
            // Kinematic (e.g. an NPC not currently possessed, or missing Rigidbody) -- keep
            // the exact original behaviour.
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