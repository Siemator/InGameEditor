using UnityEngine;

public class EditorInputs : MonoBehaviour
{
    public bool rotationButton { get; private set; }
    public bool leftMouseButtonPressed { get; private set; }
    public bool leftShiftPressed { get; private set; }
    public bool rightMouseButtonPressed { get; private set; }

    void Update()
    {
        leftMouseButtonPressed = Input.GetMouseButtonDown(button: 0);
        rotationButton = Input.GetKey(key: KeyCode.R);
        leftShiftPressed = Input.GetKey(key: KeyCode.LeftShift);
        rightMouseButtonPressed = Input.GetMouseButtonDown(button: 1);
    }
}
