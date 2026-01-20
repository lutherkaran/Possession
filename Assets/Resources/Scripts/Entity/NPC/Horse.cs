using UnityEngine;

public class Horse : AnimalNpc
{
    [SerializeField] private CameraSceneVolumeProfileSO horseSceneVolumeProfile;

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

        animal = animalType.Horse;
        animalAnimator = GetComponent<Animator>();
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
            CameraManager.instance.ApplyCameraSettings(horseSceneVolumeProfile.fieldOfView);
            GameManager.instance.ApplyVolumeProfile(horseSceneVolumeProfile.volumeProfile);
        }
    }
}
