using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[RequireComponent(typeof(InputManager), typeof(CharacterController), typeof(PlayerHealthUI))]
[RequireComponent(typeof(PlayerInteract), typeof(PlayerUI))]

public class PlayerController : Entity, IPossessable, IDamageable
{
    [SerializeField] private float RaycastHitDistance = 40f;
    [SerializeField] private bool isAlive = true;
    private CharacterController characterController;
    private bool canPossess = true; // Indicates if the player can possess other entities.

    private GameObject targetEntity; // The current entity the player is interacting with.
    private IPossessable currentPossession; // The currently possessed entity.
    private InputManager inputManager;
    private PlayerHealthUI playerHealthUI;

    private void Start()
    {
        SetPlayer(this);
        characterController = GetComponent<CharacterController>();
        playerHealthUI = GetComponent<PlayerHealthUI>();
        inputManager = GetComponent<InputManager>();
        currentPossession = PossessionManager.Instance.ToPossess(this);
        CameraManager.instance?.AttachCameraToPossessedObject(gameObject);
        playerPossessed = currentPossession;
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


    private Ray DrawRayfromPlayerEye()
    {
        Ray ray = new Ray(transform.position + (Vector3.up * 0.5f), transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 40, Color.red);
        return ray;
    }

    public override void Attack()
    {
        Ray ray = DrawRayfromPlayerEye();
        if (Physics.Raycast(ray, out RaycastHit hit, RaycastHitDistance))
        {
            if (hit.transform.parent.CompareTag("Enemy")) // if there's an object that has no parent then it will throw an exception. current example: NPC
            {
                hit.transform.GetComponentInParent<Enemy>()?.TakeDamage(UnityEngine.Random.Range(10f, 20f));
            }
        }
    }

    public void PossessEntities()
    {
        if (!canPossess)
        {
            HandleDepossession();
            return;
        }

        Ray ray = DrawRayfromPlayerEye();

        if (Physics.Raycast(ray, out RaycastHit hit, RaycastHitDistance))
        {
            HandlePossession(hit);
        }
        else
        {
            HandleDepossession();
        }
    }

    private void HandlePossession(RaycastHit hit)
    {
        var possessableEntity = hit.transform.GetComponentInParent<IPossessable>();
        targetEntity = hit.transform.GetComponentInParent<Entity>()?.gameObject;

        if (possessableEntity == null) return;

        if (possessableEntity is Enemy && !IsBehindEnemy(targetEntity)) return;

        // Perform possession
        possessableEntity.Possess(targetEntity);
        StartCoroutine(CameraManager.instance.MovetoPosition(targetEntity));

        currentPossession = possessableEntity;
        playerPossessed = null;
        canPossess = false;
    }

    private bool IsBehindEnemy(GameObject enemy)
    {
        float dotProduct = Vector3.Dot(
            enemy.transform.forward.normalized,
            (transform.position - enemy.transform.position).normalized
        );

        return dotProduct < 0;
    }

    private void HandleDepossession()
    {
        if (targetEntity == null) return;

        canPossess = true;
        currentPossession.Depossess(targetEntity);
        currentPossession = playerPossessed = PossessionManager.Instance.ToPossess(this);
    }

    public void Possess(GameObject go)
    {
        Debug.Log($"Possessing... {go.name}");
        playerPossessed = PossessionManager.Instance.ToPossess(go.GetComponent<IPossessable>());
    }

    public void Depossess(GameObject go)
    {
        Debug.Log($"DePossessing... {go.name}");
        PossessionManager.Instance.ToDepossess();
        playerPossessed = null;
    }

    public override bool IsAlive() => isAlive;

    public Entity GetEntity() => this;

    public void TakeDamage(float damage) => playerHealthUI.TakeDamage(damage);

    public void RestoreHealth(float healAmount) => playerHealthUI.RestoreHealth(healAmount);

    public PlayerHealthUI GetPlayerHealthUIReference() => playerHealthUI;

    public InputManager GetInputManager() => inputManager;

    public CharacterController GetCharacterControllerReference() => characterController;
}