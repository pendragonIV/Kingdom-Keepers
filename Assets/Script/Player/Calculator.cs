using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OperatorType
{
    None,
    Plus,
    Minus,
    Multiply,
    Divide,
    Equal
}

public enum BlockType
{
    Number,
    Operator
}

public class Calculator : MonoBehaviour
{
    public static Calculator instance;
    private const string OBJECT_TAG = "MathObject";
    [SerializeField]
    private Sprite[] operatorSprites;
    [SerializeField]
    private Sprite[] numberSprites;
    [SerializeField]
    private Sprite[] resultSprites;

    [SerializeField]
    private List<GameObject> verticalNumber = new List<GameObject>();
    [SerializeField]
    private List<GameObject> horizontalNumber = new List<GameObject>();

    [SerializeField]
    private Transform equalContainer;
    private List<bool> isWin = new List<bool>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void CheckWin()
    {
        isWin.Clear();
        for(int i = 0; i < equalContainer.childCount; i++)
        {
            GetNearbyNumbers(equalContainer.GetChild(i).gameObject, GridCellManager.instance.GetObjCell(equalContainer.GetChild(i).position));
        }
        bool isWinGame = false;
        Debug.Log(isWin.Count);
        foreach(bool win in isWin)
        {
            isWinGame = win;
            if (!win)
            {
                break;
            }
        }
        if (isWinGame)
        {
            GameManager.instance.PlayerWinThisLevel();
        }
    }

    public void GetNearbyNumbers(GameObject objToCheck, Vector3Int positionToCheck)
    {
        verticalNumber.Clear();
        horizontalNumber.Clear();

        if (objToCheck)
        {
            horizontalNumber.Add(objToCheck);
            verticalNumber.Add(objToCheck);
        }

        Vector3Int nextLeft = positionToCheck + Vector3Int.left;
        Vector3Int nextRight = positionToCheck + Vector3Int.right;
        Vector3Int nextUp = positionToCheck + Vector3Int.up;
        Vector3Int nextDown = positionToCheck + Vector3Int.down;

        while (GridCellManager.instance.IsPlacedCell(nextRight) && GridCellManager.instance.IsCalculateArea(nextRight))
        {
            GameObject next = GridCellManager.instance.GetPlacedObj(nextRight);
            if (next.CompareTag(OBJECT_TAG))
            {
                horizontalNumber.Add(next);
                nextRight += Vector3Int.right;
            }
            else
            {
                break;
            }

        }

        while (GridCellManager.instance.IsPlacedCell(nextLeft) && GridCellManager.instance.IsCalculateArea(nextLeft))
        {
            GameObject next = GridCellManager.instance.GetPlacedObj(nextLeft);
            if (next.CompareTag(OBJECT_TAG))
            {
                horizontalNumber.Insert(0, next);
                nextLeft += Vector3Int.left;
            }
            else
            {
                break;
            }

        }

        while (GridCellManager.instance.IsPlacedCell(nextUp) && GridCellManager.instance.IsCalculateArea(nextUp))
        {
            GameObject next = GridCellManager.instance.GetPlacedObj(nextUp);
            if (next.CompareTag(OBJECT_TAG))
            {
                verticalNumber.Add(next);
                nextUp += Vector3Int.up;
            }
            else
            {
                break;
            }

        }

        while (GridCellManager.instance.IsPlacedCell(nextDown) && GridCellManager.instance.IsCalculateArea(nextDown))
        {
            GameObject next = GridCellManager.instance.GetPlacedObj(nextDown);
            if (next.CompareTag(OBJECT_TAG))
            {
                verticalNumber.Insert(0, next);
                nextDown += Vector3Int.down;
            }
            else
            {
                break;
            }

        }

        if (horizontalNumber.Count > 1)
        {
            string number = "";

            for (int i = 0; i < horizontalNumber.Count; i++)
            {
                if (horizontalNumber[i].GetComponent<Operation>().GetBlockType() == BlockType.Operator)
                {
                    number += GetOperatorSign(horizontalNumber[i].GetComponent<Operation>().GetOperatorType());
                }
                else
                {
                    number += horizontalNumber[i].GetComponent<Operation>().GetNumberValue().ToString();
                }
            }
            isWin.Add(Calculate(number));
        }
        else if (verticalNumber.Count > 1)
        {
            string number = "";

            for (int i = 0; i < verticalNumber.Count; i++)
            {
                if (verticalNumber[i].GetComponent<Operation>().GetBlockType() == BlockType.Operator)
                {
                    number += GetOperatorSign(verticalNumber[i].GetComponent<Operation>().GetOperatorType());
                }
                else
                {
                    number += verticalNumber[i].GetComponent<Operation>().GetNumberValue().ToString();
                }
            }
            isWin.Add(Calculate(number));
        }
        else
        {
            isWin.Add(false);
        }
    }

    public string GetOperatorSign(OperatorType op)
    {
        if (op == OperatorType.Plus)
        {
            return "+";
        }
        else if (op == OperatorType.Minus)
        {
            return "-";
        }
        else if (op == OperatorType.Multiply)
        {
            return "*";
        }
        else if (op == OperatorType.Divide)
        {
            return "/";
        }
        else if(op == OperatorType.Equal)
        {
            return "=";
        }
        return "";
    }

    private bool Calculate(string numberString)
    {
        Debug.Log(numberString);
        string[] numbers = numberString.Split('+', '-', '*', '/', '=');

        int start = 1;
        int result = 0;
        List<int> resultList = new List<int>();

        if (numberString[0] == '-' || numberString[0] == '+' || numberString[0] == '*' || numberString[0] == '/')
        {
            if(numbers.Length == 2)
            {
                return false;
            }
            string temp = numberString[0] + numbers[1];
            result = int.Parse(temp);
            start = 2;
        }
        else
        {
            if(numbers.Length == 1)
            {
                return false;
            }
            result = int.Parse(numbers[0]);
        }

        int index = start;

        for(int i = start; i < numberString.Length; i++)
        {
            if (numberString[i] == '+')
            {
                result += int.Parse(numbers[index]);
                index++;
            }
            else if (numberString[i] == '-')
            {
                result -= int.Parse(numbers[index]);
                index++;
            }
            else if (numberString[i] == '*')
            {
                result *= int.Parse(numbers[index]);
                index++;
            }
            else if (numberString[i] == '/')
            {
                if (int.Parse(numbers[index]) == 0)
                {
                    Debug.Log("Divide by zero");
                }
                result /= int.Parse(numbers[index]);
                index++;
            }
            else if (numberString[i] == '=')
            {
                resultList.Add(result);
                result = 0;
                if (index >= numbers.Length)
                {
                    break;
                }
                result = int.Parse(numbers[index]);
                Debug.Log("result: " + result);
                index++;
            }
        }

        if (result != 0)
        {
            resultList.Add(result);
        }

        bool isEqual = true;
        int startResult = resultList[0];
        if(resultList.Count == 1)
        {
            return false;
        }

        for(int i = 0; i < resultList.Count; i++)
        {
            if (resultList[i] != startResult)
            {
                isEqual = false;
                break;
            }
        }

        if (isEqual)
        {
            return true;
        }
        return false;
    }

    #region Get

    public Sprite GetOperatorSprite(OperatorType operatorType)
    {
        return operatorSprites[(int)operatorType - 1];
    }

    public Sprite GetNumberSprite(int number)
    {
        return numberSprites[number - 1];
    }

    public Sprite GetResultSprite(int number)
    {
        return resultSprites[number - 1];
    }

    #endregion
}
