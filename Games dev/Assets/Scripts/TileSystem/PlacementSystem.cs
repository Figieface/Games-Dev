using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlacementSystemm : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;

    [SerializeField] private ObjectDBSO database;

    [SerializeField] private GameObject gridVisualisation;

    [SerializeField] private Material whiteGrid;
    [SerializeField] private GameObject previewGrid;

    public GridData structureData;

    [SerializeField] private PreviewSystem preview;

    [SerializeField] private ObjectPlacer objectPlacer;

    [SerializeField] GameObject nextRoundUIBlocker;
    [SerializeField] GameObject nextRoundUI;
    [SerializeField] Pathfinding pathfinding;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    IBuildingState buildingState;

    private void Awake()
    {
        structureData = new();
        structureData.AddObjectAt(new Vector3Int(-1, 0, -1), new Vector2Int(3, 3), 5, 0); //places a 3x3 over the base so no objects can be placed there
    }

    private void Start()
    {
        gridVisualisation.SetActive(false);
        StopPlacement();
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

    public void StartSelectionState()
    {
        StopPlacement();
        gridVisualisation.SetActive(true);
        buildingState = new SelectionState(grid,
                                  preview,
                                  structureData,
                                  objectPlacer);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    public void StartRemoving() //method that remove button calls, creates new removestate
    {
        StopPlacement();
        gridVisualisation.SetActive(true);
        buildingState = new RemovingState(grid,
                                          preview,
                                          structureData,
                                          objectPlacer,
                                          database);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    public void StopCurrentState()
    {
        StopPlacement();
        gridVisualisation.SetActive(false);
    }

    public void SellTower()
    {
        //Debug.Log("SellTower called");
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject; //gets the button

        StopPlacement();
        buildingState = new RemovingState(grid,
                                          preview,
                                          structureData,
                                          objectPlacer,
                                          database);
        Transform buttonParent = (clickedButton.transform.parent);
        //Vector3 parentVec3 = parentTrans.position;
        Transform CanvasParent = buttonParent.parent;
        Vector3 parentVec3 = CanvasParent.position;

        Vector3Int gridPosition = grid.WorldToCell(parentVec3);
        //Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if (buildingState is RemovingState removeState)//have to have this check as otherwise a state without removeit could try to call it
        {
            removeState.RemoveIt(gridPosition);
        }
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointOverUI()) return;
        Vector3 mousePosition = inputManager.GetSelectedMapPosition(); //mouse position using our raycas method in inputmanager
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        buildingState.OnAction(gridPosition);
        List<Vector3Int> pathCheck = pathfinding.GenGridandPath(new Vector3Int(20, 0, 20), new Vector3Int(0, 0, 0));
        if (pathCheck == null)
        {
            nextRoundUI.SetActive(false);
            nextRoundUIBlocker.SetActive(true);
        }
        else
        {
            nextRoundUI.SetActive(true);
            nextRoundUIBlocker.SetActive(false);
        }
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

    public bool IsWalkable(Vector3Int gridPosition)
    {
        foreach (var gridpos in structureData.placedObjects)
        {
            if (gridpos.Value.occupiedPositions.Contains(gridPosition)) //checking occupied positions
                return false; //structure in that grid pos
        }
        return true; //nothing in the grid pos
    }
}
