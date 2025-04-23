using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystemm : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private Grid grid;

    [SerializeField] private ObjectDBSO database;

    [SerializeField] private GameObject gridVisualisation;

    [SerializeField] private Material whiteGrid;
    [SerializeField] private GameObject previewGrid;

    private GridData structureData;

    [SerializeField] private PreviewSystem preview;

    [SerializeField] private ObjectPlacer objectPlacer;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    IBuildingState buildingState;

    private void Start()
    {
        StopPlacement();
        structureData = new();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        gridVisualisation.SetActive(true);
        buildingState = new PlacementState(ID,
                                           grid,
                                           preview,
                                           database,
                                           structureData,
                                           objectPlacer);
        _inputManager.OnClicked += PlaceStructure;
        _inputManager.OnExit += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();
        gridVisualisation.SetActive(true);
        buildingState = new RemovingState(grid, preview, structureData, objectPlacer);
        _inputManager.OnClicked += PlaceStructure;
        _inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (_inputManager.IsPointOverUI()) return;
        Vector3 mousePosition = _inputManager.GetSelectedMapPosition(); //mouse position using our raycas method in inputmanager
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        buildingState.OnAction(gridPosition);
    }

    //private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    //{
    //    GridData selectedData = structureData;
    //    return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    //}

    private void StopPlacement()
    {
        if(buildingState == null) return;
        gridVisualisation.SetActive(false);
        buildingState.EndState();
        _inputManager.OnClicked -= PlaceStructure;
        _inputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
    }

    private void Update()
    {
        if (buildingState == null) return;
        Vector3 mousePosition = _inputManager.GetSelectedMapPosition(); //mouse position using our raycas method in inputmanager
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        if(lastDetectedPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition);

            lastDetectedPosition = gridPosition;
        }
    }
}
