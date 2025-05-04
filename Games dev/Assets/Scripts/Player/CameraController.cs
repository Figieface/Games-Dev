using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float panSpeed; //20
    [SerializeField] private float panSmoothSpeed; //1
    [SerializeField] private float panBorderThickness; //10
    private bool allowMovement;
    private Vector3 targetPos;
    private float limitX = 20f, limitZ = 20f;

    private void Start()
    {
        targetPos = transform.position;
    }

    private void Update()
    {
        if (GameManager.gameIsOver)
        {
            this.enabled = false;
            return;
        }
        //movement
        allowMovement = true;
        if (Input.GetMouseButton(1)) allowMovement = false;
        if (!allowMovement) return;

        Vector3 inputDir = Vector3.zero;
        if (Input.GetKey("w") /*|| Input.mousePosition.y >= Screen.height - panBorderThickness*/)
            inputDir += Vector3.forward;
        if (Input.GetKey("s") /*|| Input.mousePosition.y <= panBorderThickness*/)
            inputDir += Vector3.back;
        if (Input.GetKey("a") /*|| Input.mousePosition.x <= panBorderThickness*/)
            inputDir += Vector3.left;
        if (Input.GetKey("d") /*|| Input.mousePosition.x >= Screen.width - panBorderThickness*/)
            inputDir += Vector3.right;

        targetPos += inputDir.normalized * panSpeed * Time.deltaTime;

        targetPos.x = Mathf.Clamp(targetPos.x, -limitX, limitX);
        targetPos.z = Mathf.Clamp(targetPos.z, -limitZ - 5, limitZ);

        transform.position = Vector3.Lerp(transform.position, targetPos, panSmoothSpeed * Time.deltaTime); //smoothly move in direction

    }
}
