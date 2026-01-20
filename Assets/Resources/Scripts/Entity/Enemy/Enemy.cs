using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity, IPossessable, IDamageable, IStateContext
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
    [SerializeField] private CameraSceneVolumeProfileSO enemyVolumeProfileSO; // using the default for now.
    [SerializeField] private Transform gunBarrel;
    [SerializeField] private EnemySO enemySO;

    private NavMeshAgent enemyAgent;
    private StateMachine stateMachine;
    private EnemyAI enemyAI;

    public Vector3 defaultVelocity { get; private set; }
    public Vector3 targetsLastPosition { get; private set; }
    public Vector3 shootDirection { get; private set; }

    private Dictionary<Type, BaseState> statesDictionary;

    private Transform targetTransform;
    private Transform player;

    public void Initialize()
    {
        enemySO.currentHealth = enemySO.maxHealth;
        
        enemyAgent = GetComponent<NavMeshAgent>();
        stateMachine = GetComponent<StateMachine>();

        enemyAI = new EnemyAI(this);
        defaultVelocity = enemyAgent.velocity;
    }

    public void PostInitialize()
    {
        enemyAnimator = GetComponentInChildren<EnemyAnimator>();
        healthUI = GetComponentInChildren<HealthUI>();

        PossessionManager.instance.OnPossessed += OnEnemyPossessed;

        InitializeStatesDictionary();
    }

    private void OnEnemyPossessed(object sender, IPossessable e)
    {
        if (e.GetPossessedEntity() == this)
        {
            CameraManager.instance.ApplyCameraSettings(enemyVolumeProfileSO.fieldOfView);
            GameManager.instance.ApplyVolumeProfile(enemyVolumeProfileSO.volumeProfile);
        }
    }

    private void InitializeStatesDictionary()
    {
        statesDictionary = new Dictionary<Type, BaseState>()
        {
            {typeof(IdleState), new IdleState(this) },
            {typeof(PatrolState), new PatrolState(this) },
            {typeof(AttackState), new AttackState(this) },
            {typeof(SearchState), new SearchState(this) },
            {typeof(PossessedState), new PossessedState(this) },
        };

        stateMachine.Initialise(this, statesDictionary);
    }

    public void Refresh(float deltaTime)
    {
        stateMachine.Refresh(deltaTime);
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

    public override void MoveWhenPossessed(Vector2 input)
    {
        base.MoveWhenPossessed(input);
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
        enemySO.currentHealth = healthUI.GetHealth();
    }

    protected override bool IsAlive() => healthUI.GetHealth() > 0;

    public float GetHealth() => healthUI.GetHealth();

    public float GetMaxHealth() => enemySO.maxHealth;

    public bool IsSafe() => true;

    public Entity GetPossessedEntity() => this;

    public override Transform GetCameraAttachPoint() => cameraAttachPoint;

    public override float GetEntityPossessionTimerMax() => entitySO.entityPossessionTimerMax;

    public override float GetPossessionCooldownTimerMax() => entitySO.possessionCooldownTimerMax;

    public EnemyAnimator GetAnimator() => enemyAnimator;

    public NavMeshAgent GetEnemyAgent() => enemyAgent;

    public Transform GetGunBarrelTransform() => gunBarrel;

    public Transform GetTargetPlayerTransform() => targetTransform;

    public EnemySO GetEnemySO() => enemySO;

    public void PhysicsRefresh(float fixedDeltaTime)
    {

    }

    public void LateRefresh(float deltaTime)
    {

    }

    public void OnDemolish()
    {

    }

    public NavMeshAgent GetNavMeshAgent()
    {
        return enemyAgent;
    }

    public Transform GetTransform() => transform;

    bool IStateContext.CanSeePlayer()
    {
        player = PlayerManager.instance.GetPlayer().transform;
        if (Vector3.Distance(transform.position, player.position) < enemySO.sightDistance)
        {
            Vector3 targetDirection = player.position - transform.position;
            float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);
            if (angleToPlayer >= -enemySO.fieldOfView && angleToPlayer <= enemySO.fieldOfView)
            {
                Ray ray = new Ray(transform.position + (Vector3.up * enemySO.eyeHeight), targetDirection);
                if (Physics.Raycast(ray, out RaycastHit hitInfo, enemySO.sightDistance, enemySO.targetLayerMask))
                {
                    targetTransform = hitInfo.transform;
                    targetsLastPosition = targetTransform.position;
                    Vector3.RotateTowards(transform.forward, targetDirection.normalized, 1, 2);
                    Debug.DrawRay(ray.origin, ray.direction * enemySO.sightDistance, Color.red);
                    return true;
                }
            }
        }
        return false;

    }

    void IStateContext.ApplySettings(StateSettings _settings)
    {
        enemyAI.RunAI(_settings);
    }

    void IStateContext.ResetChanges()
    {
        enemyAI.Reset();
    }

}