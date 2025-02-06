using System.Security.Cryptography;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Vector3 _offset;
    private float _setY;

    private void Awake()
    {
        _offset = transform.position - target.position;
        _setY = transform.position.y;
    }

    private void LateUpdate()
    {
        Vector3 newPos = target.position + _offset;
        newPos.y = _setY;
        transform.position = newPos;
    }
}
