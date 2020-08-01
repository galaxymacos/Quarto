using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ChessBoard : SerializedMonoBehaviour
{
    public static ChessBoard instance;
    public List<ChessInfo> availableWhiteChess;
    public List<ChessInfo> availableBlackChess;
    public ChessInfo[,] board = new ChessInfo[4, 4];
    public Transform[,] cellPoses = new Transform[4, 4];

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

    
    public ChessType CheckVictoryCondition(ChessInfo[,] b)
    {
        for (int i = 0; i < b.GetLength(0); i++)
        {
            if (b[i, 0] == null) continue;
            var baseShape = b[i, 0].baseShape;
            var height = b[i, 0].height;
            var hasCircleOnTop = b[i, 0].hasCircleOnTop;
            var color = b[i, 0].chessType;
            for (int j = 0; j < b.GetLength(1); j++)
            {
                if (b[i, j] != null && b[i, j].baseShape != baseShape)
                    break;
                if (j == b.GetLength(1) - 1)
                {
                    return color;
                }
            }

            for (int j = 0; j < b.GetLength(1); j++)
            {
                if (b[i, j] != null && b[i, j].height != height)
                    break;
                if (j == b.GetLength(1) - 1)
                {
                    return color;
                }
            }

            for (int j = 0; j < b.GetLength(1); j++)
            {
                if (b[i, j] != null && b[i, j].hasCircleOnTop != hasCircleOnTop)
                    break;
                if (j == b.GetLength(1) - 1)
                {
                    return color;
                }
            }
        }

        for (int i = 0; i < b.GetLength(0); i++)
        {
            if (b[0, i] == null) continue;
            var baseShape = b[0, i].baseShape;
            var height = b[0, i].height;
            var hasCircleOnTop = b[0, i].hasCircleOnTop;
            var color = b[0, i].chessType;
            for (int j = 0; j < b.GetLength(1); j++)
            {
                if (b[j, i] != null && b[j, i].baseShape != baseShape)
                    break;
                if (j == b.GetLength(1) - 1)
                {
                    return color;
                }
            }

            for (int j = 0; j < b.GetLength(1); j++)
            {
                if (b[j, i] != null && b[j, i].height != height)
                    break;
                if (j == b.GetLength(1) - 1)
                {
                    return color;
                }
            }

            for (int j = 0; j < b.GetLength(1); j++)
            {
                if (b[j, i] != null && b[j, i].hasCircleOnTop != hasCircleOnTop)
                    break;
                if (j == b.GetLength(1) - 1)
                {
                    return color;
                }
            }
        }

        if (b[0, 0] != null)
        {
            var baseShape = b[0, 0].baseShape;
            var height = b[0, 0].height;
            var hasCircleOnTop = b[0, 0].hasCircleOnTop;
            var color = b[0, 0].chessType;
            for (int j = 0; j < b.GetLength(0); j++)
            {
                if (b[j, j] != null && b[j, j].baseShape != baseShape)
                    break;
                if (j == b.GetLength(1) - 1)
                {
                    return color;
                }
            }

            for (int j = 0; j < b.GetLength(0); j++)
            {
                if (b[j, j] != null && b[j, j].height != height)
                    break;
                if (j == b.GetLength(1) - 1)
                {
                    return color;
                }
            }

            for (int j = 0; j < b.GetLength(0); j++)
            {
                if (b[j, j] != null && b[j, j].hasCircleOnTop != hasCircleOnTop)
                    break;
                if (j == b.GetLength(1) - 1)
                {
                    return color;
                }
            }
        }

        if (b[0, b.GetLength(0) - 1] != null)
        {
            var baseShape = b[0, b.GetLength(0) - 1].baseShape;
            var height = b[0, b.GetLength(0) - 1].height;
            var hasCircleOnTop = b[0, b.GetLength(0) - 1].hasCircleOnTop;
            ChessType color = b[0, b.GetLength(0) - 1].chessType;
            for (int j = 0; j < b.GetLength(0); j++)
            {
                if (b[j, b.GetLength(0) - 1 - j] != null && b[j, b.GetLength(0) - 1 - j].baseShape != baseShape)
                    break;
                if (j == b.GetLength(1) - 1)
                {
                    return color;
                }
            }

            for (int j = 0; j < b.GetLength(0); j++)
            {
                if (b[j, b.GetLength(0) - 1 - j] != null && b[j, b.GetLength(0) - 1 - j].height != height)
                    break;
                if (j == b.GetLength(1) - 1)
                {
                    return color;
                }
            }

            for (int j = 0; j < b.GetLength(0); j++)
            {
                if (b[j, b.GetLength(0) - 1 - j] != null &&
                    b[j, b.GetLength(0) - 1 - j].hasCircleOnTop != hasCircleOnTop)
                    break;
                if (j == b.GetLength(1) - 1)
                {
                    return color;
                }
            }
        }

        return ChessType.Null;
    }

    public bool IsFull()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (board[i, j] == null)
                    return false;
            }
        }

        return true;
    }
}