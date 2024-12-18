using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity, IPossessible, IDamageable
{
    // Public Properties
    public IPossessible PlayerPossessed { get; private set; }
    public Vector3 LastKnownPos { get; set; }
    public NavMeshAgent Agent { get; private set; }
    public GameObject Player => player.gameObject;

    // Constants
    public const string PATROLLING = "IsPatrolling";
    public const string ATTACK = "IsAttacking";
    public const string IDLE = "Idle";
    public const string SEARCHING = "IsSearching";

    [Header("Pathfinding")]
    [SerializeField] public EnemyPath enemyPath;

    // Serialized Fields
    [Header("Sight Values")]
    [SerializeField] private float sightDistance = 20f;
    [SerializeField] private float fieldOfView = 85f;
    [SerializeField] private float eyeHeight;

    [Header("Weapon Values")]
    public Transform gunBarrel;
    [SerializeField, Range(0.1f, 10f)] public float fireRate;

    // Private Fields
    private Rigidbody rb;
    private StateMachine stateMachine;
    public Animator anim;
    private string currentState;

    // Initialization
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        health = maxHealth;
    }

    private void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        Agent = GetComponent<NavMeshAgent>();
        stateMachine.Initialise();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Main Update Loop
    private void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        HandleDebugInputs(); // Handles debug key inputs for testing

        if (PossessionManager.currentlyPossessed == playerPossessed)
        {
            DisableStateMachine(); // Disable FSM if possessed
        }
        else
        {
            EnableStateMachine(); // Enable FSM if not possessed
        }

        CanSeePlayer(); // Perform AI-related checks
        currentState = stateMachine?.activeState?.ToString() ?? "None";
    }

    // Public Overrides
    public override void Attack() { /* Implement attack logic */ }

    public override bool IsAlive() => health > 0;

    public override void ProcessMove(Vector2 input)
    {
        base.ProcessMove(input);

        if (PossessionManager.currentlyPossessed == playerPossessed)
        {
            Vector3 moveDirection = new Vector3(input.x, 0, input.y).normalized;
            Agent.Move(moveDirection * speed * Time.deltaTime);
        }
    }

    public override void ProcessJump()
    {
        if (PossessionManager.currentlyPossessed == playerPossessed)
        {
            rb.AddForce(Vector3.up * -3f, ForceMode.Impulse);
        }
    }

    public override void Sprint() { base.Sprint(); }

    // IPossessible Implementation
    public void Possess(GameObject go)
    {
        Debug.Log($"Possessing {go.name}");
        PlayerPossessed = PossessionManager.ToPossess(go.GetComponent<IPossessible>());
        CameraManager.instance.Initialize(gameObject);
        DisableStateMachine(); // Disable FSM when possessed
    }

    public void Depossess(GameObject go)
    {
        Debug.Log($"DePossessing {go.name}");
        PossessionManager.ToDepossess();
        PlayerPossessed = null;
        CameraManager.instance.Initialize(player.gameObject);
        EnableStateMachine(); // Re-enable FSM when depossessed
    }

    public Entity GetEntity() => this;

    // IDamageable Implementation
    public void TakeDamage(float damage) => health -= damage;

    public void RestoreHealth(float healAmount) => health += healAmount;

    public float GetHealth() => health;

    public bool IsSafe() => Vector3.Distance(transform.position, player.transform.position) > 20f;

    // Sight and AI Logic
    public bool CanSeePlayer()
    {
        if (PossessionManager.currentlyPossessed == playerPossessed) return false;

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

    // Private Helper Methods
    private void HandleDebugInputs()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            TakeDamage(Random.Range(10f, 20f));
            Debug.Log($"Health Damaged: {health}");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestoreHealth(Random.Range(10f, 20f));
            Debug.Log($"Health Restored: {health}");
        }
    }

    private void DisableStateMachine()
    {
        if (stateMachine != null && stateMachine.enabled)
        {
            stateMachine.enabled = false;
        }
    }

    private void EnableStateMachine()
    {
        if (stateMachine != null && !stateMachine.enabled)
        {
            stateMachine.enabled = true;
        }
    }
}
