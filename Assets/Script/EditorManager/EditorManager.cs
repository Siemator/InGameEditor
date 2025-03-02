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

    void Start()
    {
        SaveSystem.Init();
        Grid grid = new Grid(gridSize, gridSize, grindParent, gridColor);
        grindParent.gameObject.SetActive(drawGrid);
    }

    public void gridSwitch()
    {
        drawGrid = !drawGrid;
        grindParent.gameObject.SetActive(drawGrid);
    }

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

    public void saveUISwitch()
    {
        saveUI = !saveUI;
        saveUIObject.SetActive(saveUI);
    }

    public void statsSwitch()
    {
        grapStatsu = !grapStatsu;
        statsGraph.SetActive(grapStatsu);
    }

    public void vSync()
    {
        if (QualitySettings.vSyncCount == 0)
            QualitySettings.vSyncCount = 1;
        else
            QualitySettings.vSyncCount = 0;
    }

    public void exitApp()
    {
        Application.Quit();
    }
}
