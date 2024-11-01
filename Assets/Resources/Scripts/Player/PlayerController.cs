using System;
using UnityEngine;

public class PlayerController : Entity, IPossessible
{
    [SerializeField] bool isAlive = true;

    //private Vector3 moveInput;
    private CharacterController characterController;
    private bool canPossess = true; // It's a player that's currently possessed and it can possess other Entity.

    public IPossessible currentPossession;
    public IPossessible PlayerPossessed;

    private void Awake()
    {
        isAlive = true;
        characterController = GetComponent<CharacterController>();
        PlayerPossessed = currentPossession = PossessionManager.ToPossess(this, this);
        SetPlayer(this);
    }

    public void Update()
    {
        if (PossessionManager.currentlyPossessed != this.currentPossession) { return; }
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
            Debug.DrawRay(ray.origin, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10f))
            {
                var PossessableEntity = hit.transform.gameObject.GetComponentInParent<IPossessible>();
                if (PossessableEntity != null)
                {
                    PossessableEntity.Possess();
                    currentPossession = null;
                    canPossess = !canPossess;
                    //OnPossessionChanged?.Invoke();
                }
            }

            else
            {

            }
        }

        else
        {
            canPossess = true;
            UnPossess();
        }
    }

    public void Possess()
    {
        currentPossession = PlayerPossessed;
        PossessionManager.ToPossess(this, this);
    }

    public void UnPossess()
    {
        PossessionManager.UnPossessEntity();
        Possess();
    }

    public override void Attack()
    {

    }
}
