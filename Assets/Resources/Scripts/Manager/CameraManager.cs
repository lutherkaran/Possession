using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    private GameObject currentlyPossessed;
    private MouseAim mouseAim;

    [Header("Camera Settings")]
    [SerializeField] private float smoothTime = 0.3f; // Adjust for desired speed
    [SerializeField] private Vector3 DefaultPosition = new Vector3(0, .6f, 0);

    public Camera cam;
    public static CameraManager instance;

    Vector3 targetPosition = Vector3.zero;
    Vector3 velocity = Vector3.zero;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;
        mouseAim = new MouseAim();
    }

    public void AttachCameraToPossessedObject(GameObject possessedObject)
    {
        currentlyPossessed = possessedObject;
        cam = this.GetComponent<Camera>();
        StartCoroutine(MovetoPosition(currentlyPossessed));
        cam.transform.SetParent(currentlyPossessed.transform);
    }

    public IEnumerator MovetoPosition(GameObject currentlyPossessed)
    {
        InputManager.OnFootActions.Disable();
        targetPosition = currentlyPossessed.transform.position + DefaultPosition;

        while (Vector3.Distance(cam.transform.position, targetPosition) > 0.1f)
        {
            cam.transform.position = Vector3.SmoothDamp(cam.transform.position, targetPosition, ref velocity, smoothTime);
            yield return null;
        }
        InputManager.OnFootActions.Enable();
    }

    public MouseAim GetMouseAim() => mouseAim;
}
