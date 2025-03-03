using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class EditorManager : MonoBehaviour
{
    public GameObject statsGraph, saveUIObject;
    public Transform grindParent, trackPieceParent;
    public bool drawGrid = false;
    public Color gridColor;
    public int gridSize = 13;
    private bool grapStatsu, saveUI = false;
    private Transform existPiece;

    /// <summary>
    /// Initializes the save system and configures the grid display.
    /// </summary>
    /// <remarks>
    /// Called on startup, this method initializes the SaveSystem, creates a new Grid using the specified gridSize for both dimensions and gridColor,
    /// and sets the active state of the grid’s parent GameObject (grindParent) based on the drawGrid flag.
    /// </remarks>
    void Start()
    {
        SaveSystem.Init();
        Grid grid = new Grid(gridSize, gridSize, grindParent, gridColor);
        grindParent.gameObject.SetActive(drawGrid);
    }

    /// <summary>
    /// Toggles the visibility of the grid by inverting the display flag and updating the grid parent’s active state.
    /// </summary>
    /// <remarks>
    /// This method switches the current state of the grid display, enabling it if disabled and disabling it if enabled.
    /// </remarks>
    public void gridSwitch()
    {
        drawGrid = !drawGrid;
        grindParent.gameObject.SetActive(drawGrid);
    }

    /// <summary>
    /// Serializes the current editor state—including track pieces and metadata—and saves it as a JSON file.
    /// </summary>
    /// <remarks>
    /// This method gathers metadata from designated UI input fields (track name, author, version, description, and the current date)
    /// and iterates over all track pieces under the specified parent transform. For each track piece, it captures transform and
    /// collider information, and for each child piece, it records transform and sprite renderer details. The aggregated data is
    /// then converted to a JSON string and persisted via the SaveSystem.
    /// </remarks>
    public void Save()
    {

        SaveTrackObject saveObject = new SaveTrackObject()
        {
            TrackPieces = new List<PieceData>()
        };

        saveObject.Metadata = new List<Metadata>();
        Metadata mData = new Metadata()
        {
            name = saveUIObject.transform.GetChild(1).GetComponent<TMP_InputField>().text,
            author = saveUIObject.transform.GetChild(2).GetComponent<TMP_InputField>().text,
            date = System.DateTime.Now.ToString("dd/MM/yyyy"),
            version = saveUIObject.transform.GetChild(3).GetComponent<TMP_InputField>().text,
            description = saveUIObject.transform.GetChild(4).GetComponent<TMP_InputField>().text,
        };
        saveObject.Metadata.Add(mData);

        foreach (Transform trackPiece in trackPieceParent)
        {
            PieceData pData = new PieceData()
            {
                pieceName = trackPiece.name,
                componenets = new List<componentsData>(),
                childs = new List<childData>(),
            };

            componentsData cData = new componentsData()
            {
                transformDatas = new List<transformData>(),
                boxColliderDatas = new List<boxColliderData>(),
                spriteRendererDatas = new List<spriteRendererData>(),
            };

            transformData tData = new transformData()
            {
                position = trackPiece.position,
                rotation = trackPiece.rotation,
                scale = trackPiece.localScale,
            };

            cData.transformDatas.Add(tData);

            boxColliderData bData = new boxColliderData()
            {
                isTrigger = trackPiece.GetComponent<BoxCollider2D>().isTrigger,
                offset = trackPiece.GetComponent<BoxCollider2D>().offset,
                size = trackPiece.GetComponent<BoxCollider2D>().size,
            };

            cData.boxColliderDatas.Add(bData);

            foreach (Transform child in trackPiece)
            {
                childData chData = new childData()
                {
                    childName = child.name,
                    componenets = new List<componentsData>(),
                };

                componentsData chCData = new componentsData()
                {
                    transformDatas = new List<transformData>(),
                    boxColliderDatas = new List<boxColliderData>(),
                    spriteRendererDatas = new List<spriteRendererData>(),
                };

                transformData chTData = new transformData()
                {
                    position = child.position,
                    rotation = child.rotation,
                    scale = child.localScale,
                };

                chCData.transformDatas.Add(chTData);

                spriteRendererData sData = new spriteRendererData()
                {
                    color = child.GetComponent<SpriteRenderer>().color,
                    spriteName = child.GetComponent<SpriteRenderer>().sprite.name,
                    material = child.GetComponent<SpriteRenderer>().material.name,
                    orderInLayer = child.GetComponent<SpriteRenderer>().sortingOrder,
                };

                chCData.spriteRendererDatas.Add(sData);
                chData.componenets.Add(chCData);
                pData.childs.Add(chData);
            }
            pData.componenets.Add(cData);
            saveObject.TrackPieces.Add(pData);
        }
        string json = JsonUtility.ToJson(saveObject);
        SaveSystem.Save(json);

    }

    /// <summary>
    /// Loads the saved track configuration from persistent storage and updates the scene accordingly.
    /// </summary>
    /// <remarks>
    /// Deserializes a JSON string into a SaveTrackObject. If no saved data is found, an informational message is logged and the load process is aborted.
    /// For each track piece, the method creates new GameObjects if they do not exist and updates existing ones by applying parent and child data through dedicated update routines.
    /// </remarks>
    public void Load()
    {
        string json = SaveSystem.Load();
        if (json == null)
        {
            Debug.Log("No Save Found");
            return;
        }

        SaveTrackObject saveObject = JsonUtility.FromJson<SaveTrackObject>(json);

        foreach (PieceData pData in saveObject.TrackPieces)
        {
            if (trackPieceParent.Find(pData.pieceName) == null)
            {
                GameObject newPiece = new GameObject();
                parentPieceDataUpdate(newPiece.transform, pData, true, trackPieceParent);

                foreach (childData chData in pData.childs)
                {
                    GameObject newChild = new GameObject();
                    childPieceDataUpdate(newChild.transform, chData, true, newPiece.transform);
                }
            }

            existPiece = trackPieceParent.Find(pData.pieceName);
            parentPieceDataUpdate(existPiece, pData);
            foreach (childData chData in pData.childs)
            {
                foreach (Transform existChild in existPiece)
                {
                    if (existChild.name == chData.childName)
                    {
                        childPieceDataUpdate(existChild, chData);
                    }
                }
            }


        }
    }

    /// <summary>
    /// Updates a child track piece’s transform and sprite renderer using the provided data, optionally initializing it as a new object.
    /// </summary>
    /// <param name="piece">The Transform component of the child track piece to update.</param>
    /// <param name="chData">The data object containing the child's name and component configurations.</param>
    /// <param name="newObject">If true, adds a SpriteRenderer component to a new child object, sets its name, and assigns its parent.</param>
    /// <param name="parent">The parent Transform to assign to the child track piece if initializing a new object.</param>
    private void childPieceDataUpdate(Transform piece, childData chData, bool newObject = false, Transform parent = null)
    {
        if (newObject)
        {
            piece.gameObject.AddComponent<SpriteRenderer>();
            piece.name = chData.childName;
            piece.parent = parent;
        }
        SpriteRenderer pieceSpriteRenderer = piece.GetComponent<SpriteRenderer>();
        DataUtils.UpdateTransform(piece, chData.componenets[0].transformDatas[0]);
        DataUtils.UpdateSpriteRenderer(pieceSpriteRenderer, chData.componenets[0].spriteRendererDatas[0]);
    }

    /// <summary>
    /// Updates a parent piece's transform and collider properties using the provided piece data.
    /// </summary>
    /// <param name="piece">The transform of the parent piece to update.</param>
    /// <param name="pData">The data containing the updated transform and collider configurations.</param>
    /// <param name="newObj">
    /// If set to true, initializes the piece by adding a BoxCollider2D, assigning its name from the data, and setting its parent transform.
    /// </param>
    /// <param name="parent">
    /// The parent transform to assign if the piece is newly created. Ignored if newObj is false.
    /// </param>
    private void parentPieceDataUpdate(Transform piece, PieceData pData, bool newObj = false, Transform parent = null)
    {
        if (newObj)
        {
            piece.gameObject.AddComponent<BoxCollider2D>();
            piece.name = pData.pieceName;
            piece.parent = parent;
        }
        BoxCollider2D pieceBoxColider = piece.GetComponent<BoxCollider2D>();
        DataUtils.UpdateTransform(piece, pData.componenets[0].transformDatas[0]);
        DataUtils.UpdateBoxCollider(pieceBoxColider, pData.componenets[0].boxColliderDatas[0]);
    }

    /// <summary>
    /// Toggles the visibility of the save UI.
    /// </summary>
    /// <remarks>
    /// Inverts the current state of the save UI flag and updates the active state of the associated UI GameObject accordingly.
    /// </remarks>
    public void saveUISwitch()
    {
        saveUI = !saveUI;
        saveUIObject.SetActive(saveUI);
    }

    /// <summary>
    /// Toggles the visibility of the statistics graph UI element.
    /// </summary>
    /// <remarks>
    /// Inverts the internal display flag and updates the active state of the statistics graph accordingly.
    /// </remarks>
    public void statsSwitch()
    {
        grapStatsu = !grapStatsu;
        statsGraph.SetActive(grapStatsu);
    }

    /// <summary>
    /// Toggles vertical synchronization by switching QualitySettings.vSyncCount between 0 and 1.
    /// If vSync is disabled (vSyncCount equals 0), it enables vSync by setting vSyncCount to 1; otherwise, it disables vSync.
    /// </summary>
    public void vSync()
    {
        if (QualitySettings.vSyncCount == 0)
            QualitySettings.vSyncCount = 1;
        else
            QualitySettings.vSyncCount = 0;
    }

    /// <summary>
    /// Exits the application.
    /// </summary>
    /// <remarks>
    /// This method calls Application.Quit to terminate the application. Note that in the Unity editor, this call will not exit play mode.
    /// </remarks>
    public void exitApp()
    {
        Application.Quit();
    }
}
