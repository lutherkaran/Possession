using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity, IPossessable, IDamageable
{

    public event EventHandler<IDamageable.OnDamagedEventArgs> OnDamaged;

    public GameObject player;

    [SerializeField] private EnemyAnimator enemyAnimator;
    [SerializeField] private HealthUI healthUI;

    private StateMachine stateMachine;

    [Header("Pathfinding Properties")]
    [SerializeField] public EnemyPath enemyPath;
    [SerializeField] public Vector3 LastKnownPos { get; set; }
    [SerializeField] public NavMeshAgent Agent { get; private set; }
    [SerializeField] public Vector3 defaultVelocity = Vector3.zero;

    [Header("Sight Properties")]
    public float fieldOfView = 90f;
    [SerializeField] private float sightDistance = 20f;
    [SerializeField] private float eyeHeight;

    [Header("Weapon Properties")]
    public Transform gunBarrel;

    [Header("State Machine")]
    [SerializeField] private string currentState;

    [Header("Health Properties")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float health;

    private Dictionary<Type, BaseState> statesDictionary;

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        PostInitialize();
        InitializeStatesDictionary();
    }

    private void Initialize()
    {
        health = maxHealth;
        Agent = GetComponent<NavMeshAgent>();
        defaultVelocity = Agent.velocity;
    }

    private void PostInitialize()
    {
        enemyAnimator = GetComponentInChildren<EnemyAnimator>();
        player = FindAnyObjectByType<PlayerController>().gameObject;
        healthUI = GetComponentInChildren<HealthUI>();
        stateMachine = GetComponent<StateMachine>();
    }

    private void InitializeStatesDictionary()
    {
        statesDictionary = new Dictionary<Type, BaseState>()
        {
            {typeof(IdleState), new IdleState(this) },
            {typeof(PatrolState), new PatrolState(this) },
            {typeof(AttackState), new AttackState(this) },
            {typeof(HealState), new HealState(this) },
            {typeof(FleeState), new FleeState(this) },
            {typeof(SearchState), new SearchState(this) },
            {typeof(PossessedState), new PossessedState(this) },
        };

        stateMachine.Initialise(this, statesDictionary);
    }

    private void Update()
    {
        currentState = stateMachine?.activeState?.ToString() ?? "None";
    }

    public override void Attack()
    {
        //stateMachine.ChangeState(new AttackState());
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
        stateMachine.ChangeState(new PossessedState(this));
    }

    public void Depossessing(GameObject go)
    {
        stateMachine.ChangeState(new IdleState(this));
    }

    public void HealthChanged(float healthChangedValue)
    {
        OnDamaged?.Invoke(this, new IDamageable.OnDamagedEventArgs { health = healthChangedValue });
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

    public EnemyAnimator GetAnimator() => enemyAnimator;

    public override float GetEntityPossessionTimerMax() => entityPossessionTimerMax;

    public override float GetPossessionCooldownTimerMax() => possessionCooldownTimerMax;
}