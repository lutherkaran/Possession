using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected IPossessable playerPossessed { get; set; }

    [SerializeField]
    protected Camera cam;
    protected PlayerController player;

    protected Vector3 moveDirection = Vector3.zero;
    protected Vector3 velocity = Vector3.zero;

    protected float jumpHeight = 1.5f;
    protected float gravity = -9.8f;
    protected float speed = 5f;

    protected bool sprinting = false;
    protected bool isGrounded = true;

    [SerializeField] protected float health;
    [SerializeField] protected LayerMask PossessableLayerMask;

    public float maxHealth = 100f;

    public void SetPlayer(PlayerController Player)
    {
        player = Player;
    }

    public virtual void ProcessMove(Vector2 input)
    {
        moveDirection.x = input.x;
        moveDirection.z = input.y;
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

    public abstract void Attack();
    public abstract bool IsAlive();
}
//TODO Any enitity should be able to use these methods
//TODO Depends which entity is currently possessed, get it's GameObject's type and then make it perform movement's actions.