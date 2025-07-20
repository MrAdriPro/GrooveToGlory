using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System;

public class Procedural_Dungeon_Gen : MonoBehaviour
{
    //Variables

    public Vector2Int gridSize = new Vector2Int(5, 5);
    public GameObject centerRoomPrefab;
    private Vector3[,] cellCenters; // Guardar posiciones para dibujar Gizmos


    public Cell_Info[,] cellInfo;


    public float tileSize = 5;
    //Functions

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid() 
    {
        cellCenters = new Vector3[gridSize.x, gridSize.y];
        cellInfo = new Cell_Info[gridSize.x, gridSize.y];

        for (int row = 0; row < gridSize.y; row++)
        {
            for (int col = 0; col < gridSize.x; col++)
            {

                // Te calcula el centro de cada celda
                // La columna (1) * el roomsize (10) + roomSize/2 (5) = 15 == 15,0,15
                Vector3 centeredPosition = new Vector3
                    (
                        col * tileSize + tileSize / 2,
                        0f,
                        row * tileSize + tileSize / 2
                );

                cellCenters[col, row] = centeredPosition;
                cellInfo[col, row] = new Cell_Info(null);
            }
        }

        GenerateRooms();
    }

    private void GenerateRooms() 
    {
        //Genera las habitaciones despues de generar el grid comprobando las posiciones del grid
        // Primero genera la primera habitacion en la fila 0 columna random
        int randomFirstPos = UnityEngine.Random.Range(0, gridSize.x);
        cellInfo[randomFirstPos, 0].room = InstantiateRoom(cellCenters[randomFirstPos, 0]);

        //Tengo que generar la siguiente habitacion en una posicion aleatoria, comprobando que no hay habitacion en esa parte del grid y que esta dentro del grid



    }


    private GameObject InstantiateRoom(Vector3 pos) 
    {
        return Instantiate(centerRoomPrefab, pos, Quaternion.identity, transform);
    }


    void OnDrawGizmos()
    {
        if (cellCenters == null) return;

        Gizmos.color = Color.green;
        foreach (var pos in cellCenters)
        {
            Gizmos.DrawWireCube(pos, Vector3.one * tileSize * 0.9f);
        }
    }


}

[Serializable]
public class Cell_Info 
{
    public GameObject room;
    private Vector2 gridPos;

    public Cell_Info(GameObject room, Vector2 gridPos) 
    {
        this.room = room;
        this.gridPos = gridPos;
    }

    public Cell_Info(GameObject room)
    {
        this.room = room;
    }
}

public enum Direction 
{
    Up,
    Left,
    Down,
    Rigth
}
