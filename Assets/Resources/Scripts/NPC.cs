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

        }

    }

    public override void Attack()
    {

    }

    public override bool IsAlive()
    {
        return false;
    }

    public void Possess()
    {

        Debug.Log("Possessing..." + this.gameObject);
        possessed = PossessionManager.ToPossess(this, this);
    }

    public void UnPossess()
    {
        Debug.Log("Un-Possessing..." + this.gameObject);
        //PossessionManager.UnPossessing();
        //FindAnyObjectByType<Player>().Possessed();
    }
    public override void ProcessMove(Vector2 input)
    {
        base.ProcessMove(input);
    }

    public override void ProcessJump()
    {
        base.ProcessJump();
    }

    public override void Sprint()
    {
        base.Sprint();
    }
}
