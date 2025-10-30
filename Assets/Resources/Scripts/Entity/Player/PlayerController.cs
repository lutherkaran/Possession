using System;
using UnityEngine;

public class PlayerController : Entity, IPossessable, IDamageable
{
    public event EventHandler<IDamageable.OnDamagedEventArgs> OnDamaged;

    private CharacterController characterController;
    [SerializeField] private HealthUI healthUI;

    public bool isAlive { get; private set; }
    public bool isPossessed { get; private set; }

    [SerializeField] private float RaycastHitDistance = 40.0f;
    [SerializeField] private Transform gunBarrel;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float health;

    float currentFixedDeltaTime;

    public void Initialize()
    {
        health = maxHealth;
        isAlive = true;

        characterController = GetComponent<CharacterController>();
        healthUI = GetComponent<HealthUI>();
    }

    public void PostInitialize()
    {

    }

    public override void ProcessJump()
    {
        base.ProcessJump();
    }

    public override void Sprint()
    {
        base.Sprint();
    }

    public override void MoveWhenPossessed(Vector2 input)
    {
        base.MoveWhenPossessed(input);

        moveDirection = new Vector3(input.x, 0, input.y);

        transform.Translate(moveDirection * speed * currentFixedDeltaTime);
    }

    public override void Attack()
    {
        if (this != possessedByPlayer) return;

        Shoot();
    }

    private void Shoot()
    {
        Ray ray = DrawRayFromCrosshair();

        if (Physics.Raycast(ray, out RaycastHit hit, RaycastHitDistance))
        {
            Vector3 shootDirection = (hit.point - gunBarrel.position).normalized;

            BulletManager.instance.Shoot(this, gunBarrel.transform, shootDirection);
        }
    }

    public Ray DrawRayFromCrosshair()
    {
        Ray ray = CameraManager.instance.myCamera.ScreenPointToRay(PlayerUI.Instance.GetCrosshairTransform().position);
        return ray;
    }

    public void Possessing(GameObject go)
    {
        possessedByPlayer = PossessionManager.instance.GetCurrentPossessable();
        isPossessed = true;
    }

    public void Depossessing(GameObject go)
    {
        possessedByPlayer = null;
        isPossessed = false;
    }

    public void HealthChanged(float healthChangedValue)
    {
        OnDamaged?.Invoke(this, new IDamageable.OnDamagedEventArgs { health = healthChangedValue });
        //health = healthUI.GetHealth();
    }

    public void Refresh(float deltaTime)
    {
    
    }

    public void PhysicsRefresh(float fixedDeltaTime)
    {
        currentFixedDeltaTime = fixedDeltaTime;
    }

    public void LateRefresh(float deltaTime)
    {

    }

    public void OnDemolish()
    {

    }

    protected override bool IsAlive() => healthUI.GetHealth() > 0;

    public float GetMaxHealth() => maxHealth;

    public PlayerController GetPlayer() => this;

    public Entity GetPossessedEntity() => this;

    public CharacterController GetCharacterControllerReference() => characterController;

    public override Transform GetCameraAttachPoint() => cameraAttachPoint;

    public override float GetEntityPossessionTimerMax() => entityPossessionTimerMax;

    public override float GetPossessionCooldownTimerMax() => possessionCooldownTimerMax;
}