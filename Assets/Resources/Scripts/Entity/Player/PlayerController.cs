using UnityEngine;

[RequireComponent(typeof(InputManager), typeof(CharacterController), typeof(PlayerHealthUI))]
[RequireComponent(typeof(PlayerInteract), typeof(PlayerUI))]

public class PlayerController : Entity, IPossessable, IDamageable
{
    [SerializeField] private bool isAlive = true;

    private CharacterController characterController;
    private IPossessable currentPossession; // The currently possessed entity.
    private InputManager inputManager;
    private PlayerHealthUI playerHealthUI;
    private Possession possession;

    private void Start()
    {
        PostInitialize();
    }

    private void PostInitialize()
    {
        SetPlayer(this);
        possession = new Possession(this);
        characterController = GetComponent<CharacterController>();
        playerHealthUI = GetComponent<PlayerHealthUI>();
        inputManager = GetComponent<InputManager>();
        currentPossession = PossessionManager.Instance?.ToPossess(this);
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

    public override void Attack()
    {
        Ray ray = possession.DrawRayFromCamera(); // Should draw from camera to viewport.
        if (Physics.Raycast(ray, out RaycastHit hit, possession.RaycastHitDistance))
        {
            if (hit.transform.parent.CompareTag("Enemy")) // if there's an object that has no parent then it will throw an exception. current example: NPC
            {
                hit.transform.GetComponentInParent<Enemy>()?.TakeDamage(UnityEngine.Random.Range(10f, 20f));
            }
        }
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

    public Possession GetPossessionReference() => possession;
}