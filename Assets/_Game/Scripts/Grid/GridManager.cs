using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
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
                firstSelected.Hide();
                secondSelected.Hide();
                firstSelected = null;
                secondSelected = null;
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
}
