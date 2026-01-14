using UnityEngine;

public class Deer : AnimalNpc
{
    [SerializeField] private CameraSceneVolumeProfileSO deerSceneVolumeProfile;

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

        animal = animalType.Deer;
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
            CameraManager.instance.ApplyCameraSettings(deerSceneVolumeProfile.fieldOfView);
            GameManager.instance.ApplyVolumeProfile(deerSceneVolumeProfile.volumeProfile);
        }
    }
}
