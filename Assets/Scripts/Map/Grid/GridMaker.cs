using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UIElements;

public class GridMaker : MonoBehaviour
{
    // ir paso a paso
    [Header("Grid Configuration")]
    public int rows = 5;
    public int columns = 5;
    public float spacing = 4f;

    [Header("Grid and Cells")]
    [BoxGroup("Grid and Cells")] public GameObject startRoom;
    [BoxGroup("Grid and Cells")] public GameObject room;
    [BoxGroup("Grid and Cells")] public GameObject noPathRoom;
    [BoxGroup("Grid and Cells")] public GameObject bossRoom;
    [BoxGroup("Grid and Cells")] public CellData data;
    //this thing [,] basically is to create a matriz, a matrix has only 2 index, in this case is perfect if we are gonna use only rows and  columns
    [BoxGroup("Grid and Cells")] public CellData[,] grid;


    private void Start()
    {
        GenerateGrid();
    }


    void GenerateGrid()
    {
        //Here we are gonnna create the grid and put all the cells are unoccupied
        grid = new CellData[rows, columns];

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                grid[row, col] = ScriptableObject.CreateInstance<CellData>();
                grid[row, col].isOcuppied = false;
            }
        }

        // next we are gonna choose a random column int he first row to place the start room
        int currentRow = 0;
        int currentCol = Random.Range(0, columns);
        grid[currentRow, currentCol].isOcuppied = true;
        Instantiate(startRoom, GetWorldPosition(currentRow, currentCol), Quaternion.identity);
        // now a while to generate the path throught the grid until de last row 
        while (currentRow < rows - 1)
        {
            //jsut a list with the availableNeighbours to move into
            List<Vector2Int> posibles = GetAvailableNeighbours(currentRow, currentCol);

            if (posibles.Count == 0)
                break;

            // now we pick random valid neighbour
            Vector2Int next = posibles[Random.Range(0, posibles.Count)];
            currentRow = next.x;
            currentCol = next.y;

            grid[currentRow, currentCol].isOcuppied = true;
            // and now decide which room we have to instantiate a normal room or the boss one 
            GameObject toPlace = (currentRow == rows - 1) ? bossRoom : room;
            Instantiate(toPlace, GetWorldPosition(currentRow, currentCol), Quaternion.identity);
        }
        //this is for instantiate the rest of the grid that are not the path.
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (!grid[row, col].isOcuppied)
                {
                    Instantiate(noPathRoom, GetWorldPosition(row, col), Quaternion.identity);
                }
            }
        }
    }


    /// <summary>
    /// this is only to convert the grid rows and cols   to a world position 
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    Vector3 GetWorldPosition(int row, int col)
    {
        return new Vector3(col * spacing, 0, -row * spacing);
    }
    /// <summary>
    /// to be honest the first time using vector2int but basically is the same than a vector2 but using integers, in this case is perfect because the rows and the cols are ints
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    List<Vector2Int> GetAvailableNeighbours(int row, int col)
    {
        List<Vector2Int> neighbours = new List<Vector2Int>();

        // Now includes Up
        Vector2Int[] directions = new Vector2Int[]
        {
        new Vector2Int(1, 0),   // Down
        new Vector2Int(0, 1),   // Right
        new Vector2Int(0, -1),  // Left
        };

        foreach (var dir in directions)
        {
            int newRow = row + dir.x;
            int newCol = col + dir.y;

            if (newRow >= 0 && newRow < rows && newCol >= 0 && newCol < columns)
            {
                // Only add is unocuppied
                if (!grid[newRow, newCol].isOcuppied)
                {
                    neighbours.Add(new Vector2Int(newRow, newCol));
                }
            }
        }

        return neighbours;
    }

}
