using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity, IPossessible
{
    public IPossessible playerPossessed;

    private Rigidbody rb;
    private NavMeshAgent agent;
    private StateMachine stateMachine;

    private Vector3 lastKnownPos;

    public Vector3 LastKnownPos { get => lastKnownPos; set => lastKnownPos = value; }
    public NavMeshAgent Agent { get => agent; }
    public GameObject Player { get => this.player.gameObject; }

    public EnemyPath enemyPath;

    [Header("Sight Values")]
    public float sightDistance = 20f;
    public float fieldOfView = 85f;
    public float eyeHeight;

    [Header("Weapon Values")]
    public Transform gunBarrel;
    [Range(0.1f, 10f)]
    public float fireRate;

    [SerializeField]
    private string currentState;
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = this.GetComponent<Rigidbody>();
    }

    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.Initialise();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        CanSeePlayer();
        currentState = stateMachine.activeState.ToString();
    }

    public override void Attack()
    {

    }

    public override bool IsAlive()
    {
        return true;
    }

    public override void ProcessMove(Vector2 Input)
    {
        base.ProcessMove(Input);
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    public override void ProcessJump()
    {
        base.ProcessJump();
    }

    public override void Sprint()
    {
        base.Sprint();
    }

    public bool CanSeePlayer()
    {
        if (player.gameObject != null)
        {
            if (Vector3.Distance(transform.position, player.gameObject.transform.position) < sightDistance)
            {
                Vector3 targetDirection = player.gameObject.transform.position - transform.localPosition - (Vector3.up * eyeHeight);
                float angleToPlayer = Vector3.Angle(targetDirection, player.gameObject.transform.position);

                if (angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView)
                {
                    Ray ray = new Ray(gameObject.transform.position + (Vector3.up * eyeHeight), targetDirection);
                    RaycastHit hitInfo = new RaycastHit();

                    if (Physics.Raycast(ray, out hitInfo, sightDistance))
                    {
                        if (hitInfo.transform.gameObject == player.gameObject)
                        {
                            Debug.DrawRay(ray.origin, ray.direction * sightDistance);
                            return true;
                        }
                    }

                }
            }
        }
        return false;
    }

    public void Possess()
    {
        Debug.Log("Possessing..." + this.gameObject);
        playerPossessed = PossessionManager.ToPossess(this,this);
    }

    public void UnPossess()
    {
        Debug.Log("Un-Possessing..." + this.gameObject);
        //PossessionManager.UnPossessing();
        //FindAnyObjectByType<Player>().Possessed();
    }


}
