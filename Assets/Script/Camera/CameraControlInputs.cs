using UnityEngine;

public class CameraControlInputs : MonoBehaviour
{
    public float horizontalMove { get; private set; } = 0;
    public float verticalMove { get; private set; } = 0;
    public float scrollMove { get; private set; } = 0;
    public bool scrollPress { get; private set; } = false;

    void Update()
    {
        HandleInputs();
    }

    private void HandleInputs()
    {
        horizontalMove = Input.GetAxis(axisName: "Horizontal");
        verticalMove = Input.GetAxis(axisName: "Vertical");
        scrollMove = Input.GetAxis(axisName: "Mouse ScrollWheel");

        bool newScrollPress = Input.GetMouseButton(button: 2);
        if (newScrollPress != scrollPress)
        {
            scrollPress = newScrollPress;
        }
    }
}
