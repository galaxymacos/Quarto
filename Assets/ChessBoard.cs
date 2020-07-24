using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class ChessBoard : SerializedMonoBehaviour
{
    public static ChessBoard instance;
    public ChessInfo[,] board = new ChessInfo[4,4];
    public Transform[,] cellPoses = new Transform[4,4];

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        OrganizeBoard();
    }


    public void OrganizeBoard()
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j] != null)
                {
                    board[i, j].gameObject.transform.position = cellPoses[i, j].position;
                }
            }
        }
    }
    
    
    
}

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;
    public TextMeshProUGUI chessPickIndex;
    public TextMeshProUGUI dropCol;
    public TextMeshProUGUI dropRow;

    public GameObject[] availableChess;

    public GameObject currentPickedChess;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    
    
    // Select Chess
    public void OnConfirm()
    {
        int index = Convert.ToInt32(chessPickIndex.text);
        if (availableChess[index] != null)
        {
            currentPickedChess = availableChess[index];
        }
        else
        {
            Debug.Log("There is no chess at index "+index);
            return;
        }
    }

    public void OnConfirmDropPlace()
    {
        if (ChessBoard.instance.board[Convert.ToInt32(dropRow), Convert.ToInt32(dropCol)] == null)
        {
            ChessBoard.instance.board[Convert.ToInt32(dropRow), Convert.ToInt32(dropCol)] = currentPickedChess.GetComponent<ChessInfo>();
        }
    }
    
    
}