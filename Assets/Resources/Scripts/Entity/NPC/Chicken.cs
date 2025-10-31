using UnityEngine;

public class Chicken : Npc
{
    [SerializeField] private CameraSceneVolumeProfileSO chickenSceneVolumeProfile;

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
}
