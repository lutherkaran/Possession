using UnityEngine;

public class CameraManager : IManagable
{
    private static CameraManager Instance;
    public static CameraManager instance { get { return Instance == null ? Instance = new CameraManager() : Instance; } }

    private GameObject currentlyPossessed;
    private Vector3 targetPosition = Vector3.zero;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private MouseAim mouseAim;

    [Header("Camera Settings")]
    [SerializeField] private float smoothTime = .3f; // Adjust for desired speed
    [SerializeField] private Vector3 DefaultPosition = new Vector3(0, .6f, 0);
    private Transform cameraAttachPoint;

    public Camera myCamera { get; private set; }

    private bool isTransitioning = false;

    private float currentLateDeltaTime = 0;

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
        myCamera.transform.position = Vector3.SmoothDamp(myCamera.transform.position, targetPosition, ref velocity, smoothTime);

        if (Vector3.Distance(myCamera.transform.position, targetPosition) <= 0.1f)
        {
            myCamera.transform.position = targetPosition;
            isTransitioning = false;
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
        currentlyPossessed = possessedObject.GetPossessedEntity().gameObject;
        cameraAttachPoint = possessedObject.GetPossessedEntity().GetCameraAttachPoint();
        MoveToTargetPosition(cameraAttachPoint);
        myCamera.transform.SetParent(cameraAttachPoint);
    }

    public void MoveToTargetPosition(Transform cameraAttachPoint)
    {
        InputManager.instance.GetOnFootActions().Disable();

        targetPosition = cameraAttachPoint.position;
        isTransitioning = true;

        InputManager.instance.GetOnFootActions().Enable();
    }

    public MouseAim GetMouseAim() => mouseAim;

    public void OnDisable()
    {
        PossessionManager.instance.OnPossessed -= AttachCameraToPossessedObject;
    }
}
