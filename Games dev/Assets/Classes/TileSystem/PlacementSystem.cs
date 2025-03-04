using System;
using UnityEngine;

public class PlacementSystemm : MonoBehaviour
{
    [SerializeField] private GameObject _mouseIndicator, cellIndicator;
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private Grid grid;

    [SerializeField] private ObjectDBSO _database;
    private int selectedObjectIndex = -1;

    [SerializeField] private GameObject gridVisualisation;

    private void Start()
    {
        StopPlacement();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        selectedObjectIndex = _database.objectsData.FindIndex(data => data.ID == ID);
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
        Vector3Int gridPosition = grid.WorldToCell(mousePosition); //
        GameObject newObject = Instantiate(_database.objectsData[selectedObjectIndex].Prefab);
        Vector3 yOffset = new Vector3(-0.1f, 0.0f, 0.0f);
        newObject.transform.position = grid.CellToWorld(gridPosition) + yOffset;
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
        Vector3Int gridPosition = grid.WorldToCell(mousePosition); //
        _mouseIndicator.transform.position = mousePosition;
        Vector3 yOffset = new Vector3(0.0f, 0.05f, 0.0f);
        cellIndicator.transform.position = grid.CellToWorld(gridPosition) + yOffset;
    }
}
