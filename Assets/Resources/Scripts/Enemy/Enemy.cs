using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity, IPossessible, IDamageable
{
    public IPossessible PlayerPossessed { get => playerPossessed; }

    private Rigidbody rb;
    private NavMeshAgent agent;
    private StateMachine stateMachine;

    private Vector3 lastKnownPos;

    public Vector3 LastKnownPos { get => lastKnownPos; set => lastKnownPos = value; }
    public NavMeshAgent Agent { get => agent; }
    public Animator anim;
    public const string PATROLLING = "IsPatrolling";
    public const string ATTACK = "IsAttacking";
    public const string IDLE = "Idle";
    public const string SEARCHING = "IsSearching";

    public GameObject Player { get => this.player.gameObject; }

    public EnemyPath enemyPath;

    [Header("Sight Values")]
    public float sightDistance = 20f;
    public float fieldOfView = 85f;
    public float eyeHeight;

    [Header("Weapon Values")]
    public Transform gunBarrel;
    [Range(0.1f, 10f)]
    public float fireRate;

    [SerializeField]
    private string currentState;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = this.GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        health = maxHealth;
    }

    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.Initialise();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        if (Input.GetKeyDown(KeyCode.G))
        {
            TakeDamage(Random.Range(10f, 20f));
            Debug.Log("Health Damaged: " + health);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestoreHealth(Random.Range(10f, 20f));
            Debug.Log("Health Restored: " + health);
        }

        CanSeePlayer();
        currentState = stateMachine.activeState.ToString();
    }

    public override void Attack()
    {

    }

    public override bool IsAlive()
    {
        return true;
    }

    public override void ProcessMove(Vector2 Input)
    {
        base.ProcessMove(Input);
        if (PossessionManager.currentlyPossessed == playerPossessed)
            transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    public override void ProcessJump()
    {
        base.ProcessJump();
    }

    public override void Sprint()
    {
        base.Sprint();
    }

    public bool CanSeePlayer()
    {
        if (player.gameObject != null)
        {
            if (Vector3.Distance(transform.position, player.gameObject.transform.position) < sightDistance)
            {
                Vector3 targetDirection = player.transform.position - transform.localPosition - (Vector3.up * eyeHeight);
                float angleToPlayer = Vector3.Angle(targetDirection, player.gameObject.transform.position);

                if (angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView)
                {
                    Ray ray = new Ray(gameObject.transform.position + (Vector3.up * eyeHeight), targetDirection);
                    RaycastHit hitInfo = new RaycastHit();

                    if (Physics.Raycast(ray, out hitInfo, sightDistance))
                    {
                        if (hitInfo.transform.gameObject == player.gameObject)
                        {
                            lastKnownPos = player.transform.position;
                            Debug.DrawRay(ray.origin, ray.direction * sightDistance);
                            return true;
                        }
                    }

                }
            }
        }
        return false;
    }

    public void Possess(GameObject go)
    {
        Debug.Log("Possessing..." + go.name);
        playerPossessed = PossessionManager.ToPossess(go.GetComponent<IPossessible>());
        CameraManager.instance.Initialize(this.gameObject);
    }

    public void Depossess(GameObject go)
    {
        Debug.Log("DePossessing..." + go.name);
        PossessionManager.ToDepossess();
        playerPossessed = null;
        CameraManager.instance.Initialize(player.transform.gameObject);
    }

    public Entity GetEntity()
    {
        return this;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
    }

    public float GetHealth()
    {
        return health;
    }

    public bool IsSafe()
    {
        if (Vector3.Distance(this.transform.position, player.transform.position) > 20f)
        {
            return true;
        }
        else
            return false;
    }
}
