using System;
using UnityEngine;

public class PlayerController : Entity, IPossessible
{
    [SerializeField] private bool isAlive = true;
    private CharacterController characterController;
    private bool canPossess = true; // Indicates if the player can possess other entities.

    private GameObject targetEntity; // The current entity the player is interacting with.
    private IPossessible currentPossession; // The currently possessed entity.

    private InputManager inputManager;

    private void Start()
    {
        isAlive = true;
        characterController = GetComponent<CharacterController>();
        currentPossession = PossessionManager.ToPossess(this);
        playerPossessed = currentPossession;

        CameraManager.instance.Initialize(gameObject);
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

        characterController.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        characterController.Move(velocity * Time.deltaTime);
    }

    public override bool IsAlive()
    {
        return isAlive;
    }

    public override void Attack()
    {
        // Implement attack logic here.
    }

    /// <summary>
    /// Handles the possession of nearby entities using a raycast.
    /// </summary>
    public void PossessEntities()
    {
        if (!canPossess)
        {
            HandleDepossession();
            return;
        }

        Ray ray = new Ray(transform.position + (Vector3.up * 0.5f), transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 40, Color.red);

        if (Physics.Raycast(ray, out RaycastHit hit, 40f))
        {
            HandlePossession(hit);
        }
        else
        {
            HandleFailedPossession();
        }
    }

    private void HandlePossession(RaycastHit hit)
    {
        var possessableEntity = hit.transform.GetComponentInParent<IPossessible>();
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

        Debug.Log($"Dot Product: {dotProduct}");
        return dotProduct < 0;
    }

    private void HandleFailedPossession()
    {
        if (targetEntity == null) return;

        currentPossession.Depossess(targetEntity);
        currentPossession = playerPossessed = PossessionManager.ToPossess(this);
    }

    private void HandleDepossession()
    {
        if (targetEntity == null) return;

        canPossess = true;
        currentPossession.Depossess(targetEntity);
        currentPossession = playerPossessed = PossessionManager.ToPossess(this);
    }

    public void Possess(GameObject go)
    {
        Debug.Log($"Possessing... {go.name}");
        playerPossessed = PossessionManager.ToPossess(go.GetComponent<IPossessible>());
    }

    public void Depossess(GameObject go)
    {
        Debug.Log($"DePossessing... {go.name}");
        PossessionManager.ToDepossess();
        playerPossessed = null;
    }

    public Entity GetEntity()
    {
        return this;
    }
}