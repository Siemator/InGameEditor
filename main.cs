using UnityEngine;

public class ObjectManipulation : MonoBehaviour
{
    public Transform blockParent;
    private EditorInputs editorInputs;
    public Transform selectedObject = null;
    public bool gridSnap = true;
    public float gridSize = 7f; // Rozmiar siatki

    void Start()
    {
        editorInputs = this.GetComponent<EditorInputs>();
    }

    void Update()
    {
        if (editorInputs.leftMouseButtonPressed)
        {
            if (selectedObject != null & !editorInputs.leftShiftPressed)
            {
                selectedObject = null;
            }
            else if (editorInputs.leftShiftPressed)
            {
                Collider2D[] hit2D = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if (hit2D.Length > 0)
                {
                    var newBlock = Instantiate(hit2D[0].transform.gameObject);
                    newBlock.transform.SetParent(blockParent);
                    selectedObject = newBlock.transform;
                }
            }
            else
            {
                Collider2D[] hit2D = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if (hit2D.Length > 0)
                    selectedObject = hit2D[0].transform;
            }
        }

        if (editorInputs.rightMouseButtonPressed & selectedObject != null)
        {
            Destroy(selectedObject.gameObject);
            selectedObject = null;
        }
        else if (editorInputs.rightMouseButtonPressed & selectedObject == null)
        {
            Collider2D[] hit2D = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (hit2D.Length > 0)
                Destroy(hit2D[0].gameObject);
        }


        if (selectedObject != null)
        {

            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (gridSnap)
            {
                selectedObject.position = new Vector2(
                    Mathf.Round(mouseWorldPos.x / gridSize) * gridSize,
                    Mathf.Round(mouseWorldPos.y / gridSize) * gridSize
                );
            }
            else
            {
                selectedObject.position = mouseWorldPos;
            }

            if (Input.GetKeyDown(key: KeyCode.R) && Input.GetKey(key: KeyCode.LeftShift))
            {
                selectedObject.transform.Rotate(0, 0, 90);
            }
            else if (Input.GetKeyDown(key: KeyCode.R) && !Input.GetKey(key: KeyCode.LeftShift))
            {
                selectedObject.transform.Rotate(0, 0, -90);
            }
        }
    }
}