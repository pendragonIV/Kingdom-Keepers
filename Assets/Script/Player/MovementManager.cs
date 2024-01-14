using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    private GameObject hitObject;
    [SerializeField]
    private AxisMovementInput axisMovementInput;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            hitObject = CastRay();
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (hitObject && hitObject.GetComponent<Operation>() != null)
            {
                if (!hitObject.GetComponent<Operation>().IsOperationMoveable())
                {
                    hitObject = null;
                    axisMovementInput.SetDefaultMoveDirection();
                    return;
                }
            }
        }
        MoveOperation();    
    }

    private void MoveOperation()
    {
        Vector2 moveDir = axisMovementInput.GetOperationMoveDirection();
        if(hitObject != null && moveDir != Vector2.zero)
        {
            Vector3Int cellToMove = GetCellToMoveOperation(hitObject, moveDir);
            GridCellManager.instance.RemovePlacedCell(GridCellManager.instance.GetObjCell(hitObject.transform.position));
            hitObject.transform.DOMove(GridCellManager.instance.PositonToMove(cellToMove), 0.5f).OnComplete(() =>
            {
                Calculator.instance.CheckWin();
            });
            GridCellManager.instance.SetPlacedCell(cellToMove, hitObject);
            hitObject = null;
            axisMovementInput.SetDefaultMoveDirection();
        }
    }

    private Vector3Int GetCellToMoveOperation(GameObject operation, Vector2 direction)
    {
        Vector3Int defaultPos = GridCellManager.instance.GetObjCell(operation.transform.position);
        Vector3Int cellToMove = defaultPos;
        Vector3Int next = defaultPos + Vector3Int.FloorToInt(direction);

        if(!GridCellManager.instance.IsPlaceableArea(next))
        {
            return cellToMove;
        }

        Vector3 nextPos = GridCellManager.instance.PositonToMove(next);
        Collider2D collider2D = Physics2D.OverlapPoint(nextPos, LayerMask.GetMask("MathObject"));
        while (collider2D == null)
        {
            cellToMove = next;
            next += Vector3Int.FloorToInt(direction);
            if (!GridCellManager.instance.IsPlaceableArea(next))
            {
                return cellToMove;
            }
            nextPos = GridCellManager.instance.PositonToMove(next);
            collider2D = Physics2D.OverlapPoint(nextPos, LayerMask.GetMask("MathObject"));
        }

        return cellToMove;
    }

    public GameObject GetHitObject()
    {
        return hitObject;
    }

    private GameObject CastRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }

        return null;
    }
}
