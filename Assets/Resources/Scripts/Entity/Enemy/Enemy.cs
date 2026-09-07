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

    [SerializeField] private HealthUI healthUI;
    [SerializeField] private CameraSceneVolumeProfileSO enemyVolumeProfileSO; // using the default for now.
    [SerializeField] private Transform gunBarrel;
    [SerializeField] private EnemySO enemySO;

    private EnemyAnimator enemyAnimator;
    private NavMeshAgent enemyAgent;
    private StateMachine stateMachine;
    private EnemyAnimationController enemyAI;

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
        rb = GetComponent<Rigidbody>();

        enemyAI = new EnemyAnimationController(this);
        enemyAnimator = new EnemyAnimator(this);

        defaultVelocity = enemyAgent.velocity;
    }

    public void PostInitialize()
    {
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
            {typeof(SuspicionState), new SuspicionState(this) },
            {typeof(PossessedState), new PossessedState(this) },
        };

        stateMachine.Initialise(this, statesDictionary);
    }

    public void Refresh(float deltaTime)
    {
        stateMachine.Refresh(deltaTime);
    }

    public void Shoot()
    {
        shootDirection = (GetTargetPlayerTransform().position + Vector3.up * (UnityEngine.Random.Range(1f, 1.5f)) - GetGunBarrelTransform().position).normalized;
        onShoot?.Invoke(this, new OnShootEventArgs { _entity = this, _direction = shootDirection, _gunBarrel = gunBarrel });
    }

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

    public float GetHealth() => healthUI.GetHealth();

    public float GetMaxHealth() => enemySO.maxHealth;

    public bool IsSafe() => true;

    public Entity GetPossessedEntity() => this;

    public override Transform GetCameraAttachPoint() => cameraAttachPoint;

    public override Transform GetTargetLockTransform() => targetLockerPoint;

    public override EntityAnimation GetEntityAnimation() => entityAnimation;

    public override float GetEntityPossessionTimerMax() => entitySO.entityPossessionTimerMax;

    public override float GetPossessionCooldownTimerMax() => entitySO.possessionCooldownTimerMax;

    public EnemyAnimator GetAnimator() => enemyAnimator;

    public NavMeshAgent GetEnemyAgent() => enemyAgent;

    public Transform GetGunBarrelTransform() => gunBarrel;

    public Transform GetTargetPlayerTransform() => targetTransform;

    public EnemySO GetEnemySO() => enemySO;

    public StateMachine GetStateMachine() => stateMachine;

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

    // Dual-risk detection: two independent checks per frame, each feeding a different
    // state branch (SuspicionState for the animal, AttackState for the abandoned body).
    public bool CanSeePossessedAnimal()
    {
        Transform target = PossessionManager.instance.GetActiveTransform();
        if (target == null || target == PlayerManager.instance.GetPlayer().transform) return false; // player isn't possessing an animal right now
        return HasSightTo(target, enemySO.sightDistance, enemySO.animalFieldOfView);
    }

    public bool CanSeePossessedPlayer()
    {
        Transform body = PlayerManager.instance.GetPlayer().transform;
        if (PossessionManager.instance.GetActiveTransform() == body) return false; // player is currently the human -- no "abandoned" body to spot

        bool seen = HasSightTo(body, enemySO.sightDistance, enemySO.bodyFieldOfView);
        if (seen)
        {
            targetTransform = body;
            targetsLastPosition = body.position;
        }
        return seen;
    }

    private bool HasSightTo(Transform target, float sightDistance, float fov)
    {
        if (Vector3.Distance(transform.position, target.position) > sightDistance) return false;
        float angle = Vector3.Angle(target.position - transform.position, transform.forward);
        if (angle > fov) return false;
        Ray ray = new Ray(transform.position + Vector3.up * enemySO.eyeHeight, (target.position - transform.position));
        return Physics.Raycast(ray, out RaycastHit hit, sightDistance, enemySO.targetLayerMask) && hit.transform == target;
    }

    public void ReceiveAlert(Vector3 lastKnownPosition)
    {
        targetsLastPosition = lastKnownPosition;
        stateMachine.ChangeState(new SearchState(this));
    }

    public virtual void ApplySettings(StateSettings settings)
    {
        enemyAI.RunAI(settings);
    }

    public virtual void ResetChanges()
    {
        enemyAI.Reset();
    }

    public EntityAnimation GetAnimationEntity()
    {
        return entityAnimation;
    }

    public override Rigidbody GetRigidBody() => rb;
}