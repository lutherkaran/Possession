using UnityEngine;

public class Chicken : AnimalNpc
{
    [SerializeField] private CameraSceneVolumeProfileSO chickenSceneVolumeProfile;
    [SerializeField] private Transform[] pathPoints;

    private Vector3 targetLocation = Vector3.zero;

    private bool targetLocationReached = true;
    private bool targetLocationFound = false;

    public override AnimalNpc GetAnimal()
    {
        return this;
    }

    public override Animator GetAnimalAnimator()
    {
        return animalAnimator;
    }

    public override void Initialize()
    {
        base.Initialize();

        animal = animalType.Chicken;
        animalAnimator = GetComponent<Animator>();
    }

    public override void PostInitialize()
    {
        base.PostInitialize();

        PossessionManager.instance.OnPossessed += OnCatPossession;
    }


    public new void MakeChanges(StateSettings _settings)
    {


        if (_settings.currentActiveState is IdleState)
        {
            GetAnimal().GetAnimalAnimator().SetBool("IsWalking", false); // idle true
            GetAnimal().GetNavMeshAgent().isStopped = true;

            animalAgent.velocity = Vector3.zero;

        }
        else if (_settings.currentActiveState is PatrolState)
        {
            GetAnimal().GetComponent<Chicken>().MoveToLocation(Time.fixedDeltaTime);
            GetAnimal().GetAnimalAnimator().SetBool("IsWalking", true);
        }
    }

    public override void PhysicsRefresh(float fixedDeltaTime)
    {
        base.PhysicsRefresh(fixedDeltaTime);


        //if (!targetLocationFound && targetLocationReached)
        //{
        //    FindTargetLocation();
        //}
        //else
        //{
        //    if (!targetLocationReached)
        //    {
        //        MoveToLocation(fixedDeltaTime);
        //        RotateTowardsTarget();
        //    }
        //}
    }

    private void RotateTowardsTarget()
    {
        transform.LookAt(targetLocation);
    }

    public void MoveToLocation(float fixedDeltaTime)
    {
        if (!targetLocationFound && targetLocationReached)
            FindTargetLocation();
        else
        {
            RotateTowardsTarget();

            float distance = Vector3.Distance(transform.position, targetLocation);

            if (distance >= 0.1f)
            {
                Vector3 direction = (targetLocation - transform.position).normalized;

                transform.Translate(direction * fixedDeltaTime * entitySO.speed, Space.World);
                animalAnimator.SetBool("IsWalking", true);

            }
            else
            {
                targetLocationFound = false;
                targetLocationReached = true;
            }
        }
    }

    public void FindTargetLocation()
    {
        int randomIndex = UnityEngine.Random.Range(0, pathPoints.Length);
        targetLocation = pathPoints[randomIndex].position;

        targetLocationFound = true;
        targetLocationReached = false;
    }

    private void OnCatPossession(object sender, IPossessable e)
    {
        if (e.GetPossessedEntity() == this)
        {
            CameraManager.instance.ApplyCameraSettings(chickenSceneVolumeProfile.fieldOfView);
            GameManager.instance.ApplyVolumeProfile(chickenSceneVolumeProfile.volumeProfile);
        }
    }
}
