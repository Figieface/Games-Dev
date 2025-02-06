using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConroller : MonoBehaviour
{
    //movement
    private Vector2 _input;
    private CharacterController _characterController;
    private Vector3 _direction;
    //rotation
    [SerializeField] private float smoothTime = 0.05f;
    private float _currentVelocity;
    [SerializeField] private float speed;
    //gravity & jumping
    private float _gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    private float _velocity;
    [SerializeField] private float jumpPower;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        ApplyGravity();
        ApplyRotation();
        ApplyMovement();
    }


    public void ApplyRotation()
    {
        if (_input.sqrMagnitude == 0) return;

        var targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }

    public void ApplyGravity()
    {
        if (IsGrounded() && _velocity < 0.0f)
        {
            _velocity = -1.0f;
        }
        _velocity += _gravity * gravityMultiplier * Time.deltaTime;
        _direction.y = _velocity;
    }

    public void ApplyMovement()
    {
        _characterController.Move(_direction * speed * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
        _direction = new Vector3(_input.x, 0.0f, _input.y);
        Debug.Log(_input);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!IsGrounded()) return;

        _velocity += jumpPower;
    }

    public bool IsGrounded() => _characterController.isGrounded;

}
