using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    private Sprite tileSprite;
    private GridManager gridManager;
    public Vector2Int GridPosition { get; private set; }

    // Initialize tile with sprite, grid position, and reference to the manager
    public void Init(Sprite sprite, Vector2Int pos, GridManager manager)
    {
        tileSprite = sprite;
        spriteRenderer.sprite = sprite;
        GridPosition = pos;
        gridManager = manager;
    }

    // Retrieve assigned sprite
    public Sprite GetSprite()
    {
        return tileSprite;
    }

    // Assign sprite to the tile
    public void SetSprite(Sprite sprite)
    {
        tileSprite = sprite;
        spriteRenderer.sprite = sprite;
    }

    private void OnMouseDown()
    {
        if (gridManager != null)
        {
            gridManager.OnTileClicked(this);
        }
    }

    // Highlight the tile visually
    public void SetHighlight(bool isHighlighted)
    {
        spriteRenderer.color = isHighlighted ? Color.yellow : Color.white;
    }

    // Hide the tile when matched
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
