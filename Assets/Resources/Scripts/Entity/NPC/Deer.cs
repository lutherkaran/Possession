using UnityEngine;

public class Deer : Npc
{
    [SerializeField] private CameraSceneVolumeProfileSO deerSceneVolumeProfile;

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
