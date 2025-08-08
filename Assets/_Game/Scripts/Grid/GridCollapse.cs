using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollapseDirection
{
    None,
    Down,
    Up,
    Left,
    Right,
}

public static class GridCollapse
{
    public static void Collapse(Tile[,] grid, CollapseDirection direction)
    {
        switch (direction)
        {
            case CollapseDirection.Down:
                CollapseDown(grid);
                break;
            case CollapseDirection.Up:
                CollapseUp(grid);
                break;
            case CollapseDirection.Left:
                CollapseLeft(grid);
                break;
            case CollapseDirection.Right:
                CollapseRight(grid);
                break;
            case CollapseDirection.None:
                break;
        }
    }

    public static CollapseDirection GetCollapseDirectionByLevel(int level)
    {
        switch (level)
        {
            case 4:
                return CollapseDirection.Down;
            case 5:
                return CollapseDirection.Up;
            case 6:
                return CollapseDirection.Left;
            case 7:
                return CollapseDirection.Right;
            default:
                return CollapseDirection.None;
        }
    }

    public static void CollapseDown(Tile[,] grid)
    {
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        for (int x = 1; x < width - 1; x++)
        {
            for (int y = height - 2; y >= 1; y--)
            {
                if (
                    grid[x, y] != null
                    && !grid[x, y].gameObject.activeSelf
                    && grid[x, y + 1] != null
                    && grid[x, y + 1].gameObject.activeSelf
                )
                {
                    // Move the tile down
                    grid[x, y].SetSprite(grid[x, y + 1].GetSprite());
                    grid[x, y].gameObject.SetActive(true);

                    // Clear the tile above
                    grid[x, y + 1].gameObject.SetActive(false);
                }
            }
        }
    }

    public static void CollapseUp(Tile[,] grid)
    {
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        for (int x = 1; x < width - 1; x++)
        {
            for (int y = height - 2; y >= 1; y--)
            {
                for (int k = y - 1; k >= 1; k--)
                {
                    if (
                        grid[x, y] != null
                        && !grid[x, y].gameObject.activeSelf
                        && grid[x, k] != null
                        && grid[x, k].gameObject.activeSelf
                    )
                    {
                        grid[x, y].SetSprite(grid[x, k].GetSprite());
                        grid[x, y].gameObject.SetActive(true);
                        grid[x, k].gameObject.SetActive(false);
                        break;
                    }
                }
            }
        }
    }

    public static void CollapseRight(Tile[,] grid)
    {
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        for (int y = 1; y < height - 1; y++)
        {
            for (int x = 1; x < width - 1; x++)
            {
                for (int k = x + 1; k < width - 1; k++)
                {
                    if (
                        grid[x, y] != null
                        && !grid[x, y].gameObject.activeSelf
                        && grid[k, y] != null
                        && grid[k, y].gameObject.activeSelf
                    )
                    {
                        grid[x, y].SetSprite(grid[k, y].GetSprite());
                        grid[x, y].gameObject.SetActive(true);
                        grid[k, y].gameObject.SetActive(false);
                        break;
                    }
                }
            }
        }
    }

    public static void CollapseLeft(Tile[,] grid)
    {
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        for (int y = 1; y < height - 1; y++)
        {
            for (int x = width - 2; x >= 1; x--)
            {
                for (int k = x - 1; k >= 1; k--)
                {
                    if (
                        grid[x, y] != null
                        && !grid[x, y].gameObject.activeSelf
                        && grid[k, y] != null
                        && grid[k, y].gameObject.activeSelf
                    )
                    {
                        grid[x, y].SetSprite(grid[k, y].GetSprite());
                        grid[x, y].gameObject.SetActive(true);
                        grid[k, y].gameObject.SetActive(false);
                        break;
                    }
                }
            }
        }
    }
}
