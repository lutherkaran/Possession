using UnityEngine;

public class Tiger : AnimalNpc
{
    [SerializeField] private CameraSceneVolumeProfileSO tigerSceneVolumeProfile;

    public override void Initialize()
    {
        base.Initialize();

        animal = animalType.Tiger;
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
            CameraManager.instance.ApplyCameraSettings(tigerSceneVolumeProfile.fieldOfView);
            GameManager.instance.ApplyVolumeProfile(tigerSceneVolumeProfile.volumeProfile);
        }
    }
}
