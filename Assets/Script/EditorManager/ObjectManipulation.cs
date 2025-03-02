using NUnit.Framework;
using UnityEngine;

public class ObjectManipulation : MonoBehaviour
{
    [Header("References")]
    public Transform blockParent, saveUI;
    private EditorInputs editorInputs;

    [Header("Settings")]
    public bool gridSnap = true;
    public float gridSize = 7f;

    [Header("State")]
    public Transform selectedObject, lastHoveredObject;

    private int objCount;

    private void Start()
    {
        editorInputs = GetComponent<EditorInputs>();
        objCount = blockParent.childCount;
    }

    private void Update()
    {
        if (!saveUI.gameObject.activeSelf)
        {
            HandleSelectionAndDuplication();
            HandleDestruction();
            HandleLastHovering();

            if (selectedObject != null)
            {
                HandleMovement();
                HandleRotation();
            }
        }
    }

    #region Hovering
    void HandleLastHovering()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] hits = Physics2D.OverlapPointAll(mousePosition);

        Transform currentHover = hits.Length > 0 ? hits[0].transform : null;

        if (currentHover != null)
        {
            if (currentHover.childCount >= 2)
            {
                SpriteRenderer firstSprite = currentHover.GetChild(0).GetComponent<SpriteRenderer>();
                SpriteRenderer secondSprite = currentHover.GetChild(1).GetComponent<SpriteRenderer>();

                firstSprite.sortingOrder = 1;
                secondSprite.sortingOrder = 2;
            }

            if (currentHover != lastHoveredObject && lastHoveredObject != null && lastHoveredObject.childCount >= 2)
            {
                SpriteRenderer lastFirstSprite = lastHoveredObject.GetChild(0).GetComponent<SpriteRenderer>();
                SpriteRenderer lastSecondSprite = lastHoveredObject.GetChild(1).GetComponent<SpriteRenderer>();

                lastFirstSprite.sortingOrder = 0;
                lastSecondSprite.sortingOrder = 1;
            }
        }
        else if (lastHoveredObject != null && lastHoveredObject.childCount >= 2)
        {
            SpriteRenderer lastFirstSprite = lastHoveredObject.GetChild(0).GetComponent<SpriteRenderer>();
            SpriteRenderer lastSecondSprite = lastHoveredObject.GetChild(1).GetComponent<SpriteRenderer>();

            lastFirstSprite.sortingOrder = 0;
            lastSecondSprite.sortingOrder = 1;
        }

        lastHoveredObject = currentHover;
    }
    #endregion

    #region Selection
    private void HandleSelectionAndDuplication()
    {
        if (!editorInputs.leftMouseButtonPressed) return;

        if (selectedObject != null && !editorInputs.leftShiftPressed)
        {
            DeselectObject();
            return;
        }

        var hitCollider = GetTopmostCollider();
        if (!hitCollider) return;

        if (editorInputs.leftShiftPressed)
        {
            DuplicateObject(hitCollider.transform);
        }
        else
        {
            SelectObject(hitCollider.transform);
        }
    }

    private Collider2D GetTopmostCollider()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] hits = Physics2D.OverlapPointAll(mousePosition);

        return hits.Length > 0 ? hits[0] : null;
    }

    private void SelectObject(Transform newSelection)
    {
        selectedObject = newSelection;
    }

    private void DeselectObject()
    {
        selectedObject = null;
    }
    #endregion

    #region Duplication
    private void DuplicateObject(Transform original)
    {
        GameObject newObject = Instantiate(original.gameObject, parent: blockParent);
        objCount++;
        newObject.name = $"Obj.{objCount}";
        SelectObject(newSelection: newObject.transform);
    }
    #endregion

    #region Movement
    private void HandleMovement()
    {
        Vector3 targetPosition = GetTargetPosition();
        selectedObject.position = targetPosition;
    }

    private Vector3 GetTargetPosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = selectedObject.position.z;

        return gridSnap ? CalculateGridPosition(mouseWorldPos) : mouseWorldPos;
    }

    private Vector3 CalculateGridPosition(Vector3 position)
    {
        return new Vector3(
            Mathf.Round(position.x / gridSize) * gridSize,
            Mathf.Round(position.y / gridSize) * gridSize,
            position.z
        );
    }
    #endregion

    #region Rotation
    private void HandleRotation()
    {
        if (!Input.GetKeyDown(KeyCode.R)) return;

        float rotationAmount = editorInputs.leftShiftPressed ? 90f : -90f;
        selectedObject.Rotate(0, 0, rotationAmount);
    }
    #endregion

    #region Destruction
    private void HandleDestruction()
    {
        if (!editorInputs.rightMouseButtonPressed) return;

        if (selectedObject != null)
        {
            DestroySelectedObject();
        }
        else
        {
            TryDestroyObjectUnderMouse();
        }
    }

    private void DestroySelectedObject()
    {
        Destroy(selectedObject.gameObject);
        DeselectObject();
    }

    private void TryDestroyObjectUnderMouse()
    {
        var hitCollider = GetTopmostCollider();
        if (hitCollider)
        {
            Destroy(hitCollider.gameObject);
        }
    }
    #endregion
}