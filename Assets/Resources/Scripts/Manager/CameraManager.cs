using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    private GameObject currentlyPossessed;
    private MouseAim mouseAim;

    [Header("Camera Settings")]
    [SerializeField] private float smoothTime = 0.3f; // Adjust for desired speed
    [SerializeField] private Vector3 DefaultPosition = new Vector3(0, .6f, 0);

    public Camera camera;
    public static CameraManager instance;

    Vector3 targetPosition = Vector3.zero;
    Vector3 velocity = Vector3.zero;

    private void OnEnable()
    {
        PossessionManager.Instance.OnPossessed += AttachCameraToPossessedObject;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;
        mouseAim = new MouseAim();
        camera = GetComponent<Camera>();
    }

    private void AttachCameraToPossessedObject(object sender, GameObject possessedObject)
    {
        currentlyPossessed = possessedObject;
        StartCoroutine(MovetoPosition(currentlyPossessed));
        camera.transform.SetParent(currentlyPossessed.transform);
    }

    public IEnumerator MovetoPosition(GameObject currentlyPossessed)
    {
        InputManager.OnFootActions.Disable();
        targetPosition = currentlyPossessed.transform.position + DefaultPosition;

        while (Vector3.Distance(camera.transform.position, targetPosition) > 0.1f)
        {
            camera.transform.position = Vector3.SmoothDamp(camera.transform.position, targetPosition, ref velocity, smoothTime);
            yield return null;
        }
        InputManager.OnFootActions.Enable();
    }

    public MouseAim GetMouseAim() => mouseAim;

    public void OnDisable()
    {
        PossessionManager.Instance.OnPossessed -= AttachCameraToPossessedObject;
    }
}
