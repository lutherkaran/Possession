using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName ="CameraVolumeProfile")]
public class CameraSceneVolumeProfileSO : ScriptableObject
{
    public VolumeProfile volumeProfile;
    
    public float fieldOfView;
}
