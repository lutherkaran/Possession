using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    public Camera cam;
    private GameObject tempGO;

    private float xRotation = 0f;
    private bool MouseControl = false;
    private float xSensitivity = 30f;
    private float ySensitivity = 30f;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
    }

    public void Initialize(GameObject go)
    {
        cam = this.GetComponent<Camera>();
        tempGO = go;
        cam.transform.SetParent(go.transform);
        StartCoroutine(MovetoPosition());
    }

    private IEnumerator MovetoPosition()
    {
        float smoothTime = 0.3f; // Adjust for desired speed
        Vector3 velocity = Vector3.zero;
        Vector3 targetPosition = tempGO.transform.position + new Vector3(0, .6f, 0);

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
