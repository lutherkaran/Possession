using System;
using System.Net.Sockets;
using UnityEngine;

public class Chicken : AnimalNpc
{
    [SerializeField] private CameraSceneVolumeProfileSO chickenSceneVolumeProfile;
    [SerializeField] private Transform[] pathPoints;
    
    private Animator chickenAnimator;

    private Vector3 targetLocation = Vector3.zero;

    private bool targetLocationReached = true;
    private bool targetLocationFound = false;

    public override void Initialize()
    {
        base.Initialize();
        
        animal = animalType.Chicken;
        chickenAnimator = GetComponent<Animator>();

    }

    public override void PostInitialize()
    {
        base.PostInitialize();
        PossessionManager.instance.OnPossessed += OnCatPossession;
    }

    public override void PhysicsRefresh(float fixedDeltaTime)
    {
        base.PhysicsRefresh(fixedDeltaTime);

        if (!targetLocationFound && targetLocationReached)
        {
            FindTargetLocation();
        }
        else
        {
            if (!targetLocationReached)
            {
                MoveToLocation(fixedDeltaTime);
                RotateTowardsTarget();
            }
        }
    }

    private void RotateTowardsTarget()
    {
        transform.LookAt(targetLocation);
    }

    private void MoveToLocation(float fixedDeltaTime)
    {
        float distance = Vector3.Distance(transform.position, targetLocation);

        if (distance >= 0.1f)
        {
            Vector3 direction = (targetLocation - transform.position).normalized;

            transform.Translate(direction * fixedDeltaTime * entitySO.speed, Space.World);
            chickenAnimator.SetBool("IsWalking", true);

        }
        else
        {
            targetLocationFound = false;
            targetLocationReached = true;
            chickenAnimator.SetBool("IsWalking", false);
        }
    }

    private void FindTargetLocation()
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
