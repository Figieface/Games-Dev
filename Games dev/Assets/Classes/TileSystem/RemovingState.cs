using System;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private int gameObjectIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    GridData structureData;
    ObjectPlacer objectPlacer;

    public RemovingState(Grid grid,
                         PreviewSystem previewSystem,
                         GridData structureData,
                         ObjectPlacer objectPlacer)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.structureData = structureData;
        this.objectPlacer = objectPlacer;

        previewSystem.StartShowingRemovePreview();
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        GridData selectedData = null;
        if(structureData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false)
        {
            selectedData = structureData;
        }
        if (selectedData == null)
        {
            //nothing here to remove
        }
        else
        {
            gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);
            if (gameObjectIndex == -1)
                return;
            selectedData.RemoveObjectAt(gridPosition);
            objectPlacer.RemoveObjectAt(gameObjectIndex);
        }
        Vector3 cellPosition = grid.CellToWorld(gridPosition);
        previewSystem.UpdatePosition(cellPosition, CheckIfSelectionIsValid(gridPosition));
    }

    private bool CheckIfSelectionIsValid(Vector3Int gridPosition)
    {
        return !(structureData.CanPlaceObjectAt(gridPosition, Vector2Int.one));
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool validity = CheckIfSelectionIsValid(gridPosition);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), validity);
    }
}
