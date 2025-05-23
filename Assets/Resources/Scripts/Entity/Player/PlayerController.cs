using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(InputManager), typeof(CharacterController))]

public class PlayerController : Entity, IPossessable, IDamageable
{
    [SerializeField] private bool isAlive = true;
    [SerializeField] private HealthUI healthUI;

    private CharacterController characterController;
    private InputManager inputManager;

    public float RaycastHitDistance = 40.0f;

    [SerializeField] private Transform gunBarrel;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float health;

    public event EventHandler<IDamageable.OnDamagedEventArgs> OnDamaged;

    private void Awake()
    {
        health = maxHealth;
        characterController = GetComponent<CharacterController>();
        inputManager = GetComponent<InputManager>();
        SetPlayer(this);
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

        Ray ray = DrawRayFromCrosshair();

        if (Physics.Raycast(ray, out RaycastHit hit, RaycastHitDistance))
        {
            Vector3 shootDirection = (hit.point - gunBarrel.position).normalized;
            
            Bullet.Shoot(playerController, hit, gunBarrel.transform, shootDirection);
        }
    }

    public Ray DrawRayFromCrosshair()
    {
        Ray ray = CameraManager.instance.cam.ScreenPointToRay(PlayerUI.Instance.GetCrosshairTransform().position);
        return ray;
    }

    public void Possessing(GameObject go)
    {
        //Debug.Log($"Possessing... {go.name}");
        possessedByPlayer = PossessionManager.Instance.GetCurrentPossessable();
    }

    public void Depossessing(GameObject go)
    {
        //Debug.Log($"DePossessing... {go.name}");
        possessedByPlayer = null;
    }

    public void HealthChanged(float healthChangedValue)
    {
        OnDamaged?.Invoke(this, new IDamageable.OnDamagedEventArgs { health = healthChangedValue });
        health = healthUI.GetHealth();
    }

    public override bool IsAlive() => healthUI.GetHealth() > 0;

    public float GetMaxHealth() => maxHealth;

    public Entity GetPossessedEntity() => this;

    public override Entity GetEntity() => this;

    public InputManager GetInputManager() => inputManager;

    public CharacterController GetCharacterControllerReference() => characterController;

    public override Transform GetCameraAttachPoint() => cameraAttachPoint;

}