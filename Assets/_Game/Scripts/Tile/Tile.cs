using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private Sprite tileSprite;

    // Assign sprite to the tile
    public void SetSprite(Sprite sprite)
    {
        tileSprite = sprite;
        spriteRenderer.sprite = sprite;
    }

    // Retrieve assigned sprite
    public Sprite GetSprite()
    {
        return tileSprite;
    }
}
