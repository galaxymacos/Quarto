using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ChessBoard : SerializedMonoBehaviour
{
    public static ChessBoard instance;
    public List<ChessInfo> availableWhiteChess;
    public List<ChessInfo> availableBlackChess;
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
        RefreshBoard();
    }


    public void RefreshBoard()
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