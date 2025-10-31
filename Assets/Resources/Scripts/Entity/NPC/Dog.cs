using UnityEngine;

public class Dog : Npc
{
    [SerializeField] private CameraSceneVolumeProfileSO dogSceneVolumeProfile;

    public override void Initialize()
    {
        base.Initialize();
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
