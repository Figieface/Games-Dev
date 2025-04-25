using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UIElements;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedObjects = new(); //Dictionary with the location(vec3) on the grid(int), and placementdata (grid spaces it occupies, object ID, index in list of placed objects)

    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int ID, int placedObjectIndex) //adds object data to dictionary of placed objects
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize); 
        PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectIndex);
        foreach (var position in positionToOccupy) 
        {
            if (placedObjects.ContainsKey(position)) //if user somehow places an object ontop of another
            {
                throw new Exception($"Dictionary already contains this cell position {position}");
            }
            placedObjects[position] = data; //placement data set if nothing there to block placement
        }
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize) //calcs the positions an object takes up on the grid (grid pos is bottom left)
    {
        List<Vector3Int> returnVal = new();
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        return returnVal;
    }

    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize) //checks whether an object can be placed, by looking in placed objects list and at the positions
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        foreach (var position in positionToOccupy)
        {
            if (placedObjects.ContainsKey(position))
            {
                return false;
            }
        }
        return true;
    }

    internal int GetRepresentationIndex(Vector3Int gridPosition) //finds object via the space it takes up on the grid
    {
        if (placedObjects.ContainsKey(gridPosition) == false) 
            return -1;
        return placedObjects[gridPosition].PlacedObjectIndex;
    }

    internal void RemoveObjectAt(Vector3Int gridPosition) //removes object position data in placed objects
    {
        foreach(var position in placedObjects[gridPosition].occupiedPositions)
        {
            placedObjects.Remove(position);
        }
    }
}

public class PlacementData //class containing data for placed objects
{
    public List<Vector3Int> occupiedPositions;
    public int ID { get; private set; }
    public int PlacedObjectIndex { get; private set; }
    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
    }
}