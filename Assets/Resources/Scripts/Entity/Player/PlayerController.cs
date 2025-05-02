using System.Collections;
using UnityEngine;

[RequireComponent(typeof(InputManager), typeof(CharacterController))]

public class PlayerController : Entity, IPossessable, IDamageable
{
    [SerializeField] private bool isAlive = true;
    [SerializeField] private PlayerHealthUI playerHealthUI;

    private CharacterController characterController;
    private InputManager inputManager;
    public float RaycastHitDistance = 40.0f;

    [SerializeField] public float health;
    public float maxHealth = 100f;

    private void Start()
    {
        PostInitialize();
    }

    private void PostInitialize()
    {
        SetPlayer(this);
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
        Ray ray = DrawRayFromCamera();
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
                hit.transform.GetComponentInParent<Enemy>()?.TakeDamage(UnityEngine.Random.Range(10f, 20f));
            }
        }
    }

    public Ray DrawRayFromCamera()
    {
        Ray ray = CameraManager.instance.cam.ScreenPointToRay(Input.mousePosition);
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

    public override bool IsAlive() => isAlive;

    public Entity GetEntity() => this;

    public void TakeDamage(float damage) => playerHealthUI.TakeDamage(damage);

    public void RestoreHealth(float healAmount) => playerHealthUI.RestoreHealth(healAmount);

    public InputManager GetInputManager() => inputManager;

    public CharacterController GetCharacterControllerReference() => characterController;

}