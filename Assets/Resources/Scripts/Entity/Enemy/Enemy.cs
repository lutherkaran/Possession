using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity, IPossessable, IDamageable
{
    // Public Properties
    public Vector3 LastKnownPos { get; set; }
    public NavMeshAgent Agent { get; private set; }
    public GameObject Player => player.gameObject;

    // Constants
    public const string IS_IDLE = "IsIdle";
    public const string IS_PATROLLING = "IsPatrolling";
    public const string IS_SEARCHING = "IsSearching";
    public const string IS_ATTACKING = "IsAttacking";
    public const string IS_FLEEING = "IsFleeing";

    [Header("Pathfinding")]
    [SerializeField] public EnemyPath enemyPath;

    // Serialized Fields
    [Header("Sight Values")]
    public float fieldOfView = 90f;
    [SerializeField] private float sightDistance = 20f;
    [SerializeField] private float eyeHeight;

    [Header("Weapon Values")]
    public Transform gunBarrel;
    [SerializeField, Range(0.1f, 10f)] public float fireRate;

    // Private Fields
    private Rigidbody rb;
    [SerializeField] private string currentState;
    private StateMachine stateMachine;

    public Animator anim;
    public Vector3 defaultVelocity = Vector3.zero;
    public event EventHandler<float> OnEnemyHealthChanged;

    private EnemyHealthUI enemyHealthUI;

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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        enemyHealthUI = GetComponent<EnemyHealthUI>();
        health = enemyHealthUI.GetMaxHealth();
        stateMachine = GetComponent<StateMachine>();
        Agent = GetComponent<NavMeshAgent>();
    }

    private void PostInitialize()
    {
        defaultVelocity = Agent.velocity;
        stateMachine.Initialise();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (enemyHealthUI.GetHealth() >= 0)
        {
            CanSeePlayer();
            currentState = stateMachine?.activeState?.ToString() ?? "None";
        }
    }

    // Public Overrides
    public override void Attack() { /* Implement attack logic */ }

    public override bool IsAlive() => enemyHealthUI.GetHealth() > 0;

    public override void ProcessMove(Vector2 input)
    {
        base.ProcessMove(input);

        if (PossessionManager.Instance.GetCurrentPossessable() == possessedByPlayer)
        {
            Vector3 moveDirection = new Vector3(input.x, 0, input.y).normalized;
            Agent.Move(moveDirection * speed * Time.deltaTime);
        }
    }

    public override void ProcessJump()
    {
        if (PossessionManager.Instance.GetCurrentPossessable() == possessedByPlayer)
        {
            rb.AddForce(Vector3.up * -3f, ForceMode.Impulse);
        }
    }

    public override void Sprint() { base.Sprint(); }

    // IPossessible Implementation
    public void Possessing(GameObject go)
    {
        Debug.Log($"Possessing {go.name}");
        possessedByPlayer = PossessionManager.Instance.GetCurrentPossessable();
    }

    public void Depossessing(GameObject go)
    {
        Debug.Log($"DePossessing {go.name}");
    }

    // IDamageable Implementation
    public void TakeDamage(float damage)
    {
        if (enemyHealthUI.GetHealth() > 0)
        {
            enemyHealthUI.TakeDamage(damage);
            OnEnemyHealthChanged?.Invoke(this, enemyHealthUI.GetHealth());
        }
    }

    public void RestoreHealth(float healAmount)
    {
        enemyHealthUI.RestoreHealth(healAmount);
    }

    public float GetHealth() => enemyHealthUI.GetHealth();

    public bool IsSafe() => Vector3.Distance(transform.position, player.transform.position) >= 20f;

    // Sight and AI Logic
    public bool CanSeePlayer()
    {
        if (PossessionManager.Instance.GetCurrentPossessable() == possessedByPlayer) return false;

        if (player != null && Vector3.Distance(transform.position, player.transform.position) < sightDistance)
        {
            Vector3 targetDirection = player.transform.position - transform.position - (Vector3.up * eyeHeight);
            float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);

            if (angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView)
            {
                Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);
                if (Physics.Raycast(ray, out RaycastHit hitInfo, sightDistance) && hitInfo.transform.gameObject == player.gameObject)
                {
                    LastKnownPos = player.transform.position;
                    Debug.DrawRay(ray.origin, ray.direction * sightDistance, Color.red);
                    return true;
                }
            }
        }

        return false;
    }

    public Entity GetEntity() => this;
}
