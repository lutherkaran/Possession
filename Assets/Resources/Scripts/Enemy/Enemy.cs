using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity, IPossessible
{
    IPossessible possessed;

    private Rigidbody rb;
    private NavMeshAgent agent;
    private StateMachine stateMachine;
    private GameObject player;

    public NavMeshAgent Agent { get => agent; }
    public GameObject Player { get => player; }

    private float speed = 10;
    private Vector3 moveInput;

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
        rb = this.GetComponent<Rigidbody>();
    }

    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.Initialise();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        CanSeePlayer();
        currentState = stateMachine.activeState.ToString();
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
        return true;
    }

    public override void Jump()
    {

    }

    public override void Movement()
    {
        moveInput = Vector3.zero;
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), .0f, Input.GetAxisRaw("Vertical")).normalized;
        transform.Translate(moveInput * speed * Time.deltaTime);
    }

    public bool CanSeePlayer()
    {
        if (player != null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < sightDistance)
            {
                Vector3 targetDirection = player.transform.position - transform.position - (Vector3.up * eyeHeight);
                float angleToPlayer = Vector3.Angle(targetDirection, player.transform.position);

                if (angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView)
                {
                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);
                    RaycastHit hitInfo = new RaycastHit();

                    if (Physics.Raycast(ray, out hitInfo, sightDistance))
                    {
                        if (hitInfo.transform.gameObject == player)
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
        possessed = PossessionManager.ToPossess(this);
    }

    public void UnPossess()
    {
        Debug.Log("Un-Possessing..." + this.gameObject);
        //PossessionManager.UnPossessing();
        //FindAnyObjectByType<Player>().Possessed();
    }
}
