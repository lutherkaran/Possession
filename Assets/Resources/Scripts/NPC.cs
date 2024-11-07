using UnityEngine;

public class NPC : Entity, IPossessible
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
    }

    public override void ProcessJump()
    {
        base.ProcessJump();
    }

    public override void Sprint()
    {
        base.Sprint();
    }

    public void Possess(GameObject go)
    {
        Debug.Log("Possessing..." + go.name);
        playerPossessed = PossessionManager.ToPossess(go.GetComponent<IPossessible>());
        CameraManager.instance.Initialize(this.gameObject);
    }

    public void Depossess(GameObject go)
    {
        Debug.Log("DePossessing..." + go.name);
        PossessionManager.ToDepossess();
        playerPossessed = null;
        CameraManager.instance.Initialize(player.transform.gameObject);
    }

    public Entity GetEntity()
    {
        return this;
    }
}
