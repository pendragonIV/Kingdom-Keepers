using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Operation : MonoBehaviour
{
    [SerializeField]
    private BlockType blockType;
    [SerializeField]
    private OperatorType operatorType;
    [SerializeField]
    private int numberValue;
    [SerializeField]
    private bool _isMoveable;

    public bool IsOperationMoveable()
    {
        return _isMoveable;
    }

    private void Awake()
    {
        if(blockType == BlockType.Number)
        {
            operatorType = OperatorType.None;
        }
        else if(blockType == BlockType.Operator)
        {
            numberValue = 0;
        }
    }

    private void Start()
    {
        TMP_Text text = this.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();

        if (blockType == BlockType.Number)
        {
            text.text = numberValue.ToString();
        }
        else if (blockType == BlockType.Operator)
        {
            text.text = Calculator.instance.GetOperatorSign(operatorType);
        }
    }

    public BlockType GetBlockType()
    {
        return blockType;
    }

    public OperatorType GetOperatorType()
    {
        return operatorType;
    }

    public int GetNumberValue()
    {
        return numberValue;
    }
}
