using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AxisMovementInput : MonoBehaviour
{
    #region Variables
    private Vector2 mouseDownPos;
    private Vector2 mouseUpPos;
    private Vector2 moveDirection;
    #endregion

    private void Update()
    {
        PlayerInputHandler();
    }

    #region Getters
    public Vector2 GetOperationMoveDirection()
    {
        return moveDirection;
    }
    #endregion

    #region Setters

    public void SetDefaultMoveDirection()
    {
        moveDirection = Vector2.zero;
    }

    #endregion

    #region Movement
    private void PlayerInputHandler()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseDownPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0))
        {
            mouseUpPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Vector2.Distance(mouseDownPos, mouseUpPos) > 1f)
            {
                CalculateOperationMoveDirection(mouseDownPos, mouseUpPos);
            }
            else
            {
                moveDirection = Vector2.zero;
            }
        }
    }
    private void CalculateOperationMoveDirection(Vector2 startPos, Vector2 endPos)
    {
        if (Mathf.Abs(startPos.x - endPos.x) > Mathf.Abs(startPos.y - endPos.y))
        {
            if (startPos.x > endPos.x)
            {
                moveDirection = Vector2.left;
            }
            else
            {
                moveDirection = Vector2.right;
            }
        }
        else
        {
            if (startPos.y > endPos.y)
            {
                moveDirection = Vector2.down;
            }
            else
            {
                moveDirection = Vector2.up;
            }
        }
    }
    #endregion
}
