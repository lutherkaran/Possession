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
        return animalAnimator;
    }

    public override void Initialize()
    {
        base.Initialize();

        animal = animalType.Chicken;
        animalAnimator = GetComponent<Animator>();
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

    public new Transform GetTransform() => transform;
}
