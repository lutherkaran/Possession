using UnityEngine;

public class NPC : Entity, IPossessable
{
    Rigidbody rb;

    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    public void Update()
    {

    }

    public override void Attack()
    {

    }

    public override bool IsAlive()
    {
        return false;
    }

    public override void ProcessMove(Vector2 input)
    {
        base.ProcessMove(input);
        if (PossessionManager.Instance.GetCurrentPossessable() == possessedByPlayer)
            transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    public override void ProcessJump()
    {
        base.ProcessJump();
        if (PossessionManager.Instance.GetCurrentPossessable() == possessedByPlayer)
        {
            transform.position += velocity * Time.deltaTime;
            velocity.y = gravity * Time.deltaTime * 10;
        }
    }

    public override void Sprint()
    {
        base.Sprint();
    }

    public void Possessing(GameObject go)
    {
        Debug.Log("Possessing..." + go.name);
        possessedByPlayer = PossessionManager.Instance.ToPossess(go.GetComponent<IPossessable>()).GetCurrentPossession();
    }

    public void Depossessing(GameObject go)
    {
        Debug.Log("DePossessing..." + go.name);
        PossessionManager.Instance.ToDepossess(this);
        possessedByPlayer = null;
    }

    public Entity GetEntity() => this;
}
