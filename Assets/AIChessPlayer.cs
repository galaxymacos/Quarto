using System;
using System.Collections;
using System.Linq;
using Packages.Rider.Editor.UnitTesting;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIChessPlayer : MonoBehaviour
{
    public static AIChessPlayer instance;
    public ChessInfo currentPickChess;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        TurnManager.instance.onStateChange += React;
    }

    private void OnDestroy()
    {
        TurnManager.instance.onStateChange -= React;
    }

    public void PickChessForPlayer()
    {
        print("Pick Chess For Player");
        int randomIndex = Random.Range(0, ChessBoard.instance.availableWhiteChess.Count);
        ChessPlayer.instance.currentPickedChess = ChessBoard.instance.availableWhiteChess[randomIndex];
        TurnManager.instance.currentState = TurnManager.State.PlayerMove;
        ChessBoard.instance.availableWhiteChess.RemoveAt(randomIndex);
        if (ChessPlayer.instance.currentPickedChess == null)
        {
            Debug.LogError("AI didn't pick any chess for player");
        }
        print("Player Move");
    }

    private IEnumerator PlaceChess()
    {
        yield return new WaitForSeconds(1f);

        var bestMove = findBestMove(ChessBoard.instance.board);
        ChessBoard.instance.board[bestMove.row, bestMove.col] = bestMove.chess;
        ChessBoard.instance.RefreshBoard();
        currentPickChess = null;
        TurnManager.instance.currentState = TurnManager.State.AIPickForPlayer;
    }

    private void React(TurnManager.State newState)
    {
        switch (newState)
        {
            case TurnManager.State.AIPickForPlayer:
                PickChessForPlayer();
                break;
            case TurnManager.State.AIMove:
                StartCoroutine(PlaceChess());
                break;
        }
    }

    public bool hasMoveLeft()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (ChessBoard.instance.board[i, j] == null)
                    return true;
            }
        }

        return false;
    }

    public int Evaluate(ChessInfo[,] b)
    {
        for (int i = 0; i < b.GetLength(0); i++)
        {
            if (b[i, 0] == null) continue;
            var baseShape = b[i, 0].baseShape;
            var height = b[i, 0].height;
            var hasCircleOnTop = b[i, 0].hasCircleOnTop;
            var color = b[i, 0].chessColor;
            for (int j = 0; j < b.GetLength(1); j++)
            {
                if (b[i,j] != null && b[i, j].baseShape != baseShape)
                    break;
                if (j == b.GetLength(1) - 1)
                {
                    return color == ChessColor.Black ? 1000 : -1000;
                }
            }

            for (int j = 0; j < b.GetLength(1); j++)
            {
                if (b[i,j] != null && b[i, j].height != height)
                    break;
                if (j == b.GetLength(1) - 1)
                {
                    return color == ChessColor.Black ? 1000 : -1000;
                }
            }

            for (int j = 0; j < b.GetLength(1); j++)
            {
                if (b[i,j] != null && b[i, j].hasCircleOnTop != hasCircleOnTop)
                    break;
                if (j == b.GetLength(1) - 1)
                {
                    return color == ChessColor.Black ? 1000 : -1000;
                }
            }
        }

        for (int i = 0; i < b.GetLength(0); i++)
        {
            if (b[0,i] == null) continue;
            var baseShape = b[0, i].baseShape;
            var height = b[0, i].height;
            var hasCircleOnTop = b[0, i].hasCircleOnTop;
            var color = b[0, i].chessColor;
            for (int j = 0; j < b.GetLength(1); j++)
            {
                if (b[j,i] != null && b[j, i].baseShape != baseShape)
                    break;
                if (j == b.GetLength(1) - 1)
                {
                    return color == ChessColor.Black ? 1000 : -1000;
                }
            }

            for (int j = 0; j < b.GetLength(1); j++)
            {
                if (b[j,i] != null && b[j, i].height != height)
                    break;
                if (j == b.GetLength(1) - 1)
                {
                    return color == ChessColor.Black ? 1000 : -1000;
                }
            }

            for (int j = 0; j < b.GetLength(1); j++)
            {
                if (b[j,i] != null && b[j, i].hasCircleOnTop != hasCircleOnTop)
                    break;
                if (j == b.GetLength(1) - 1)
                {
                    return color == ChessColor.Black ? 1000 : -1000;
                }
            }
        }

        if (b[0, 0] != null)
        {
            var baseShape = b[0, 0].baseShape;
            var height = b[0, 0].height;
            var hasCircleOnTop = b[0, 0].hasCircleOnTop;
            var color = b[0, 0].chessColor;
            for (int j = 0; j < b.GetLength(0); j++)
            {
                if (b[j,j] != null &&b[j, j].baseShape != baseShape)
                    break;
                if (j == b.GetLength(1) - 1)
                {
                    return color == ChessColor.Black ? 1000 : -1000;
                }
            }

            for (int j = 0; j < b.GetLength(0); j++)
            {
                if (b[j,j] != null && b[j, j].height != height)
                    break;
                if (j == b.GetLength(1) - 1)
                {
                    return color == ChessColor.Black ? 1000 : -1000;
                }
            }

            for (int j = 0; j < b.GetLength(0); j++)
            {
                if (b[j,j] != null && b[j, j].hasCircleOnTop != hasCircleOnTop)
                    break;
                if (j == b.GetLength(1) - 1)
                {
                    return color == ChessColor.Black ? 1000 : -1000;
                }
            }
        }

        if (b[0, b.GetLength(0) - 1]!=null)
        {
            var baseShape = b[0, b.GetLength(0) - 1].baseShape;
            var height = b[0, b.GetLength(0) - 1].height;
            var hasCircleOnTop = b[0, b.GetLength(0) - 1].hasCircleOnTop;
            var color = b[0, b.GetLength(0) - 1].chessColor;
            for (int j = 0; j < b.GetLength(0); j++)
            {
                if (b[j,b.GetLength(0)-1-j] != null && b[j, b.GetLength(0) - 1 - j].baseShape != baseShape)
                    break;
                if (j == b.GetLength(1) - 1)
                {
                    return color == ChessColor.Black ? 1000 : -1000;
                }
            }

            for (int j = 0; j < b.GetLength(0); j++)
            {
                if (b[j,b.GetLength(0)-1-j]!=null && b[j, b.GetLength(0) - 1 - j].height != height)
                    break;
                if (j == b.GetLength(1) - 1)
                {
                    return color == ChessColor.Black ? 1000 : -1000;
                }
            }

            for (int j = 0; j < b.GetLength(0); j++)
            {
                if (b[j,b.GetLength(0)-1-j]!=null && b[j, b.GetLength(0) - 1 - j].hasCircleOnTop != hasCircleOnTop)
                    break;
                if (j == b.GetLength(1) - 1)
                {
                    return color == ChessColor.Black ? 1000 : -1000;
                }
            }
        }

        return 0;
    }

    public int MiniMax(ChessInfo[,] board, int depth, bool isMax)
    {
        // Max depth is 3
        if (depth >= 3)
            return 0;
        
        int score = Evaluate(board);

        if (score == 1000 || score == -1000)
            return score;

        if (!hasMoveLeft())
            return 0;

        if (isMax)
        {
            int best = -1000;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (board[i, j] == null)
                    {
                            board[i, j] = currentPickChess;
                            ChessBoard.instance.availableBlackChess.Remove(currentPickChess);
                            best = Mathf.Max(best, MiniMax(board, depth + 1, false));
                            ChessBoard.instance.availableBlackChess.Add(board[i, j]);
                            board[i, j] = null;
                    }
                }
            }

            return best;
        }
        else
        {
            int best = 1000;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (board[i, j] == null)
                    {
                            board[i, j] = ChessPlayer.instance.currentPickedChess;
                            ChessBoard.instance.availableWhiteChess.Remove(currentPickChess);
                            best = Mathf.Max(best, MiniMax(board, depth + 1, true));
                            ChessBoard.instance.availableWhiteChess.Add(board[i, j]);
                            board[i, j] = null;
                    }
                }
            }

            return best;
        }
    }

    public class Move
    {
        public int row;
        public int col;
        public ChessInfo chess;
    }

    public Move findBestMove(ChessInfo[,] board)
    {
        print("AI is thinking...");
        int bestVal = -1000;
        Move bestMove = new Move();
        bestMove.row = -1;
        bestMove.col = -1;

        // Traverse all cells, evaluate minimax function  
        // for all empty cells. And return the cell  
        // with optimal value. 
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                // Check if cell is empty 
                if (board[i, j] == null)
                {
                    board[i, j] = currentPickChess;
                    ChessBoard.instance.availableBlackChess.Remove(currentPickChess);
                    int moveVal = MiniMax(board, 0, false);

                    ChessBoard.instance.availableBlackChess.Add(currentPickChess);

                    if (moveVal > bestVal)
                    {
                        bestMove.row = i;
                        bestMove.col = j;
                        bestMove.chess = board[i, j];
                        bestVal = moveVal;
                    }

                    board[i, j] = null;
                }
            }
        }


        print("AI finished thinking...");
        return bestMove;
    }
}