using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MoveDirection
{
    None,
    Horizontal,
    Vertical,
    LeftDiagonal,
    RightDiagonal
}
public class CloudUI : MonoBehaviour
{
    [SerializeField]
    private MoveDirection _moveDirection;
    [SerializeField]
    private float _cloudMoveSpeed;
    [SerializeField]
    private Vector2 _cloudDefaultPosition;
    [SerializeField]
    private float _cloudMoveRange;

    private void Start()
    {
        _cloudDefaultPosition = transform.position;
    }

    private void FixedUpdate()
    {
        MovingThisCloud();
    }

    private void MovingThisCloud()
    {
        if (transform.position.x > _cloudDefaultPosition.x + _cloudMoveRange || transform.position.y > _cloudDefaultPosition.y + _cloudMoveRange)
        {
            _cloudMoveSpeed = -_cloudMoveSpeed;
        }
        else if (transform.position.x < _cloudDefaultPosition.x - _cloudMoveRange || transform.position.y < _cloudDefaultPosition.y - _cloudMoveRange)
        {
            _cloudMoveSpeed = -_cloudMoveSpeed;
        }
        Vector2 moveDir = GetCloudMoveDiretion();
        transform.position += (Vector3)moveDir * Time.deltaTime;
    }

    public Vector2 GetCloudMoveDiretion()
    {
        switch (_moveDirection)
        {
            case MoveDirection.Horizontal:
                return new Vector2(_cloudMoveSpeed, 0);
            case MoveDirection.Vertical:
                return new Vector2(0, _cloudMoveSpeed);
            case MoveDirection.LeftDiagonal:
                return new Vector2(_cloudMoveSpeed, _cloudMoveSpeed);
            case MoveDirection.RightDiagonal:
                return new Vector2(-_cloudMoveSpeed, _cloudMoveSpeed);
            default:
                return Vector2.zero;
        }
    }
}
