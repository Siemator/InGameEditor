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
    private Sprite sprite;
    private Material material;
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
        SaveSystem.Save(json);

    }

    public void Load()
    {
        string json = SaveSystem.Load();
        if (json != null)
        {
            SaveTrackObject saveObject = JsonUtility.FromJson<SaveTrackObject>(json);
            foreach (pieceData pData in saveObject.trackPieces)
            {
                if (trackPieceParent.Find(pData.pieceName) != null)
                {
                    existPiece = trackPieceParent.Find(pData.pieceName);
                    existPiece.transform.position = pData.componenets[0].transformDatas[0].position;
                    existPiece.transform.rotation = pData.componenets[0].transformDatas[0].rotation;
                    existPiece.transform.localScale = pData.componenets[0].transformDatas[0].scale;
                    existPiece.GetComponent<BoxCollider2D>().isTrigger = pData.componenets[0].boxColliderDatas[0].isTrigger;
                    existPiece.GetComponent<BoxCollider2D>().offset = pData.componenets[0].boxColliderDatas[0].offset;
                    existPiece.GetComponent<BoxCollider2D>().size = pData.componenets[0].boxColliderDatas[0].size;
                    foreach (childData chData in pData.childs)
                    {
                        foreach (Transform existChild in existPiece)
                        {
                            if (existChild.name == chData.childName)
                            {
                                existChild.transform.position = chData.componenets[0].transformDatas[0].position;
                                existChild.transform.rotation = chData.componenets[0].transformDatas[0].rotation;
                                existChild.transform.localScale = chData.componenets[0].transformDatas[0].scale;
                                existChild.GetComponent<SpriteRenderer>().color = chData.componenets[0].spriteRendererDatas[0].color;
                                sprite = Resources.Load<Sprite>($"Sprites/{chData.componenets[0].spriteRendererDatas[0].spriteName}");
                                if (sprite != null)
                                    existChild.GetComponent<SpriteRenderer>().sprite = sprite;
                                material = Resources.Load<Material>($"Mats/{chData.componenets[0].spriteRendererDatas[0].material}");
                                if (material != null)
                                    existChild.GetComponent<SpriteRenderer>().material = material;
                                existChild.GetComponent<SpriteRenderer>().sortingOrder = chData.componenets[0].spriteRendererDatas[0].orderInLayer;
                            }
                        }
                    }
                }
                else
                {
                    GameObject newPiece = new GameObject();
                    newPiece.name = pData.pieceName;
                    newPiece.transform.parent = trackPieceParent;
                    newPiece.transform.position = pData.componenets[0].transformDatas[0].position;
                    newPiece.transform.rotation = pData.componenets[0].transformDatas[0].rotation;
                    newPiece.transform.localScale = pData.componenets[0].transformDatas[0].scale;
                    newPiece.AddComponent<BoxCollider2D>();
                    newPiece.GetComponent<BoxCollider2D>().isTrigger = pData.componenets[0].boxColliderDatas[0].isTrigger;
                    newPiece.GetComponent<BoxCollider2D>().offset = pData.componenets[0].boxColliderDatas[0].offset;
                    newPiece.GetComponent<BoxCollider2D>().size = pData.componenets[0].boxColliderDatas[0].size;

                    foreach (childData chData in pData.childs)
                    {

                        GameObject newChild = new GameObject();
                        newChild.name = chData.childName;
                        newChild.transform.parent = newPiece.transform;
                        newChild.transform.position = chData.componenets[0].transformDatas[0].position;
                        newChild.transform.rotation = chData.componenets[0].transformDatas[0].rotation;
                        newChild.transform.localScale = chData.componenets[0].transformDatas[0].scale;
                        newChild.AddComponent<SpriteRenderer>();
                        newChild.GetComponent<SpriteRenderer>().color = chData.componenets[0].spriteRendererDatas[0].color;
                        sprite = Resources.Load<Sprite>($"Sprites/{chData.componenets[0].spriteRendererDatas[0].spriteName}");
                        if (sprite != null)
                            newChild.GetComponent<SpriteRenderer>().sprite = sprite;
                        material = Resources.Load<Material>($"Mats/{chData.componenets[0].spriteRendererDatas[0].material}");
                        if (material != null)
                            newChild.GetComponent<SpriteRenderer>().material = material;
                        newChild.GetComponent<SpriteRenderer>().sortingOrder = chData.componenets[0].spriteRendererDatas[0].orderInLayer;
                    }
                }

            }
        }
        else
        {
            Debug.Log("No Save Found");
        }
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
