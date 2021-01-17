using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMgr : MonoBehaviour
{
    public static Grid[,] grids = new Grid[4,9];
    static int colCount = 9;


    public static void PawnMoveOnGrid(Grid currGrid, out List<Grid> resultGrid)
    {
        int currRow = currGrid.rowIdx;
        int currCol = currGrid.colIdx;

        
        int nextRow = currRow;
        int nextCol = currCol + 1;

        resultGrid = new List<Grid>();
        resultGrid.Clear();

        if (nextCol >= grids.Length / 4)
        {
            resultGrid.Add(grids[0, colCount - 1]);
        }
        else
        {
            if(grids[nextRow, nextCol].piece == null)
                resultGrid.Add(grids[nextRow, nextCol]);
        }
    }
    public static void RookMoveOnGrid(Grid currGrid, out List<Grid> resultGrid)
    {
        int currRow = currGrid.rowIdx;
        int currCol = currGrid.colIdx;


        int nextRow = currRow;
        int nextCol = currCol + 3;

        resultGrid = new List<Grid>();
        resultGrid.Clear();

        if (nextCol >= grids.Length / 4 || nextRow >= 4 || nextRow < 0)
        {
            resultGrid.Add(grids[0, colCount - 1]);
        }
        else
        {
            if (grids[nextRow, nextCol].piece == null)
                resultGrid.Add(grids[nextRow, nextCol]);
        }
    }
    public static void KnightMoveOnGrid(Grid currGrid, out List<Grid> resultGrid)
    {
        int currRow = currGrid.rowIdx;
        int currCol = currGrid.colIdx;


        int nextRow = currRow + 1;
        int nextCol = currCol + 2;

        resultGrid = new List<Grid>();
        resultGrid.Clear();

        if (nextCol >= grids.Length / 4 || nextRow >= 4 || nextRow < 0)
        {
            if(!(nextRow >= 4 || nextRow < 0))
            {
                resultGrid.Add(grids[0, colCount - 1]);
            }
        }
        else
        {
            if (grids[nextRow, nextCol].piece == null)
                resultGrid.Add(grids[nextRow, nextCol]);
        }

        nextRow = currRow - 1;
        nextCol = currCol + 2;

        if (nextCol >= grids.Length / 4 || nextRow >= 4 || nextRow < 0)
        {
            if (!(nextRow >= 4 || nextRow < 0))
            {
                resultGrid.Add(grids[0, colCount - 1]);
            }
        }
        else
        {
            if (grids[nextRow, nextCol].piece == null)
                resultGrid.Add(grids[nextRow, nextCol]);
        }
    }
}
