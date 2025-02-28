using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    [SerializeField] private LayerMask _placementCheckMask;
    [SerializeField] private LayerMask _placementCollideMask;
    [SerializeField] private Camera _playerCamera;
    private GameObject _currentPlacingTower;


    private void Update()
    {
        if (_currentPlacingTower != null)
        {
            Ray camray = _playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;


            if(Physics.Raycast(camray, out hitInfo, 100f, _placementCollideMask)) //3rd field is max distance
            {
                _currentPlacingTower.transform.position = hitInfo.point; //will insantiate where raycast hits
            }

            if (Input.GetMouseButtonDown(0) && hitInfo.collider.gameObject != null)
            {
                if (!hitInfo.collider.gameObject.CompareTag("CannotPlace"))
                {
                    BoxCollider towerCollider = _currentPlacingTower.gameObject.GetComponent<BoxCollider>();
                    towerCollider.isTrigger = true;

                    Vector3 boxCentre = _currentPlacingTower.gameObject.transform.position + towerCollider.center;
                    Vector3 halfExtents = towerCollider.size / 2;
                    if (Physics.CheckBox(boxCentre, halfExtents, Quaternion.identity, _placementCheckMask, QueryTriggerInteraction.Ignore))
                    {
                        towerCollider.isTrigger = false;
                        _currentPlacingTower = null;
                    }
                } 
            }
        }
    }
    public void SetTowerToPlace(GameObject tower)
    {
        _currentPlacingTower = Instantiate(tower, Vector3.zero, Quaternion.identity);
    }
}
