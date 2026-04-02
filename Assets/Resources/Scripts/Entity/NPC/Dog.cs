using UnityEngine;

public class Dog : AnimalNpc
{
    [SerializeField] private CameraSceneVolumeProfileSO dogSceneVolumeProfile;

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

        animal = animalType.Dog;
    }

    public override void ApplySettings(StateSettings _settings)
    {
        base.ApplySettings(_settings);

        animalNpcController.RunAI(_settings);
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
            CameraManager.instance.ApplyCameraSettings(dogSceneVolumeProfile.fieldOfView);
            GameManager.instance.ApplyVolumeProfile(dogSceneVolumeProfile.volumeProfile);
        }
    }
}
