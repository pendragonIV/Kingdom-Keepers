using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterCell : MonoBehaviour
{
    private void Start()
    {
        SetCenterCell();
    }
    private void SetCenterCell()
    {
        Vector3Int cellPos = GridCellManager.instance.GetObjCell(transform.position);
        this.transform.position = GridCellManager.instance.PositonToMove(cellPos);
        GridCellManager.instance.SetPlacedCell(cellPos, this.gameObject);
    }
}
