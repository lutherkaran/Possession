using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    private GameObject currentlyPossessed;
    private Vector3 targetPosition;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private MouseAim mouseAim;

    [Header("Camera Settings")]
    [SerializeField] private float smoothTime = 0.3f; // Adjust for desired speed
    [SerializeField] private Vector3 DefaultPosition = new Vector3(0, .6f, 0);
    private Transform cameraAttachPoint;

    public Camera cam;
    public static CameraManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;
        cam = GetComponent<Camera>();
        InitializingMouse();
    }

    private void Start()
    {
        PossessionManager.instance.OnPossessed += AttachCameraToPossessedObject;
    }

    private void InitializingMouse()
    {
        mouseAim = new MouseAim();
        mouseAim.OnFocus();
    }

    private void AttachCameraToPossessedObject(object sender, IPossessable possessedObject)
    {
        currentlyPossessed = possessedObject.GetPossessedEntity().gameObject;
        cameraAttachPoint = possessedObject.GetPossessedEntity().GetCameraAttachPoint();

        StartCoroutine(MovetoPosition(cameraAttachPoint.gameObject));
        cam.transform.SetParent(cameraAttachPoint);
    }

    public IEnumerator MovetoPosition(GameObject currentlyPossessed)
    {
        InputManager.instance.GetOnFootActions().Disable();
        targetPosition = Vector3.zero;
        targetPosition = cameraAttachPoint.position;

        while (Vector3.Distance(cam.transform.position, targetPosition) > 0.1f)
        {
            cam.transform.position = Vector3.SmoothDamp(cam.transform.position, targetPosition, ref velocity, smoothTime);
            yield return null;
        }
        InputManager.instance.GetOnFootActions().Enable();
    }

    public MouseAim GetMouseAim() => mouseAim;

    public void OnDisable()
    {
        PossessionManager.instance.OnPossessed -= AttachCameraToPossessedObject;
    }
}
