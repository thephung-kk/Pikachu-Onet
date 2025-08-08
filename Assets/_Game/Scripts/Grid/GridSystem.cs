using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    private int width = 18;
    private int height = 11;
    private int totalTiles;

    [SerializeField]
    private float cellSize = 0.8f;

    [SerializeField]
    private int numberOfSprites = 10;
    private Vector3 originPosition = Vector3.zero;

    [SerializeField]
    private GameObject tilePrefab;

    [SerializeField]
    private Sprite[] tileSprites;

    [SerializeField]
    public GridManager gridManager; // Reference to the click handler
    public Tile[,] grid;
    public int currentLevel;
    public LevelManager levelManager;

    private void Start()
    {
        OnServerInitialized();
    }

    private void OnServerInitialized()
    {
        totalTiles = (width - 2) * (height - 2);
        CalculateOriginPosition();
        CreateGrid();

        if (levelManager != null)
        {
            currentLevel = levelManager.currentLevel;
        }
        else
        {
            currentLevel = 1; // Default level if no LevelManager assigned
        }
    }

    private void Update()
    {
        CollapseDirection dir = GridCollapse.GetCollapseDirectionByLevel(currentLevel);
        GridCollapse.Collapse(grid, dir);
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
        grid = new Tile[width, height];

        // Grid must have an even number of tiles to form pairs
        if (totalTiles % 2 != 0)
        {
            return;
        }

        int totalPairs = totalTiles / 2;

        // Check if we have any sprites at all
        if (allSprites.Length == 0)
        {
            return;
        }

        // Select a limited number of unique sprites from allSprites
        List<Sprite> availableSprites = new List<Sprite>(allSprites);
        List<Sprite> selectedSprites = new List<Sprite>();

        if (numberOfSprites > allSprites.Length)
        {
            numberOfSprites = allSprites.Length;
        }
        else if (numberOfSprites < 24)
        {
            numberOfSprites = 24;
        }

        for (int i = 0; i < numberOfSprites; i++)
        {
            int randIndex = Random.Range(0, availableSprites.Count);
            selectedSprites.Add(availableSprites[randIndex]);
            availableSprites.RemoveAt(randIndex);
        }

        // Generate totalPairs using randomly selected sprites (duplicates allowed)
        List<Sprite> spritePairs = new List<Sprite>();
        for (int i = 0; i < totalPairs; i++)
        {
            if (selectedSprites.Count < 36) // Case: 24 <= count < 36
            {
                // Triplicate each sprite (each sprite appears 3 times for pairing)
                List<Sprite> expanded = new List<Sprite>();
                foreach (var sprite in selectedSprites)
                {
                    expanded.Add(sprite);
                    expanded.Add(sprite);
                    expanded.Add(sprite);
                }
                selectedSprites = expanded;
            }
            else // Case: >= 36
            {
                // Duplicate each sprite (each sprite appears 2 times for pairing)
                List<Sprite> expanded = new List<Sprite>();
                foreach (var sprite in selectedSprites)
                {
                    expanded.Add(sprite);
                    expanded.Add(sprite);
                }
                selectedSprites = expanded;
            }

            // Now randomly take pairs until grid is filled
            while (spritePairs.Count < totalTiles)
            {
                Sprite chosen = selectedSprites[Random.Range(0, selectedSprites.Count)];
                spritePairs.Add(chosen);
                spritePairs.Add(chosen);
                selectedSprites.Remove(chosen);
            }
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

        // Clear grid
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = null;
            }
        }
        // Instantiate and assign sprites to tiles
        int spriteIndex = 0;
        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                Vector3 spawnPos = new Vector3(
                    origin.x + x * cellSize,
                    origin.y + y * cellSize,
                    0f
                );

                GameObject tile = Instantiate(tilePrefab, spawnPos, Quaternion.identity, transform);
                grid[x, y] = tile.GetComponent<Tile>();
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
                tileScript.Init(spritePairs[spriteIndex], new Vector2Int(x, y), gridManager);
                spriteIndex++;
            }
        }
        gridManager.grid = grid;
    }
}
