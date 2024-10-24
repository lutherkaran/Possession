using UnityEngine;

public class NPC : Entity, IPossessible
{
    Rigidbody rb;
    IPossessible possessed;

    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    public void Update()
    {
        if (PossessionManager.currentlyPossessed == possessed)
        {
            PlayerControlling();
        }

    }

    private void PlayerControlling()
    {
        Jump();
        Movement();
        Attack();
        if (Input.GetKeyDown(KeyCode.R))
        {
            UnPossessed();
        }
    }

    public override void Attack()
    {

    }

    public override bool IsAlive()
    {
        return false;
    }

    public override void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
        }
    }

    public override void Movement()
    {

    }

    public void Possessed()
    {
        possessed = PossessionManager.Possessing(this, this.gameObject);
    }

    public void UnPossessed()
    {
        PossessionManager.UnPossessing();
        PossessionManager.Possessing(FindAnyObjectByType<Player>(), FindAnyObjectByType<Player>().gameObject);
    }
}
