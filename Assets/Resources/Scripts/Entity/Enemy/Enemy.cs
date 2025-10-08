using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity, IPossessable, IDamageable
{
    public event EventHandler<IDamageable.OnDamagedEventArgs> OnDamaged;

    public event EventHandler<OnShootEventArgs> onShoot;

    public class OnShootEventArgs : EventArgs
    {
        public Entity _entity;
        public Transform _gunBarrel;
        public Vector3 _direction;
    }

    [SerializeField] private EnemyAnimator enemyAnimator;
    [SerializeField] private HealthUI healthUI;
    [SerializeField] private EnemyPath enemyPath;

    private NavMeshAgent agent;
    private StateMachine stateMachine;

    public Vector3 defaultVelocity { get; private set; }
    public Vector3 targetsLastPosition { get; private set; }
    public Vector3 shootDirection { get; private set; }

    [Header("Sight Properties")]
    [SerializeField] private float sightDistance = 20f;
    [SerializeField] private float eyeHeight;
    [SerializeField] private LayerMask targetLayerMask;
    public float fieldOfView = 90f;

    [Range(1f, 1.8f)]
    [SerializeField] private float targetHeight;

    [SerializeField] private Transform gunBarrel;

    [Header("Health Properties")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    private Dictionary<Type, BaseState> statesDictionary;
    private Transform targetTransform;

    public void Initialize()
    {
        agent = GetComponent<NavMeshAgent>();

        currentHealth = maxHealth;
        defaultVelocity = agent.velocity;
    }

    public void PostInitialize()
    {
        enemyAnimator = GetComponentInChildren<EnemyAnimator>();
        healthUI = GetComponentInChildren<HealthUI>();
        stateMachine = GetComponent<StateMachine>();

        InitializeStatesDictionary();
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

    public void Refresh(float deltaTime)
    {

    }

    public void PhysicsRefresh(float fixedDeltaTime)
    {

    }

    public void LateRefresh(float deltaTime)
    {

    }

    public void OnDemolish()
    {

    }

    public override void Attack()
    {
        Shoot();
    }

    public void Shoot()
    {
        shootDirection = (GetTargetPlayerTransform().position + Vector3.up * (UnityEngine.Random.Range(1f, 1.5f)) - GetGunBarrelTransform().position).normalized;
        onShoot?.Invoke(this, new OnShootEventArgs { _entity = this, _direction = shootDirection, _gunBarrel = gunBarrel });
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
        possessedByPlayer = PossessionManager.instance.GetCurrentPossessable();
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
        if (playerController.transform != null && Vector3.Distance(transform.position, playerController.transform.position) < sightDistance)
        {
            Vector3 targetDirection = playerController.transform.position - transform.position;
            float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);

            if (angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView)
            {
                Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);

                if (Physics.Raycast(ray, out RaycastHit hitInfo, sightDistance, targetLayerMask))
                {
                    targetTransform = hitInfo.transform;
                    targetsLastPosition = targetTransform.position;

                    Vector3.RotateTowards(transform.forward, targetDirection.normalized, 1, 2);

                    Debug.DrawRay(ray.origin, ray.direction * sightDistance, Color.red);
                    return true;
                }
            }
        }

        return false;
    }

    protected override bool IsAlive() => healthUI.GetHealth() > 0;

    public float GetHealth() => healthUI.GetHealth();

    public float GetMaxHealth() => maxHealth;

    public bool IsSafe() => Vector3.Distance(transform.position, targetTransform.position) >= 20f;

    public Entity GetPossessedEntity() => this;

    public override Transform GetCameraAttachPoint() => cameraAttachPoint;

    public override float GetEntityPossessionTimerMax() => entityPossessionTimerMax;

    public override float GetPossessionCooldownTimerMax() => possessionCooldownTimerMax;

    public EnemyAnimator GetAnimator() => enemyAnimator;

    public NavMeshAgent GetEnemyAgent() => agent;

    public EnemyPath GetEnemyPath() => enemyPath;

    public Transform GetGunBarrelTransform() => gunBarrel;

    public Transform GetTargetPlayerTransform() => targetTransform;

}