using System;
using UnityEngine;

public class Player : Entity, IPossessible, IInputHandler
{
    [SerializeField] float speed;
    [SerializeField] bool isAlive = true;

    //private Vector3 moveInput;
    private CharacterController characterController;
    private bool isGrounded = true;
    private bool sprinting = false; // To be able to sprint after pressing LShift button.
    private bool Crouching = false;
    private bool canPossess = true; // It's a player that's currently possessed and it can possess other Entity.
    private Vector3 PlayerVelocity; 

    public IPossessible currentPossession;
    public IPossessible PlayerPossessed;
    public float gravity = -9.8f;
    public float jumpHeight = 1.5f;
    public static event Action OnPossessionChanged;

    private void Awake()
    {
        isAlive = true;
        characterController = GetComponent<CharacterController>();
        PlayerPossessed = currentPossession = PossessionManager.ToPossess(this);
    }

    public void Update()
    {
        if (PossessionManager.currentlyPossessed != this.currentPossession) { return; }
        isGrounded = characterController.isGrounded;

        //Using a new Input System on Player
        //moveInput = Vector3.zero;
        //moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), .0f, Input.GetAxisRaw("Vertical")).normalized;
    }

    // To sprint the player.
    public void Sprint()
    {
        sprinting = !sprinting;
        if (sprinting) speed = 25f;
        else speed = 5f;
    }

    // To move the player.
    public void ProcessMove(Vector2 Input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = Input.x;
        moveDirection.z = Input.y;
        characterController.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        PlayerVelocity.y += gravity * Time.deltaTime;

        if (isGrounded && PlayerVelocity.y < 0)
        {
            PlayerVelocity.y = -2f;
        }

        characterController.Move(PlayerVelocity * Time.deltaTime);
    }

    // To jump the player.
    public void ProcessJump()
    {
        if (currentPossession == null) { return; }
        if (isGrounded)
        {
            PlayerVelocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity);
        }
    }

    #region InheritedFunctionsNotBeingUsed
    public override void Movement()
    {
        //transform.Translate(moveInput * speed * Time.deltaTime);
    }

    public override void Jump()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Debug.Log("Jumped" + this.name);
        //}
    }

    public override void Attack()
    {
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    Debug.Log("Attacked" + this.name);
        //}
    }
    #endregion

    public override bool IsAlive()
    {
        return isAlive;
    }

    // To possess the entities by shooting a raycast.
    public void PossessEntities()
    {
        if (canPossess) 
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up * .5f, transform.forward, out hit, 5f))
            {
                var entity = hit.transform.gameObject.GetComponent<IPossessible>();
                if (entity != null)
                {
                    entity.Possess();
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
        PossessionManager.ToPossess(currentPossession);
    }

    public void UnPossess()
    {
        PossessionManager.UnPossessEntity();
        Possess(); 
    }
}
