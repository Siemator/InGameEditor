using UnityEngine;

[RequireComponent(typeof(CameraControlInputs), typeof(Camera))]
public class CameraControl : MonoBehaviour
{
    private CameraControlInputs cameraImputs = null;
    private Camera cam;
    private bool isDragging = false;
    private Vector3 lastMousePosition;

    public Transform saveUI;
    public float baseMoveSpeed = 5f;
    public float zoomSpeed = 2f;
    public float minZoom = 2f;
    public float maxZoom = 10f;

    void Start()
    {
        cameraImputs = this.GetComponent<CameraControlInputs>();
        cam = this.GetComponent<Camera>();
    }

    void Update()
    {
        if (!saveUI.gameObject.activeSelf)
        {
            HandleZoom();
            HandleCameraMovement();
        }
    }

    void HandleZoom()
    {
        if (cameraImputs.scrollMove != 0)
        {
            cam.orthographicSize -= cameraImputs.scrollMove * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(value: cam.orthographicSize, min: minZoom, max: maxZoom);
        }
    }

    void HandleCameraMovement()
    {
        if (cameraImputs.scrollPress)
        {
            if (!isDragging)
            {
                lastMousePosition = Input.mousePosition;
            }
            isDragging = true;
        }
        else
        {
            isDragging = false;
        }

        if (isDragging)
        {
            float dynamicSpeed = baseMoveSpeed * (cam.orthographicSize / maxZoom);
            Vector3 delta = Input.mousePosition - lastMousePosition;

            transform.position -= new Vector3(delta.x * dynamicSpeed * Time.deltaTime, delta.y * dynamicSpeed * Time.deltaTime, 0);

            lastMousePosition = Input.mousePosition;
        }
    }

}
