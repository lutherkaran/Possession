using UnityEngine;

public class Horse : Npc
{
    [SerializeField] private CameraSceneVolumeProfileSO horseSceneVolumeProfile;

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
