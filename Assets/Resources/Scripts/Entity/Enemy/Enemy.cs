using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity, IPossessable, IDamageable
{
    // Constants
    public const string IS_IDLE = "IsIdle";
    public const string IS_PATROLLING = "IsPatrolling";
    public const string IS_SEARCHING = "IsSearching";
    public const string IS_ATTACKING = "IsAttacking";
    public const string IS_FLEEING = "IsFleeing";

    // Public Properties
    public GameObject player;

    [Header("Pathfinding")]
    [SerializeField] public EnemyPath enemyPath;
    [SerializeField] public Vector3 LastKnownPos { get; set; }
    [SerializeField] public NavMeshAgent Agent { get; private set; }

    // Serialized Fields
    [Header("Sight Values")]
    public float fieldOfView = 90f;
    [SerializeField] private float sightDistance = 20f;
    [SerializeField] private float eyeHeight;

    [Header("Weapon Values")]
    public Transform gunBarrel;

    // Private Fields
    private Rigidbody rb;

    [SerializeField] private HealthUI healthUI;

    [Header("State Machine")]
    [SerializeField] private string currentState;
    private StateMachine stateMachine;


    private Animator anim;
    public Vector3 defaultVelocity = Vector3.zero;
    public event EventHandler<float> OnEnemyHealthChanged;

    public event EventHandler<IDamageable.OnDamagedEventArgs> OnDamaged;

    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float health;

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        PostInitialize();
    }

    private void Initialize()
    {
        health = maxHealth;

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        Agent = GetComponent<NavMeshAgent>();
        defaultVelocity = Agent.velocity;

        stateMachine = GetComponent<StateMachine>();
        stateMachine.Initialise();

    }

    private void PostInitialize()
    {
        player = FindAnyObjectByType<PlayerController>().gameObject;
        healthUI = GetComponentInChildren<HealthUI>();
    }

    private void Update()
    {
        currentState = stateMachine?.activeState?.ToString() ?? "None";
    }

    // Public Overrides
    public override void Attack()
    {
        stateMachine.ChangeState(new AttackState());
    }

    public override void ProcessJump()
    {
        base.ProcessJump();
    }

    public override void ProcessMove(Vector2 input)
    {
        base.ProcessMove(input);
    }

    public override void Sprint() { base.Sprint(); }

    public void Possessing(GameObject go)
    {
        possessedByPlayer = PossessionManager.Instance.GetCurrentPossessable();
        stateMachine.ChangeState(new PossessedState());
    }

    public void Depossessing(GameObject go)
    {
        stateMachine.ChangeState(new IdleState());
    }

    public void HealthChanged(float healthChangedValue)
    {
        OnDamaged?.Invoke(this, new IDamageable.OnDamagedEventArgs { health = healthChangedValue });
        OnEnemyHealthChanged?.Invoke(this, healthUI.GetHealth());
        health = healthUI.GetHealth();
    }

    public bool CanSeePlayer()
    {
        if (PossessionManager.Instance.GetCurrentPossessable() == possessedByPlayer) return false;

        if (player != null && Vector3.Distance(transform.position, player.transform.position) < sightDistance)
        {
            Vector3 targetDirection = player.transform.position - transform.position;
            float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);

            if (angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView)
            {
                Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);
                if (Physics.Raycast(ray, out RaycastHit hitInfo, sightDistance) && hitInfo.transform.gameObject == player)
                {
                    LastKnownPos = player.transform.position;
                    Debug.DrawRay(ray.origin, ray.direction * sightDistance, Color.red);
                    return true;
                }
            }
        }

        return false;
    }

    public override bool IsAlive() => healthUI.GetHealth() > 0;

    public float GetHealth() => healthUI.GetHealth();

    public float GetMaxHealth() => maxHealth;

    public bool IsSafe() => Vector3.Distance(transform.position, player.transform.position) >= 20f;

    public Entity GetPossessedEntity() => this;

    public override Entity GetEntity() => this;

    public override Transform GetCameraAttachPoint() => cameraAttachPoint;

    public Animator GetAnimator()
    {
        return anim;
    }

}