using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    public Camera cam;
    private GameObject tempGO;

    private float xRotation = 0f;
    private bool MouseControl = false;
    private float xSensitivity = 30f;
    private float ySensitivity = 30f;
    Vector3 targetPosition = Vector3.zero;
    float smoothTime = 0.3f; // Adjust for desired speed
    Vector3 velocity = Vector3.zero;

    InputManager inputManager;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
        inputManager = GameObject.FindObjectOfType<InputManager>();
    }

    private void LateUpdate()
    {
        if (tempGO != null)
        {
            //FollowTarget();
        }
    }

    public void FollowTarget()
    {
        targetPosition = tempGO.transform.position + new Vector3(0, .6f, 0);
        targetPosition = Vector3.SmoothDamp(transform.position, tempGO.transform.position, ref velocity, smoothTime * Time.deltaTime);
        transform.position = targetPosition;
    }

    public void Initialize(GameObject go)
    {
        cam = this.GetComponent<Camera>();
        tempGO = go;
        cam.transform.SetParent(go.transform);

        inputManager.playerInput.OnFoot.Disable();
        inputManager.playerInput.Disable();
        StartCoroutine(MovetoPosition(go));
        inputManager.playerInput.Enable();
        inputManager.playerInput.OnFoot.Enable();

        this.gameObject.transform.position = Vector3.zero;
        this.gameObject.transform.rotation = Quaternion.identity;
    }

    public IEnumerator MovetoPosition(GameObject go)
    {
        this.gameObject.transform.position = Vector3.zero;
        this.gameObject.transform.rotation = Quaternion.identity;

        targetPosition = go.transform.position + new Vector3(0, .6f, 0);

        while (Vector3.Distance(cam.transform.position, targetPosition) > 0.1f)
        {
            cam.transform.position = Vector3.SmoothDamp(cam.transform.position, targetPosition, ref velocity, smoothTime);
            yield return null;
        }
    }

    // camera to become a child of possessed entity.

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        if (!MouseControl)
        {
            cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            tempGO.transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
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
