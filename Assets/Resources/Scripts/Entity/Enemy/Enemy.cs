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
    [SerializeField] private EnemyPath enemyPath;

    private NavMeshAgent Agent;
    private StateMachine stateMachine;

    public Vector3 defaultVelocity { get; private set; }
    public Vector3 targetLastLocation { get; private set; }

    [Header("Sight Properties")]
    public float fieldOfView = 90f;
    [SerializeField] private float sightDistance = 20f;
    [SerializeField] private float eyeHeight;

    [Header("Weapon Properties")]
    public Transform gunBarrel;

    [Header("Health Properties")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

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
        currentHealth = maxHealth;
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
        currentHealth = healthUI.GetHealth();
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
                    targetLastLocation = player.transform.position;

                    Vector3.RotateTowards(transform.forward, targetDirection.normalized, 1, 2);

                    Debug.DrawRay(ray.origin, ray.direction * sightDistance, Color.red);
                    return true;
                }
            }
        }

        return false;
    }

    protected override Entity GetEntity() => this;

    protected override bool IsAlive() => healthUI.GetHealth() > 0;

    public float GetHealth() => healthUI.GetHealth();

    public float GetMaxHealth() => maxHealth;

    public bool IsSafe() => Vector3.Distance(transform.position, player.transform.position) >= 20f;

    public Entity GetPossessedEntity() => this;

    public override Transform GetCameraAttachPoint() => cameraAttachPoint;

    public EnemyAnimator GetAnimator() => enemyAnimator;

    public override float GetEntityPossessionTimerMax() => entityPossessionTimerMax;

    public override float GetPossessionCooldownTimerMax() => possessionCooldownTimerMax;

    public NavMeshAgent GetEnemyAgent() => Agent;

    public EnemyPath GetEnemyPath() => enemyPath;
}