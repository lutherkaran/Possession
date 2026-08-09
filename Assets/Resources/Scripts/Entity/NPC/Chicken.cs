using UnityEngine;

public class Chicken : AnimalNpc
{
    [SerializeField] private CameraSceneVolumeProfileSO chickenSceneVolumeProfile;

    public override AnimalNpc GetAnimal()
    {
        return this;
    }

    public override Animator GetAnimalAnimator()
    {
        return animalAnimation.GetAnimator();
    }

    public override void Initialize()
    {
        base.Initialize();

        animal = animalType.Chicken;
    }

    public override void ApplySettings(StateSettings _settings)
    {
        base.ApplySettings(_settings);

        animalNpcController.RunAI(_settings);
    }

    public override void PhysicsRefresh(float fixedDeltaTime)
    {
        base.PhysicsRefresh(fixedDeltaTime);
    }

    public override void PostInitialize()
    {
        base.PostInitialize();

        PossessionManager.instance.OnPossessed += OnCatPossession;
    }

    private void OnCatPossession(object sender, IPossessable e)
    {
        if (e.GetPossessedEntity() == this)
        {
            CameraManager.instance.ApplyCameraSettings(chickenSceneVolumeProfile.fieldOfView);
            GameManager.instance.ApplyVolumeProfile(chickenSceneVolumeProfile.volumeProfile);
        }
    }

    public override bool IsSafe()
    {
        foreach (var entity in EntityManager.instance.GetEntities())
        {
            if (entity is not Chicken)
            {
                if (Vector3.Distance(transform.position, entity.GetTransform().position) >= safeDistance)

                    return false;
            }
            else
            {
                return true;
            }   
        }
        return false;
    }

    public override EntityAnimation GetEntityAnimation() => entityAnimation;

    public new Transform GetTransform() => transform;
}
