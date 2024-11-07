using System;
using UnityEngine;

public class PlayerController : Entity, IPossessible
{
    [SerializeField] bool isAlive = true;

    private CharacterController characterController;
    private bool canPossess = true; // It's a player that's currently possessed and it can possess other Entity.

    public IPossessible currentPossession;

    private GameObject Go;
    private void Start()
    {
        isAlive = true;
        characterController = GetComponent<CharacterController>();
        playerPossessed = currentPossession = PossessionManager.ToPossess(this);
        CameraManager.instance.Initialize(this.gameObject);
        SetPlayer(this);
    }

    public void Update()
    {
        isGrounded = characterController.isGrounded;
    }

    // To jump the player.
    public override void ProcessJump()
    {
        base.ProcessJump();
    }

    // To sprint the player.
    public override void Sprint()
    {
        base.Sprint();
    }

    // To move the player.
    public override void ProcessMove(Vector2 Input)
    {
        base.ProcessMove(Input);
        characterController.Move(transform.TransformDirection(moveDirection) * base.speed * Time.deltaTime);
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

    // To possess the entities by shooting a raycast.
    public void PossessEntities()
    {
        if (canPossess)
        {
            Ray ray = new Ray(transform.position + (Vector3.up * .5f), transform.forward);
            Debug.DrawRay(ray.origin, transform.forward * 20);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 20f))
            {
                var PossessableEntity = hit.transform.gameObject.GetComponentInParent<IPossessible>();
                Go = hit.transform.GetComponentInParent<Entity>().gameObject;
                Debug.LogWarning(": " + hit.transform.gameObject.name);
                if (PossessableEntity != null)
                {
                    PossessableEntity.Possess(Go);
                    Debug.LogWarning("Hello");
                    currentPossession = PossessableEntity;
                    playerPossessed = null;
                    canPossess = !canPossess;
                }
                // Can't possess that object.
            }

            else
            {
                // Please go near to that object.
                if (Go != null)
                {
                    currentPossession.Depossess(Go);
                    playerPossessed = currentPossession = PossessionManager.ToPossess(this);
                }
            }

        }

        else
        {
            if (Go != null)
            {
                canPossess = true;
                currentPossession.Depossess(Go);
                playerPossessed = currentPossession = PossessionManager.ToPossess(this);
            }
        }
    }

    public override void Attack()
    {

    }

    public void Possess(GameObject go)
    {
        Debug.Log("Possessing..." + go.name);
        playerPossessed = PossessionManager.ToPossess(go.GetComponent<IPossessible>());
    }

    public void Depossess(GameObject go)
    {
        Debug.Log("DePossessing..." + go.name);
        PossessionManager.ToDepossess();
        playerPossessed = null;
    }

    public Entity GetEntity()
    {
        return this;
    }
}
