using UnityEngine;

public class Penguin : Npc
{
    [SerializeField] private CameraSceneVolumeProfileSO penguinSceneVolumeProfile;

    public override void Initialize()
    {
        base.Initialize();
        PossessionManager.instance.OnPossessed += OnCatPossession;
    }

    private void OnCatPossession(object sender, IPossessable e)
    {
        if (e.GetPossessedEntity() == this)
        {
            CameraManager.instance.ApplyCameraSettings(penguinSceneVolumeProfile.fieldOfView);
            GameManager.instance.ApplyVolumeProfile(penguinSceneVolumeProfile.volumeProfile);
        }
    }
}
