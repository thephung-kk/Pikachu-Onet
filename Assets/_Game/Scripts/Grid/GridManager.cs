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
                Debug.Log("Tiles matched!");
            }
            else
            {
                firstSelected.SetHighlight(false);
                secondSelected.SetHighlight(true);
                firstSelected = secondSelected;
                secondSelected = null;
                Debug.Log("Tiles cannot be matched!");
            }
        }
    }
}
