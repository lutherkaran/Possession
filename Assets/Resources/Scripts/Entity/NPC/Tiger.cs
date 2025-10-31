using UnityEngine;

public class Tiger : Npc
{
    [SerializeField] private CameraSceneVolumeProfileSO tigerSceneVolumeProfile;

    public override void Initialize()
    {
        base.Initialize();
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
