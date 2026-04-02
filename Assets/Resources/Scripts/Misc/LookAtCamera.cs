using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private enum Mode
    {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted
    }

    [SerializeField] private Mode mode;

    private void LateUpdate()
    {
        if (CameraManager.instance != null)
        {
            Transform cam = CameraManager.instance.myCamera.transform;

            switch (mode)
            {
                case Mode.LookAt:
                    transform.LookAt(cam);
                    break;
                case Mode.LookAtInverted:
                    Vector3 dirFromCamera = transform.forward - cam.position;
                    transform.LookAt(transform.position + dirFromCamera);
                    break;
                case Mode.CameraForward:
                    transform.forward = cam.forward;
                    break;
                case Mode.CameraForwardInverted:
                    transform.forward = -1 * cam.forward;
                    break;
            }
        }

    }
}
