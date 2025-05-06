using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(InputManager), typeof(CharacterController))]

public class PlayerController : Entity, IPossessable, IDamageable
{
    [SerializeField] private bool isAlive = true;
    [SerializeField] private HealthUI playerHealthUI;

    private CharacterController characterController;
    private InputManager inputManager;
    public float RaycastHitDistance = 40.0f;

    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth = 100f;

    public event EventHandler<IDamageable.OnDamagedEventArgs> OnDamaged;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        inputManager = GetComponent<InputManager>();
    }

    private void Start()
    {
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
            string tag = hit.transform.gameObject.tag;

            if (hit.transform.CompareTag("Npc"))
            {
                Debug.Log("Hiittt to NPC: ");
            }
            else if (hit.transform.CompareTag("Player"))
            {
                Debug.Log("Unable to Hit");
            }
            else if (hit.transform.parent.CompareTag("Enemy"))
            {
                hit.transform.GetComponentInParent<Enemy>()?.HealthChanged(UnityEngine.Random.Range(-10f, -20f));
            }
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
        OnDamaged?.Invoke(this, new IDamageable.OnDamagedEventArgs { health = healthChangedValue, maxHealth = this.maxHealth });
    }

    public override bool IsAlive() => playerHealthUI.GetCurrentHealth()>0;

    public Entity GetEntity() => this;

    public InputManager GetInputManager() => inputManager;

    public CharacterController GetCharacterControllerReference() => characterController;
}