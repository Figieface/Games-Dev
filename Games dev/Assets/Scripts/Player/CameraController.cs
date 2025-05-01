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

    [Header("Rotation")]
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private float rotationSpeed; //100
    [SerializeField] private float rotationSmoothSpeed; //5
    private Quaternion targetRotation;
    private bool isRotating = false;


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
        if(!allowMovement) return;

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
        transform.position = Vector3.Lerp(transform.position, targetPos, panSmoothSpeed * Time.deltaTime); //smoothly move in direction

        //rotation
        if (!isRotating) //if not rotating we can call our rotate method
        {
            if (Input.GetKeyDown("q"))
            {
                RotateCam(-90f);
            }
            else if (Input.GetKeyDown("e"))
            {
                RotateCam(90f);
            }
        }
        else //else we get smooth rotation and rotate the camera
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f) //stop rotating
            {
                transform.rotation = targetRotation;
                isRotating = false; //once its finished it is not rotating anymore
            }
        }
    }

    private void RotateCam(float angle)
    {
        Vector3 pivot = cameraPivot.position;
        transform.RotateAround(pivot, Vector3.up, angle); 
        targetRotation = transform.rotation; //rotate around the pivot and save the target rotation

        transform.RotateAround(pivot, Vector3.up, -angle); //undo the actual rotation, as itll be done smoothly with slerp

        isRotating = true;
    }
}
