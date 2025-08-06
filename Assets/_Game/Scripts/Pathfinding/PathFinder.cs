using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathFinder
{
    public static bool CanConnect(Tile[,] grid, Tile firstTile, Tile secondTile)
    {
        if (firstTile == null || secondTile == null)
        {
            return false;
        }

        if (firstTile == secondTile)
        {
            return false; // Cannot connect a tile to itself
        }

        if (firstTile.GetSprite() != secondTile.GetSprite())
        {
            return false;
        }

        // Implement the pathfinding logic here
        if (
            firstTile.GridPosition.x == secondTile.GridPosition.x
            || firstTile.GridPosition.y == secondTile.GridPosition.y
        )
        {
            // Check if there is any tile between firstTile and secondTile in a straight line (row or column)
            int x1 = firstTile.GridPosition.x;
            int y1 = firstTile.GridPosition.y;
            int x2 = secondTile.GridPosition.x;
            int y2 = secondTile.GridPosition.y;

            // Check along the same column
            if (x1 == x2)
            {
                int minY = Mathf.Min(y1, y2);
                int maxY = Mathf.Max(y1, y2);
                for (int y = minY + 1; y < maxY; y++)
                {
                    if (grid[x1, y] != null && grid[x1, y].gameObject.activeSelf)
                    {
                        // There is a tile between the two tiles
                        if (isUShape(grid, firstTile.GridPosition, secondTile.GridPosition))
                        {
                            return true;
                        }
                        else if (isZShape(grid, firstTile.GridPosition, secondTile.GridPosition))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            // Check along the same row
            else if (y1 == y2)
            {
                int minX = Mathf.Min(x1, x2);
                int maxX = Mathf.Max(x1, x2);
                for (int x = minX + 1; x < maxX; x++)
                {
                    if (grid[x, y1] != null && grid[x, y1].gameObject.activeSelf)
                    {
                        // There is a tile between the two tiles
                        if (isUShape(grid, firstTile.GridPosition, secondTile.GridPosition))
                        {
                            return true;
                        }
                        else if (isZShape(grid, firstTile.GridPosition, secondTile.GridPosition))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        else
        {
            Vector2Int a = firstTile.GridPosition;
            Vector2Int b = secondTile.GridPosition;

            if (isLShape(grid, firstTile.GridPosition, secondTile.GridPosition))
            {
                return true;
            }
            else if (isUShape(grid, firstTile.GridPosition, secondTile.GridPosition))
            {
                return true;
            }
            else if (isZShape(grid, firstTile.GridPosition, secondTile.GridPosition))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    private static bool isLShape(Tile[,] grid, Vector2Int a, Vector2Int b)
    {
        Vector2Int corner1 = new Vector2Int(a.x, b.y);
        Vector2Int corner2 = new Vector2Int(b.x, a.y);

        if (
            isEmpty(grid, corner1)
            && isStraightMove(grid, a, corner1)
            && isStraightMove(grid, corner1, b)
        )
        {
            return true;
        }

        if (
            isEmpty(grid, corner2)
            && isStraightMove(grid, a, corner2)
            && isStraightMove(grid, corner2, b)
        )
        {
            return true;
        }
        return false;
    }

    private static bool isUShape(Tile[,] grid, Vector2Int a, Vector2Int b)
    {
        int minX = Mathf.Min(a.x, b.x);
        int maxX = Mathf.Max(a.x, b.x);
        int minY = Mathf.Min(a.y, b.y);
        int maxY = Mathf.Max(a.y, b.y);

        // Check U-shape paths in vertical direction
        for (int x = minX - 1; x >= 0; x--)
        {
            Vector2Int p1 = new Vector2Int(x, a.y);
            Vector2Int p2 = new Vector2Int(x, b.y);

            // Stop searching this direction if blocked
            if (!isEmpty(grid, p1) || !isEmpty(grid, p2))
                break;

            // Check if full path from a to b through p1 and p2 is clear
            if (
                isStraightMove(grid, a, p1)
                && isStraightMove(grid, p1, p2)
                && isStraightMove(grid, p2, b)
            )
            {
                return true;
            }
        }

        for (int x = maxX + 1; x <= grid.GetLength(0) - 1; x++)
        {
            Vector2Int p1 = new Vector2Int(x, a.y);
            Vector2Int p2 = new Vector2Int(x, b.y);

            if (!isEmpty(grid, p1) || !isEmpty(grid, p2))
                break;

            if (
                isStraightMove(grid, a, p1)
                && isStraightMove(grid, p1, p2)
                && isStraightMove(grid, p2, b)
            )
            {
                return true;
            }
        }

        // Check U-shape paths in horizontal direction
        for (int y = minY - 1; y >= 0; y--)
        {
            Vector2Int p1 = new Vector2Int(a.x, y);
            Vector2Int p2 = new Vector2Int(b.x, y);

            if (!isEmpty(grid, p1) || !isEmpty(grid, p2))
                break;

            if (
                isStraightMove(grid, a, p1)
                && isStraightMove(grid, p1, p2)
                && isStraightMove(grid, p2, b)
            )
            {
                return true;
            }
        }

        for (int y = maxY + 1; y <= grid.GetLength(1) - 1; y++)
        {
            Vector2Int p1 = new Vector2Int(a.x, y);
            Vector2Int p2 = new Vector2Int(b.x, y);

            if (!isEmpty(grid, p1) || !isEmpty(grid, p2))
                break;

            if (
                isStraightMove(grid, a, p1)
                && isStraightMove(grid, p1, p2)
                && isStraightMove(grid, p2, b)
            )
            {
                return true;
            }
        }

        return false;
    }

    private static bool isZShape(Tile[,] grid, Vector2Int a, Vector2Int b)
    {
        if (a.x < b.x && a.y < b.y)
        {
            for (int x = a.x + 1; x <= b.x - 1; x++)
            {
                if (
                    isLShape(grid, new Vector2Int(x, a.y), b)
                    && isStraightMove(grid, a, new Vector2Int(x, a.y))
                )
                {
                    return true;
                }
            }
            for (int y = a.y + 1; y <= b.y - 1; y++)
            {
                if (
                    isLShape(grid, new Vector2Int(a.x, y), b)
                    && isStraightMove(grid, a, new Vector2Int(a.x, y))
                )
                {
                    return true;
                }
            }
            return false;
        }
        else if (a.x > b.x && a.y > b.y)
        {
            for (int x = a.x - 1; x >= b.x + 1; x--)
            {
                if (
                    isLShape(grid, new Vector2Int(x, a.y), b)
                    && isStraightMove(grid, a, new Vector2Int(x, a.y))
                )
                {
                    return true;
                }
            }
            for (int y = a.y - 1; y >= b.y + 1; y--)
            {
                if (
                    isLShape(grid, new Vector2Int(a.x, y), b)
                    && isStraightMove(grid, a, new Vector2Int(a.x, y))
                )
                {
                    return true;
                }
            }
        }
        else if (a.x > b.x && a.y < b.y)
        {
            for (int x = a.x - 1; x >= b.x + 1; x--)
            {
                if (
                    isLShape(grid, new Vector2Int(x, a.y), b)
                    && isStraightMove(grid, a, new Vector2Int(x, a.y))
                )
                {
                    return true;
                }
            }
            for (int y = a.y + 1; y <= b.y - 1; y++)
            {
                if (
                    isLShape(grid, new Vector2Int(a.x, y), b)
                    && isStraightMove(grid, a, new Vector2Int(a.x, y))
                )
                {
                    return true;
                }
            }
        }
        else if (a.x < b.x && a.y > b.y)
        {
            for (int y = a.y - 1; y >= b.y + 1; y--)
            {
                if (
                    isLShape(grid, new Vector2Int(a.x, y), b)
                    && isStraightMove(grid, a, new Vector2Int(a.x, y))
                )
                {
                    return true;
                }
            }
            for (int x = a.x + 1; x <= b.x - 1; x++)
            {
                if (
                    isLShape(grid, new Vector2Int(x, a.y), b)
                    && isStraightMove(grid, a, new Vector2Int(x, a.y))
                )
                {
                    return true;
                }
            }
        }
        return false;
    }

    private static bool isEmpty(Tile[,] grid, Vector2Int position)
    {
        //  Check out of limit
        if (
            position.x < 0
            || position.x >= grid.GetLength(0)
            || position.y < 0
            || position.y >= grid.GetLength(1)
        )
            return true;
        // Check if null or deactive
        return grid[position.x, position.y] == null
            || !grid[position.x, position.y].gameObject.activeSelf;
    }

    private static bool isStraightMove(Tile[,] grid, Vector2Int from, Vector2Int to)
    {
        if (from.x == to.x)
        {
            int minY = Mathf.Min(from.y, to.y);
            int maxY = Mathf.Max(from.y, to.y);
            for (int y = minY + 1; y < maxY; y++)
            {
                if (!isEmpty(grid, new Vector2Int(from.x, y)))
                    return false;
            }
        }
        else if (from.y == to.y)
        {
            int minX = Mathf.Min(from.x, to.x);
            int maxX = Mathf.Max(from.x, to.x);
            for (int x = minX + 1; x < maxX; x++)
            {
                if (!isEmpty(grid, new Vector2Int(x, from.y)))
                    return false;
            }
        }
        else
        {
            return false; // Not straigh
        }
        return true;
    }
}
