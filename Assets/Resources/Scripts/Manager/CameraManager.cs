using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class CameraManager : MonoBehaviour
{
    private InputManager inputManager;
    public static CameraManager instance;
    public Camera cam;

    [Header("Mouse Controls")]
    [SerializeField] private float xRotation = 0f;
    [SerializeField] private bool MouseControl = false;
    [SerializeField] private float xSensitivity = 30f;
    [SerializeField] private float ySensitivity = 30f;

    [Header("Camera Settings")]
    [SerializeField] private float smoothTime = 0.3f; // Adjust for desired speed
    [SerializeField] private Vector3 DefaultPosition = new Vector3(0, .6f, 0);
    
    Vector3 targetPosition = Vector3.zero;
    Vector3 velocity = Vector3.zero;

    private GameObject currentlyPossessed;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
        inputManager = GameObject.FindObjectOfType<InputManager>();
    }

    public void AttachCameraToPossessedObject(GameObject possessedObject)
    {
        currentlyPossessed = possessedObject;
        cam = this.GetComponent<Camera>();
        StartCoroutine(MovetoPosition(currentlyPossessed));
        cam.transform.SetParent(currentlyPossessed.transform);
        this.gameObject.transform.position = Vector3.zero;
        this.gameObject.transform.rotation = Quaternion.identity;
    }

    public IEnumerator MovetoPosition(GameObject currentlyPossessed)
    {
        targetPosition = currentlyPossessed.transform.position + DefaultPosition;

        while (Vector3.Distance(cam.transform.position, targetPosition) > 0.1f)
        {
            cam.transform.position = Vector3.SmoothDamp(cam.transform.position, targetPosition, ref velocity, smoothTime);
            yield return null;
        }
    }

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        if (!MouseControl)
        {
            cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            currentlyPossessed.transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
        }
    }

    public void MouseInteraction()
    {
        MouseControl = !MouseControl;

        if (!MouseControl)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
