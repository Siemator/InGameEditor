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

    /// <summary>
    /// Updates the camera controls by processing zoom and panning inputs when the save UI is inactive.
    /// </summary>
    /// <remarks>
    /// This method is called once per frame. It checks if the save UI is not active and, if so, calls the functions responsible for adjusting the camera's zoom level and position based on user input.
    /// </remarks>
    void Update()
    {
        if (!saveUI.gameObject.activeSelf)
        {
            HandleZoom();
            HandleCameraMovement();
        }
    }

    /// <summary>
    /// Adjusts the camera's zoom level based on scroll input.
    /// </summary>
    /// <remarks>
    /// When a scroll input is detected, this method decreases the camera's orthographic size by the product of the scroll movement and the zoom speed,
    /// then clamps the value between the defined minimum and maximum zoom limits.
    /// </remarks>
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
