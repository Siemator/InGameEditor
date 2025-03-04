using UnityEngine;

public static class DataUtils
{
    /// <summary>
    /// Updates the specified Transform's position, rotation, and local scale based on the provided transform data.
    /// </summary>
    /// <param name="piece">The Transform to be updated.</param>
    /// <param name="tData">The transform data containing the new position, rotation, and scale values.</param>
    public static void UpdateTransform(Transform piece, transformData tData)
    {
        piece.position = tData.position;
        piece.rotation = tData.rotation;
        piece.localScale = tData.scale;
    }

    /// <summary>
    /// Updates the properties of a SpriteRenderer using values from a spriteRendererData object.
    /// </summary>
    /// <param name="sr">The SpriteRenderer to update.</param>
    /// <param name="sData">
    /// The data containing the new color, sorting order, sprite name, and material name. If a sprite or material matching the provided names is found in the "Sprites" or "Mats" resources folder, it is applied to the SpriteRenderer.
    /// </param>
    public static void UpdateSpriteRenderer(SpriteRenderer sr, spriteRendererData sData)
    {
        sr.color = sData.color;
        sr.sortingOrder = sData.orderInLayer;

        Material material = Resources.Load<Material>($"Mats/{sData.material}");
        Sprite sprite = Resources.Load<Sprite>($"Sprites/{sData.spriteName}");

        if (sprite != null) sr.sprite = sprite;
        if (material != null) sr.material = material;
    }

    /// <summary>
    /// Configures a BoxCollider2D using the provided boxColliderData, updating its trigger state, offset, and size.
    /// </summary>
    /// <param name="collider">The BoxCollider2D component to update.</param>
    /// <param name="bData">The data structure containing the new trigger state, offset, and dimensions.</param>
    public static void UpdateBoxCollider(BoxCollider2D collider, boxColliderData bData)
    {
        collider.isTrigger = bData.isTrigger;
        collider.offset = bData.offset;
        collider.size = bData.size;
    }
}