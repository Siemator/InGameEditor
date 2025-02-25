using System.Collections.Generic;
using UnityEngine;

public class EditorManager : MonoBehaviour
{
    public GameObject statsGraph;
    public Transform grindParent, trackPieceParent;
    public bool drawGrid = false;
    public Color gridColor;
    public int gridSize = 13;

    private bool grapStatsu = false;

    void Start()
    {
        SaveSystem.Init();
        Grid grid = new Grid(gridSize, gridSize, grindParent, gridColor);
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
            trackPieces = new List<pieceData>()
        };

        foreach (Transform trackPiece in trackPieceParent)
        {
            pieceData pData = new pieceData()
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
            saveObject.trackPieces.Add(pData);
        }
        string json = JsonUtility.ToJson(saveObject);
        // Debug.Log(json);
        SaveSystem.Save(json);

    }

    public void Load()
    {
        // string saveString = SaveSystem.Load();
        // SaveTrackObject saveObject = JsonUtility.FromJson<SaveTrackObject>(saveString);
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
