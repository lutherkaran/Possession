using UnityEngine;

public class CameraManager : IManagable
{
    private static CameraManager Instance;

    public static CameraManager instance
    {
        get { return Instance == null ? Instance = new CameraManager() : Instance; }
    }

    private Vector3 targetPosition;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private MouseAim mouseAim;

    [Header("Camera Settings")]
    [SerializeField]
    private float smoothTime = .3f; // Adjust for desired speed

    private Transform cameraAttachPoint;

    public Camera myCamera { get; private set; }

    private bool isTransitioning = false;

    public void Initialize()
    {
        GameObject newCamera = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Others/MainCamera"));
        myCamera = newCamera.GetComponent<Camera>();

        InitializingMouse();
    }

    public void PostInitialize()
    {
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
            myCamera.transform.position = targetPosition;
            myCamera.transform.SetParent(cameraAttachPoint);
            isTransitioning = false;
            InputManager.instance.GetOnFootActions().Enable();
        }
    }

    public void OnDemolish()
    {
        Instance = null;
    }

    private void InitializingMouse()
    {
        mouseAim = new MouseAim();
        mouseAim.OnFocus();
    }

    private void AttachCameraToPossessedObject(object sender, IPossessable possessedObject)
    {
        cameraAttachPoint = possessedObject.GetPossessedEntity().GetCameraAttachPoint();
        AttachCamera(cameraAttachPoint);
    }

    public void AttachCamera(Transform _cameraAttachPoint)
    {
        cameraAttachPoint = _cameraAttachPoint;
        targetPosition = _cameraAttachPoint.position;

        isTransitioning = true;
    }

    public MouseAim GetMouseAim() => mouseAim;

    public void OnDisable()
    {
        PossessionManager.instance.OnPossessed -= AttachCameraToPossessedObject;
    }
}