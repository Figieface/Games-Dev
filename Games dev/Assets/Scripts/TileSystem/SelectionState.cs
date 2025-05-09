using System.Linq;
using UnityEngine;

public class SelectionState : IBuildingState
{
    private int gameObjectIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    GridData structureData;
    ObjectPlacer objectPlacer;

    public SelectionState(Grid grid,
                         PreviewSystem previewSystem,
                         GridData structureData,
                         ObjectPlacer objectPlacer)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.structureData = structureData;
        this.objectPlacer = objectPlacer;

        previewSystem.StartShowingRemovePreview(); //turn on selection preview in removing state
    }

    public void EndState() //end removing state
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        GridData selectedData = null;
        if (structureData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false)  //putting the structure data in a variable if something is there
        {
            selectedData = structureData;
        }
        if (selectedData == null)
        {
            //nothing here to select
        }
        else //removes the structure from the objectindex and 
        {
            gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition); //gets the index positions for the object from the grid data
            GameObject selectedTower = objectPlacer.SelectObjectAt(gameObjectIndex); //select it from our object list
            Debug.Log(selectedTower);
            Canvas upgradePanel = selectedTower.GetComponentInChildren<Canvas>(true);
            //Debug.Log(upgradePanel);
            upgradePanel.gameObject.SetActive(true);            
        }
        Vector3 cellPosition = grid.CellToWorld(gridPosition);
        previewSystem.UpdatePosition(cellPosition, CheckIfSelectionIsValid(gridPosition));
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
