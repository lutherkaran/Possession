using UnityEngine;

public class NPC : Entity, IPossessible
{
    Rigidbody rb;

    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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
        if (PossessionManager.currentlyPossessed == playerPossessed)
            transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    public override void ProcessJump()
    {
        base.ProcessJump();
        if (PossessionManager.currentlyPossessed == playerPossessed)
        {
            transform.position += velocity * Time.deltaTime;
            velocity.y = gravity * Time.deltaTime * 10;
        }
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
