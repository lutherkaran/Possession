using UnityEngine;

public class Cat : Npc
{
    [SerializeField] private CameraSceneVolumeProfileSO catSceneVolumeProfile;

    public override void Initialize()
    {
        base.Initialize();
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
