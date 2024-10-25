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
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
        }
    }

    public override void Movement()
    {

    }

    public void Possess()
    {

        Debug.Log("Possessing..." + this.gameObject);
        possessed = PossessionManager.ToPossess(this);
    }

    public void UnPossess()
    {
        Debug.Log("Un-Possessing..." + this.gameObject);
        //PossessionManager.UnPossessing();
        //FindAnyObjectByType<Player>().Possessed();
    }
}
