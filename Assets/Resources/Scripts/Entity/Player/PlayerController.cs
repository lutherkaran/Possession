using System;
using UnityEngine;

public class PlayerController : Entity, IPossessable, IDamageable
{
    public event EventHandler<IDamageable.OnDamagedEventArgs> OnDamaged;

    [SerializeField] private HealthUI healthUI;
    [SerializeField] private PlayerSO playerSO;
    [SerializeField] private CameraSceneVolumeProfileSO playerVolumeProfileSO; // using the default for now
    [SerializeField] private Transform gunBarrel;

    public bool isPossessed { get; private set; }

    public void Initialize()
    {
        playerSO.health = playerSO.maxHealth;

        healthUI = GetComponent<HealthUI>();
        rb = GetComponent<Rigidbody>();
    }

    public void PostInitialize()
    {
        PossessionManager.instance.OnPossessed += OnPlayerPossessed;

        cameraAttachPoint.localPosition = new Vector3(0, entitySO.cameraHeightAndDistance.x, entitySO.cameraHeightAndDistance.y);
        cameraAttachPoint.transform.eulerAngles = new Vector3(entitySO.cameraAngle, 0, 0);
    }

    private void OnPlayerPossessed(object sender, IPossessable e)
    {
        if (e.GetPossessedEntity() == this)
        {
            CameraManager.instance.ApplyCameraSettings(playerVolumeProfileSO.fieldOfView);
            GameManager.instance.ApplyVolumeProfile(playerVolumeProfileSO.volumeProfile);
        }
    }

    public override void MoveWhenPossessed(Vector2 input)
    {
        base.MoveWhenPossessed(input);

        float actualSpeed = moveDir.magnitude;
        entityAnimation.SetSpeed(actualSpeed);
    }

    private void Shoot()
    {
        Ray ray = DrawRayFromCrosshair();

        if (Physics.Raycast(ray, out RaycastHit hit, playerSO.RaycastHitDistance))
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

    }

    public void LateRefresh(float deltaTime)
    {

    }

    public void OnDemolish()
    {

    }

    public float GetMaxHealth() => playerSO.maxHealth;

    public PlayerController GetPlayer() => this;

    public Entity GetPossessedEntity() => this;

    public override Transform GetCameraAttachPoint() => cameraAttachPoint;

    public override Transform GetTargetLockerPoint() => targetLockerPoint;

    public override EntityAnimation GetEntityAnimation() => entityAnimation;

    public override float GetEntityPossessionTimerMax() => entitySO.entityPossessionTimerMax;

    public override float GetPossessionCooldownTimerMax() => entitySO.possessionCooldownTimerMax;

    public bool isSprinting() => sprinting;

    public override Rigidbody GetRigidBody() => rb;
}