using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    private int width = 16;
    private int height = 9;
    private int totalTiles;

    [SerializeField]
    private float cellSize = 1f;

    [SerializeField]
    private int numberOfSprites = 10;
    private Vector3 originPosition = Vector3.zero;

    [SerializeField]
    private GameObject tilePrefab;

    [SerializeField]
    private Sprite[] tileSprites;

    private void Start()
    {
        totalTiles = width * height;
        CalculateOriginPosition();
        CreateGrid();
    }

    private void CalculateOriginPosition()
    {
        originPosition = new Vector3(
            -(width * cellSize) / 2f + cellSize / 2f,
            -(height * cellSize) / 2f + cellSize / 2f,
            0f
        );
    }

    private void CreateGrid()
    {
        // Load all sprites from Resources folder
        Sprite[] allSprites = Resources.LoadAll<Sprite>("Sprites/Tiles");

        // Grid must have an even number of tiles to form pairs
        if (totalTiles % 2 != 0)
        {
            Debug.LogError("Total number of tiles must be even!");
            return;
        }

        int totalPairs = totalTiles / 2;

        // Check if we have any sprites at all
        if (allSprites.Length == 0)
        {
            Debug.LogError("No sprites found in Resources/Sprites/Tiles!");
            return;
        }

        // Select a limited number of unique sprites from allSprites
        List<Sprite> availableSprites = new List<Sprite>(allSprites);
        List<Sprite> selectedSprites = new List<Sprite>();

        if (availableSprites.Count < numberOfSprites)
        {
            numberOfSprites = allSprites.Length;
        }

        for (int i = 0; i < numberOfSprites; i++)
        {
            int randIndex = Random.Range(0, availableSprites.Count);
            selectedSprites.Add(availableSprites[randIndex]);
            availableSprites.RemoveAt(randIndex); // Ensure uniqueness
        }

        // Generate totalPairs using randomly selected sprites (duplicates allowed)
        List<Sprite> spritePairs = new List<Sprite>();
        for (int i = 0; i < totalPairs; i++)
        {
            Sprite chosen = selectedSprites[Random.Range(0, selectedSprites.Count)];
            spritePairs.Add(chosen);
            spritePairs.Add(chosen);
        }

        // Shuffle the pairs
        for (int i = 0; i < spritePairs.Count; i++)
        {
            int randIndex = Random.Range(i, spritePairs.Count);
            (spritePairs[i], spritePairs[randIndex]) = (spritePairs[randIndex], spritePairs[i]);
        }

        // Calculate grid center offset
        float gridWidth = width * cellSize;
        float gridHeight = height * cellSize;
        Vector3 origin = new Vector3(
            -gridWidth / 2f + cellSize / 2f,
            -gridHeight / 2f + cellSize / 2f,
            0f
        );

        // Instantiate and assign sprites to tiles
        int spriteIndex = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 spawnPos = new Vector3(
                    origin.x + x * cellSize,
                    origin.y + y * cellSize,
                    0f
                );

                GameObject tile = Instantiate(tilePrefab, spawnPos, Quaternion.identity, transform);
                // Adjust tile scale to match cell size
                SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    Vector2 spriteSize = sr.sprite.bounds.size; // Get sprite size in Unity units
                    float scaleX = cellSize / spriteSize.x;
                    float scaleY = cellSize / spriteSize.y;
                    tile.transform.localScale = new Vector3(scaleX, scaleY, 1f);
                }
                Tile tileScript = tile.GetComponent<Tile>();
                tileScript.SetSprite(spritePairs[spriteIndex]);
                spriteIndex++;
            }
        }
    }
}
