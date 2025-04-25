using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystemm : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
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

    public void StartPlacement(int ID) //method that building button calls, creates new placementstate
    {
        StopPlacement();
        gridVisualisation.SetActive(true);
        buildingState = new PlacementState(ID,
                                           grid,
                                           preview,
                                           database,
                                           structureData,
                                           objectPlacer);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    public void StartRemoving() //method that remove button calls, creates new removestate
    {
        StopPlacement();
        gridVisualisation.SetActive(true);
        buildingState = new RemovingState(grid, preview, structureData, objectPlacer);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointOverUI()) return;
        Vector3 mousePosition = inputManager.GetSelectedMapPosition(); //mouse position using our raycas method in inputmanager
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        buildingState.OnAction(gridPosition);
    }

    private void StopPlacement() 
    {
        if(buildingState == null) return;
        gridVisualisation.SetActive(false); //turn off grid shader
        buildingState.EndState(); //end build state
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
    }

    private void Update()
    {
        if (buildingState == null) return;
        Vector3 mousePosition = inputManager.GetSelectedMapPosition(); //mouse position using our raycas method in inputmanager
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        if(lastDetectedPosition != gridPosition) //if we dont change where mouse is it wont run updatestate
        {
            buildingState.UpdateState(gridPosition);

            lastDetectedPosition = gridPosition;
        }
    }
}
