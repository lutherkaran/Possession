using UnityEngine;

public class Cat : AnimalNpc
{
    [SerializeField] private CameraSceneVolumeProfileSO catSceneVolumeProfile;

    public override AnimalNpc GetAnimal()
    {
        return this;
    }

    public override Animator GetAnimalAnimator()
    {
        return animalAnimation.GetAnimator();
    }

    public override void ApplySettings(StateSettings _settings)
    {
        base.ApplySettings(_settings);

        animalNpcController.RunAI(_settings);
    }

    public override void Initialize()
    {
        base.Initialize();

        animal = animalType.Cat;
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
            CameraManager.instance.ApplyCameraSettings(catSceneVolumeProfile.fieldOfView);
            GameManager.instance.ApplyVolumeProfile(catSceneVolumeProfile.volumeProfile);
        }
    }
}
