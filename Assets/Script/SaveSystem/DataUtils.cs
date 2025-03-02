using UnityEngine;

public static class DataUtils
{
    public static void UpdateTransform(Transform piece, transformData tData)
    {
        piece.position = tData.position;
        piece.rotation = tData.rotation;
        piece.localScale = tData.scale;
    }

    public static void UpdateSpriteRenderer(SpriteRenderer sr, spriteRendererData sData)
    {
        sr.color = sData.color;
        sr.sortingOrder = sData.orderInLayer;

        Material material = Resources.Load<Material>($"Mats/{sData.material}");
        Sprite sprite = Resources.Load<Sprite>($"Sprites/{sData.spriteName}");

        if (sprite != null) sr.sprite = sprite;
        if (material != null) sr.material = material;
    }

    public static void UpdateBoxCollider(BoxCollider2D collider, boxColliderData bData)
    {
        collider.isTrigger = bData.isTrigger;
        collider.offset = bData.offset;
        collider.size = bData.size;
    }
}