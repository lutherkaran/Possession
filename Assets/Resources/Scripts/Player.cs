using System;
using UnityEngine;

public class Player : Entity, IPossessible, IInputHandler
{
    [SerializeField] float speed;
    [SerializeField] bool isAlive = true;

    //private Vector3 moveInput;
    private CharacterController characterController;
    private bool isGrounded = true;
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
        PlayerPossessed = currentPossession = PossessionManager.Possessing(this);

    }

    public void Update()
    {
        if (PossessionManager.currentlyPossessed != this.currentPossession) { return; }
        isGrounded = characterController.isGrounded;
        //Using a new Input System on Player
        //moveInput = Vector3.zero;
        //moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), .0f, Input.GetAxisRaw("Vertical")).normalized;

        if (Input.GetKeyDown(KeyCode.G))
        {
            PossessEntities();
        }
    }

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

    public void ProcessJump()
    {
        if (currentPossession == null) { return; }
        if (isGrounded)
        {
            PlayerVelocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity);
        }
    }

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
        if (Input.GetKeyDown(KeyCode.A))
        {
            //Debug.Log("Attacked" + this.name);
        }
    }

    public override bool IsAlive()
    {
        return isAlive;
    }

    public void PossessEntities()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * .5f, transform.forward, out hit, 5f))
        {
            Debug.DrawRay(transform.position + Vector3.up * .5f, transform.forward, Color.yellow);
            var v = hit.transform.gameObject.GetComponent<IPossessible>();
            if (v != null)
            {
                v.Possessed();
                currentPossession = null;
                //OnPossessionChanged?.Invoke();
            }
        }
        else
        {

        }
    }

    public void Possessed()
    {
        currentPossession = PlayerPossessed;
        PossessionManager.Possessing(currentPossession);
    }

    public void UnPossessed()
    {
        PossessionManager.UnPossessing();
        Possessed();
    }
}
