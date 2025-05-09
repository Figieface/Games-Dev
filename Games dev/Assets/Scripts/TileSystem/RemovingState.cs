using System;
using System.Linq;
using UnityEngine;

public class RemovingState : IBuildingState //inheriting from interface
{
    private int gameObjectIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    GridData structureData;
    ObjectPlacer objectPlacer;
    ObjectDBSO database;

    public RemovingState(Grid grid,
                         PreviewSystem previewSystem,
                         GridData structureData,
                         ObjectPlacer objectPlacer,
                         ObjectDBSO database)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.structureData = structureData;
        this.objectPlacer = objectPlacer;
        this.database = database;

        previewSystem.StartShowingRemovePreview(); //turn on remove preview in removing state
    }

    public void EndState() //end removing state
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        GridData selectedData = null;
        if(structureData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false)  //putting the structure data in a variable if something is there
        {
            selectedData = structureData;
        }
        if (selectedData == null)
        {
            //nothing here to remove
        }
        else //removes the structure from the objectindex and 
        {
            gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition); //gets the index positions for the object from the grid data
            ObjectData structure = database.objectsData.FirstOrDefault(structure => structure.ID == selectedData.placedObjects[gridPosition].ID);
            StructureShop.currency = StructureShop.currency + Mathf.CeilToInt(structure.Cost * 0.666f);
            if (gameObjectIndex == -1)
                return;
            AudioManager.demolishSound();
            selectedData.RemoveObjectAt(gridPosition); //remove actual prefab
            objectPlacer.RemoveObjectAt(gameObjectIndex); //remove it from our object list
        }
        Vector3 cellPosition = grid.CellToWorld(gridPosition);
        previewSystem.UpdatePosition(cellPosition, CheckIfSelectionIsValid(gridPosition));
    }

    public void RemoveIt(Vector3Int gridPosition)
    {
        GridData selectedData = null;
        selectedData = structureData;
        gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition); //gets the index positions for the object from the grid data
        ObjectData structure = database.objectsData.FirstOrDefault(structure => structure.ID == selectedData.placedObjects[gridPosition].ID);
        StructureShop.currency = StructureShop.currency + Mathf.CeilToInt(structure.Cost * 0.666f);
        if (gameObjectIndex == -1)
            return;
        selectedData.RemoveObjectAt(gridPosition); //remove actual prefab
        objectPlacer.RemoveObjectAt(gameObjectIndex); //remove it from our object list
        //Vector3 cellPosition = grid.CellToWorld(gridPosition);
        //previewSystem.UpdatePosition(cellPosition, CheckIfSelectionIsValid(gridPosition));
    }

    private bool CheckIfSelectionIsValid(Vector3Int gridPosition) //returns inverted canplaceobject, as object needs to be in the way to be removed
    {
        return !(structureData.CanPlaceObjectAt(gridPosition, Vector2Int.one));
    }

    public void UpdateState(Vector3Int gridPosition) //updating preview but for removeal
    {
        bool validity = CheckIfSelectionIsValid(gridPosition);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), validity);
    }
}
