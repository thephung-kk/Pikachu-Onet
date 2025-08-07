using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingUtils : MonoBehaviour
{
    // Check if any valid move exists on the grid
    public static bool HasAnyValidMove(Tile[,] grid)
    {
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        for (int x1 = 1; x1 < width - 1; x1++)
        {
            for (int y1 = 1; y1 < height - 1; y1++)
            {
                Tile tile1 = grid[x1, y1];
                if (tile1 == null || !tile1.gameObject.activeSelf)
                    continue;

                for (int x2 = 1; x2 < width - 1; x2++)
                {
                    for (int y2 = 1; y2 < height - 1; y2++)
                    {
                        if (x1 == x2 && y1 == y2)
                            continue;
                        Tile tile2 = grid[x2, y2];
                        if (tile2 == null || !tile2.gameObject.activeSelf)
                            continue;

                        if (tile1.GetSprite() != tile2.GetSprite())
                            continue;

                        if (PathFinder.CanConnect(grid, tile1, tile2))
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public static (Tile, Tile) GetRandomValidMove(Tile[,] grid)
    {
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        List<(Tile, Tile)> validPairs = new List<(Tile, Tile)>();

        for (int x1 = 1; x1 < width - 1; x1++)
        {
            for (int y1 = 1; y1 < height - 1; y1++)
            {
                Tile tile1 = grid[x1, y1];
                if (tile1 == null || !tile1.gameObject.activeSelf)
                    continue;

                for (int x2 = 1; x2 < width - 1; x2++)
                {
                    for (int y2 = 1; y2 < height - 1; y2++)
                    {
                        if (x1 == x2 && y1 == y2)
                            continue;

                        Tile tile2 = grid[x2, y2];
                        if (tile2 == null || !tile2.gameObject.activeSelf)
                            continue;

                        if (tile1.GetSprite() != tile2.GetSprite())
                            continue;

                        if (PathFinder.CanConnect(grid, tile1, tile2))
                        {
                            validPairs.Add((tile1, tile2));
                        }
                    }
                }
            }
        }

        if (validPairs.Count > 0)
        {
            return validPairs[Random.Range(0, validPairs.Count)];
        }

        return (null, null);
    }

    // Shuffle all active tiles randomly while preserving their sprite pairing
    public static void ShuffleTiles(Tile[,] grid)
    {
        List<Sprite> activeSprites = new List<Sprite>();

        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        // Collect all active sprites
        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                Tile tile = grid[x, y];
                if (tile != null && tile.gameObject.activeSelf)
                {
                    activeSprites.Add(tile.GetSprite());
                }
            }
        }

        // Shuffle the sprite list
        for (int i = 0; i < activeSprites.Count; i++)
        {
            int randomIndex = Random.Range(i, activeSprites.Count);
            (activeSprites[i], activeSprites[randomIndex]) = (
                activeSprites[randomIndex],
                activeSprites[i]
            );
        }

        // Assign the shuffled sprites back to the active tiles
        int spriteIndex = 0;
        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                Tile tile = grid[x, y];
                if (tile != null && tile.gameObject.activeSelf)
                {
                    tile.SetSprite(activeSprites[spriteIndex]);
                    spriteIndex++;
                }
            }
        }
    }
}
