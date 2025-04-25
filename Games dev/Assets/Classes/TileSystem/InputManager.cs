using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera _sceneCamera; //to send raycast from

    private Vector3 _lastPosition; //to store last pos detected

    [SerializeField] private LayerMask _placementLayerMask; //mask to dictate what layer to place stuff on

    public event Action OnClicked, OnExit;

    private void Update()
    {
        //just if statments to listen for actions
        if (Input.GetMouseButtonDown(0))
        {
            OnClicked?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            OnExit?.Invoke();
        }
    }

    public bool IsPointOverUI() => EventSystem.current.IsPointerOverGameObject();

    public Vector3 GetSelectedMapPosition() //gives position where mouse is hovering over by ray projected at mouse
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = _sceneCamera.nearClipPlane; //cannot select objects not in camera render
        Ray ray = _sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, _placementLayerMask))
        {
            _lastPosition = hit.point;
        }
        return _lastPosition; //returning where the ray hit
    }
}

