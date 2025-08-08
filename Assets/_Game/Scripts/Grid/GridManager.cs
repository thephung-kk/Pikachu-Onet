using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private LevelManager levelManager;
    private Tile firstSelected;
    private Tile secondSelected;
    public Tile[,] grid;

    public void OnTileClicked(Tile clickedTile)
    {
        if (firstSelected == null)
        {
            firstSelected = clickedTile;
            firstSelected.SetHighlight(true);
        }
        else if (clickedTile == firstSelected)
        {
            firstSelected.SetHighlight(false);
            firstSelected = null;
        }
        else
        {
            secondSelected = clickedTile;
            secondSelected.SetHighlight(true);

            if (PathFinder.CanConnect(grid, firstSelected, secondSelected))
            {
                firstSelected.SetHighlight(false);
                secondSelected.SetHighlight(false);
                firstSelected.Hide();
                secondSelected.Hide();
                firstSelected = null;
                secondSelected = null;
                HandlePostMatchState();
            }
            else
            {
                firstSelected.SetHighlight(false);
                secondSelected.SetHighlight(true);
                firstSelected = secondSelected;
                secondSelected = null;
            }
        }
    }

    public void ShuffleGrid()
    {
        PathFindingUtils.ShuffleTiles(grid);
        firstSelected = null;
        secondSelected = null;
    }

    public void HintValidMove()
    {
        if (PathFindingUtils.HasAnyValidMove(grid))
        {
            (firstSelected, secondSelected) = PathFindingUtils.GetRandomValidMove(grid);
            firstSelected.SetHighlight(true);
            secondSelected.SetHighlight(true);
            firstSelected = null;
            secondSelected = null;
        }
    }

    private void HandlePostMatchState()
    {
        if (!PathFindingUtils.HasAnyValidMove(grid))
        {
            ShuffleGrid();
        }

        if (IsAllTilesHidden())
        {
            if (levelManager != null)
            {
                levelManager.CompleteLevel();
            }
        }
    }

    private bool IsAllTilesHidden()
    {
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                Tile tile = grid[x, y];
                if (tile != null && tile.gameObject.activeSelf)
                    return false;
            }
        }
        return true;
    }
}
