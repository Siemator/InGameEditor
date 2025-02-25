using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class transformData
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
}
[System.Serializable]
public class boxColliderData
{
    public bool isTrigger;
    public Vector2 offset;
    public Vector2 size;
}
[System.Serializable]
public class spriteRendererData
{
    public Color color;
    public string spriteName;
    public string material;
    public int orderInLayer;
}

[System.Serializable]
public class childData
{
    public string childName;
    public List<componentsData> componenets;
}
[System.Serializable]
public class componentsData
{
    public List<transformData> transformDatas;
    public List<boxColliderData> boxColliderDatas;
    public List<spriteRendererData> spriteRendererDatas;
}

[System.Serializable]
public class pieceData
{
    public string pieceName;
    public List<componentsData> componenets;
    public List<childData> childs;
}

[System.Serializable]
public class SaveTrackObject
{
    public List<pieceData> trackPieces;
}

