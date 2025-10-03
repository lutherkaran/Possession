using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : Entity, IPossessable, IDamageable
{
    public event EventHandler<IDamageable.OnDamagedEventArgs> OnDamaged;

    public static PlayerController instance { get; private set; }

    private bool isAlive = true;

    public bool isPossessed { get; private set; }

    [SerializeField] private HealthUI healthUI;

    private CharacterController characterController;
    private InputManager inputManager;

    [SerializeField] private float RaycastHitDistance = 40.0f;

    [SerializeField] private Transform gunBarrel;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float health;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        health = maxHealth;
        characterController = GetComponent<CharacterController>();
        inputManager = GetComponent<InputManager>();
    }

    private void Update()
    {
        isGrounded = characterController.isGrounded;
    }

    public override void ProcessJump()
    {
        base.ProcessJump();
    }

    public override void Sprint()
    {
        base.Sprint();
    }

    public override void ProcessMove(Vector2 input)
    {
        base.ProcessMove(input);

        characterController.Move(transform.TransformDirection(new Vector3(moveDirection.x, 0, moveDirection.z)) * speed * Time.deltaTime);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1f;
        }
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
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
        Ray ray = CameraManager.instance.cam.ScreenPointToRay(PlayerUI.Instance.GetCrosshairTransform().position);
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
        health = healthUI.GetHealth();
    }

    protected override bool IsAlive() => healthUI.GetHealth() > 0;

    public float GetMaxHealth() => maxHealth;

    public PlayerController GetPlayer() => this;

    public Entity GetPossessedEntity() => this;

    public InputManager GetInputManager() => inputManager;

    public CharacterController GetCharacterControllerReference() => characterController;

    public override Transform GetCameraAttachPoint() => cameraAttachPoint;

    public override float GetEntityPossessionTimerMax() => entityPossessionTimerMax;

    public override float GetPossessionCooldownTimerMax() => possessionCooldownTimerMax;
}