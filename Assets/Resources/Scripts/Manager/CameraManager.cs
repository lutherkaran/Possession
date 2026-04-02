using UnityEngine;

public class CameraManager : IManagable
{
    private static CameraManager Instance;

    public static CameraManager instance
    {
        get { return Instance == null ? Instance = new CameraManager() : Instance; }
    }

    [Header("Camera Settings")]
    [SerializeField] private float smoothTime = .3f; // Adjust for desired speed

    [SerializeField] private MouseAim mouseAim;

    private Vector3 targetPosition;
    private Vector3 velocity = Vector3.zero;

    private bool isTransitioning = false;
    
    private Transform cameraAttachPoint;
    private GameObject newCamera;

    public Camera myCamera { get; private set; }

    public void Initialize()
    {
        newCamera = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Others/MainCamera"));
        myCamera = newCamera.GetComponent<Camera>();
    }

    public void PostInitialize()
    {
        InitializingMouse(newCamera);
        PossessionManager.instance.OnPossessed += AttachCameraToPossessedObject;
    }

    public void Refresh(float deltaTime)
    {
    }

    public void PhysicsRefresh(float fixedDeltaTime)
    {
    }

    public void LateRefresh(float deltaTime)
    {
        if (!isTransitioning) return;

        MoveCamera(deltaTime);
    }

    private void MoveCamera(float deltaTIme)
    {
        InputManager.instance.GetOnFootActions().Disable();

        myCamera.transform.position =
            Vector3.SmoothDamp(myCamera.transform.position, targetPosition, ref velocity, smoothTime);

        if (Vector3.Distance(myCamera.transform.position, targetPosition) <= 0.1f)
        {
            myCamera.transform.localPosition = Vector3.zero;
            isTransitioning = false;
            InputManager.instance.GetOnFootActions().Enable();
        }
    }

    public void OnDemolish()
    {
        Instance = null;
    }

    private void InitializingMouse(GameObject newCamera)
    {
        mouseAim = newCamera.GetComponent<MouseAim>();
        mouseAim.InitializeTargetLocker();
        mouseAim.OnFocus();
    }

    private void AttachCameraToPossessedObject(object sender, IPossessable possessedObject)
    {
        cameraAttachPoint = possessedObject.GetPossessedEntity().GetCameraAttachPoint();

        myCamera.transform.SetParent(cameraAttachPoint);
        targetPosition = cameraAttachPoint.position;

        isTransitioning = true;
    }

    public void OnDisable()
    {
        PossessionManager.instance.OnPossessed -= AttachCameraToPossessedObject;
    }

    public void ApplyCameraSettings(float fieldOfView) => myCamera.fieldOfView = fieldOfView;

    public MouseAim GetMouseAim() => mouseAim;
}