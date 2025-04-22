using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystemm : MonoBehaviour
{
    [SerializeField] private GameObject _mouseIndicator, cellIndicator;
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private Grid grid;

    [SerializeField] private ObjectDBSO database;
    private int selectedObjectIndex = -1;

    [SerializeField] private GameObject gridVisualisation;

    [SerializeField] private Material whiteGrid, redGrid;
    [SerializeField] private GameObject previewGrid;

    private GridData structureData;
    private Renderer previewRenderer;

    private List<GameObject> placedGameObjects = new();

    private void Start()
    {
        StopPlacement();
        structureData = new();
        previewRenderer = previewGrid.GetComponent<Renderer>();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex < 0)
        {
            Debug.LogError($"NO ID FOUND {ID}");
            return;
        }
        gridVisualisation.SetActive(true);
        cellIndicator.SetActive(true);
        _inputManager.OnClicked += PlaceStructure;
        _inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (_inputManager.IsPointOverUI()) return;
        Vector3 mousePosition = _inputManager.GetSelectedMapPosition(); //mouse position using our raycas method in inputmanager
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (placementValidity == false) return;

        GameObject newObject = Instantiate(database.objectsData[selectedObjectIndex].Prefab);
        Vector3 yOffset = new Vector3(-0.1f, 0.0f, 0.0f);
        newObject.transform.position = grid.CellToWorld(gridPosition) + yOffset;
        placedGameObjects.Add(newObject);
        GridData selectedData = structureData;
        selectedData.AddObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size, database.objectsData[selectedObjectIndex].ID, placedGameObjects.Count - 1); 
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = structureData;
        return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }

    private void StopPlacement()
    {
        selectedObjectIndex = -1;
        gridVisualisation.SetActive(false);
        cellIndicator.SetActive(false);
        _inputManager.OnClicked -= PlaceStructure;
        _inputManager.OnExit -= StopPlacement;
    }

    private void Update()
    {
        if (selectedObjectIndex < 0) return;
        Vector3 mousePosition = _inputManager.GetSelectedMapPosition(); //mouse position using our raycas method in inputmanager
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        if (!placementValidity)
        {
            previewRenderer.material = redGrid;
        }
        else
        {
            previewRenderer.material = whiteGrid;
        }
        //previewRenderer.material.color = placementValidity ? Color.white : Color.red;

        _mouseIndicator.transform.position = mousePosition;
        Vector3 yOffset = new Vector3(0.0f, 0.05f, 0.0f);
        cellIndicator.transform.position = grid.CellToWorld(gridPosition) + yOffset;
    }
}
