using System;
using UnityEngine;

public class Player : Entity, IPossessible
{
    [SerializeField] float speed;
    [SerializeField] bool isAlive = true;

    private Vector3 moveInput;
    public IPossessible possessed;

    public static event Action OnPossessionChanged;

    private void Awake()
    {
        isAlive = true;
        possessed = PossessionManager.Possessing(this, this.gameObject);
    }


    public void Update()
    {
        if (PossessionManager.currentlyPossessed != possessed) { return; }

        moveInput = Vector3.zero;
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), .0f, Input.GetAxisRaw("Vertical")).normalized;

        if (Input.GetKeyDown(KeyCode.G))
        {
            ChangePossession();
        }
    }

    private void ChangePossession()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * .5f, transform.forward, out hit, 5f))
        {
            Debug.Log("FOUND");
            Debug.DrawRay(transform.position + Vector3.up * .5f, transform.forward, Color.yellow);
            var v = hit.transform.gameObject.GetComponent<IPossessible>();
            if (v != null)
            {
                v.Possessed();
                OnPossessionChanged?.Invoke();
            }
        }
        else
        {
            Debug.Log("NOT FOUND");
        }
    }

    private void FixedUpdate()
    {
        if (PossessionManager.currentlyPossessed != possessed) { return; }
        {
            Movement();
            Jump();
            Attack();
        }

    }

    public override void Movement()
    {
        transform.Translate(moveInput * speed * Time.deltaTime);
    }

    public override void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Jumped" + this.name);
        }
    }

    public override void Attack()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Attacked" + this.name);
        }
    }

    public override bool IsAlive()
    {
        return isAlive;
    }

    public void Possessed()
    {
        possessed = PossessionManager.Possessing(this, this.gameObject);
    }

    public void UnPossessed()
    {
        PossessionManager.UnPossessing();
    }
}
