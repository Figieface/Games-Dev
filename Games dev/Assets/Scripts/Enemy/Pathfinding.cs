using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] PlacementSystemm placementData;
    //grid.WorldToCell

    private int gridHeight = 55, gridWidth = 50;
    private int cellHeight= 1, cellWidth = 1;

    private Dictionary<Vector3Int, Cell> cells;
    [SerializeField] private List<Vector3Int> cellsToSearch;
    [SerializeField] private List<Vector3Int> searchedCells;
    [SerializeField] private List<Vector3Int> finalPath;

    private GridData structureData;

    private void Start()
    {
        structureData = placementData.structureData;
    }

    public List<Vector3Int> GenGridandPath(Vector3Int startPos, Vector3Int endPos)
    {
        GenerateGrid();
        return FindPath(startPos, endPos);
    }

    private void GenerateGrid()
    {
        cells = new Dictionary<Vector3Int, Cell>();
        //Debug.Log(cells);
        List<Vector3Int> walls = structureData.BlockedPositions(); //getting all taken up positions

        List<Vector3Int> pathThrough = new List<Vector3Int> //any positions the enemies can always pass through
        {
            new Vector3Int(0, 0, 0),
            new Vector3Int(1, 0, 0),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(0, 0, 1),
            new Vector3Int(0, 0, -1)
        };
        walls.RemoveAll(n => pathThrough.Contains(n)); //removes the positions from list of blocked positions


        for (int x = 0; x < gridWidth; x += cellWidth)
        {
            for (int y = 0; y < gridHeight; y += cellHeight)
            {
                int centreX = x - (gridWidth / 2);
                int centreY = y - (gridHeight / 2);

                Vector3Int position = new Vector3Int(centreX, 0, centreY); //so its centre is 0,0 like my grid object
                cells.Add(position, new Cell(position));

                if (walls == null)
                {
                    //no walls placed yet
                }
                else if (walls.Contains(position)) //if the cell position is in the blockedpositions list
                {
                    cells[position].isWall = true;
                }
            }
        }
    }

    private List<Vector3Int> FindPath(Vector3Int startPos, Vector3Int endPos)
    {
        //Debug.Log("func FindPath called");

        searchedCells = new List<Vector3Int>();
        cellsToSearch = new List<Vector3Int> {startPos};
        finalPath = new List<Vector3Int>();

        Cell startCell = cells[startPos];
        startCell.gCost = 0;
        startCell.hCost = GetDistance(startPos, endPos);
        startCell.fCost = GetDistance(startPos, endPos);

        while (cellsToSearch.Count > 0) //while there are cells to search
        {
            Vector3Int cellToSearch = cellsToSearch[0];
            
            foreach (Vector3Int pos in cellsToSearch) //getting cell with lowest fcost
            {
                Cell c = cells[pos];
                if (c.fCost < cells[cellToSearch].fCost ||
                    c.fCost == cells[cellToSearch].fCost && //or if they have same fcost, look for lower hcost
                    c.hCost == cells[cellToSearch].hCost)
                {
                    cellToSearch = pos;
                }
            }

            cellsToSearch.Remove(cellToSearch);
            searchedCells.Add(cellToSearch);

            if (cellToSearch == endPos)
            {
                Cell pathCell = cells[endPos];

                while (pathCell.position != startPos) //creates final path by following back the 'connection' to startpos
                {
                    finalPath.Add(pathCell.position);
                    pathCell = cells[pathCell.connection];

                }
                finalPath.Add(startPos);
                return finalPath;
            }

            SearchCellNeighbours(cellToSearch, endPos);
        }
        return null; //could only happen if destination not reachable
    }

    private void SearchCellNeighbours(Vector3Int cellPos, Vector3Int endPos)
    {
        /*for (int x = cellPos.x - cellWidth; x <= cellWidth + cellPos.x; x += cellWidth)
        {
            for (int y = cellPos.z - cellHeight; y <= cellHeight + cellPos.z; y += cellHeight)
            {
                Vector3Int neighbourPos = new Vector3Int(x, 0, y);
                if (cells.TryGetValue(neighbourPos, out Cell c) && !searchedCells.Contains(neighbourPos) && !cells[neighbourPos].isWall) //cehcking for: it exists, it hasnt already been searched, it isnt a wall
                {
                    int gCostToNeighbour = cells[cellPos].gCost + GetDistance(cellPos, neighbourPos);

                    if (gCostToNeighbour < cells[neighbourPos].gCost) //if new gcost is lower thawn current then new cost set
                    {
                        Cell neighbourNode = cells[neighbourPos];

                        neighbourNode.connection = cellPos; //setting its connection/parent
                        neighbourNode.gCost = gCostToNeighbour; //setting costs
                        neighbourNode.hCost = GetDistance(neighbourPos, endPos);
                        neighbourNode.fCost = neighbourNode.gCost +neighbourNode.hCost;

                        if (!cellsToSearch.Contains(neighbourPos)) //if not on the list it is added
                        {
                            cellsToSearch.Add(neighbourPos);
                        }
                    }
                }
            }
        }*/

        foreach (var dir in directions)
        {
            Vector3Int neighbourPos = new Vector3Int(cellPos.x + dir.x, 0,cellPos.z + dir.z);
            if (cells.TryGetValue(neighbourPos, out Cell c) && !searchedCells.Contains(neighbourPos) && !cells[neighbourPos].isWall) //cehcking for: it exists, it hasnt already been searched, it isnt a wall
            {
                int gCostToNeighbour = cells[cellPos].gCost + GetDistance(cellPos, neighbourPos);

                if (gCostToNeighbour < cells[neighbourPos].gCost) //if new gcost is lower thawn current then new cost set
                {
                    Cell neighbourNode = cells[neighbourPos];

                    neighbourNode.connection = cellPos; //setting its connection/parent
                    neighbourNode.gCost = gCostToNeighbour; //setting costs
                    neighbourNode.hCost = GetDistance(neighbourPos, endPos);
                    neighbourNode.fCost = neighbourNode.gCost + neighbourNode.hCost;

                    if (!cellsToSearch.Contains(neighbourPos)) //if not on the list it is added
                    {
                        cellsToSearch.Add(neighbourPos);
                    }
                }
            }
        }
    }

    private int GetDistance(Vector3Int pos1, Vector3Int pos2)
    {
        Vector3Int distance = new Vector3Int(Mathf.Abs(pos1.x - pos2.x), Mathf.Abs(pos1.z - pos2.z));

        return distance.x + distance.z; //we just want to move in cardinal directions so distance is their sum
    }

    Vector3Int[] directions = new Vector3Int[]
    {
        new Vector3Int(1,0,0), //right
        new Vector3Int(0,0,1), //up
        new Vector3Int(0,0,-1),//down
        new Vector3Int(-1,0,0) //left
    };

    private class Cell
    {
        public Vector3Int position;
        public int fCost = int.MaxValue;
        public int gCost = int.MaxValue;
        public int hCost = int.MaxValue;
        public Vector3Int connection;
        public bool isWall;

        public Cell(Vector3Int position)
        {
            this.position = position;
        }
    }
}
