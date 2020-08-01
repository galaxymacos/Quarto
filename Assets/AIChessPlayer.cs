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
    public int searchDepth;

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
        ChessPlayer.instance.currentPickChess = ChessBoard.instance.availableWhiteChess[randomIndex];
        ChessBoard.instance.availableWhiteChess.RemoveAt(randomIndex);
        TurnManager.instance.currentState = TurnManager.State.PlayerMove;
    }

    private void PlaceChess()
    {
        // TODO add it back
        var bestMove = FindBestMove(ChessBoard.instance.board);
        ChessBoard.instance.board[bestMove.row, bestMove.col] = currentPickChess;
        ChessBoard.instance.availableBlackChess.Remove(currentPickChess);
        ChessBoard.instance.RefreshBoard();
        TurnManager.instance.currentState = TurnManager.State.AIPickForPlayer;
        PickChessForPlayer();

        // for (int i = 0; i < ChessBoard.instance.board.GetLength(0); i++)
        // {
            // for (int j = 0; j < ChessBoard.instance.board.GetLength(1); j++)
            // {
                // if (ChessBoard.instance.board[i, j] == null)
                // {
                    // ChessBoard.instance.board[i, j] = currentPickChess;
                    // print("Drop chess onto board");
                    // ChessBoard.instance.availableBlackChess.Remove(currentPickChess);
                    // goto OuterLoop;
                // }
            // }
        // }

        // OuterLoop:
        // ChessBoard.instance.RefreshBoard();
        // print("hey");
        // TurnManager.instance.currentState = TurnManager.State.AIPickForPlayer;
        // PickChessForPlayer();
        // print("bro");
    }

    private void React(TurnManager.State newState)
    {
        switch (newState)
        {
            case TurnManager.State.AIPickForPlayer:
                print("???");
                PickChessForPlayer();
                break;
            case TurnManager.State.AIMove:
                PlaceChess();
                break;
        }
    }


    public int Evaluate(ChessInfo[,] b)
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
                    return color == ChessType.Black ? 1000 : -1000;
                }
            }

            for (int j = 0; j < b.GetLength(1); j++)
            {
                if (b[i, j] != null && b[i, j].height != height)
                    break;
                if (j == b.GetLength(1) - 1)
                {
                    return color == ChessType.Black ? 1000 : -1000;
                }
            }

            for (int j = 0; j < b.GetLength(1); j++)
            {
                if (b[i, j] != null && b[i, j].hasCircleOnTop != hasCircleOnTop)
                    break;
                if (j == b.GetLength(1) - 1)
                {
                    return color == ChessType.Black ? 1000 : -1000;
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
                    return color == ChessType.Black ? 1000 : -1000;
                }
            }

            for (int j = 0; j < b.GetLength(1); j++)
            {
                if (b[j, i] != null && b[j, i].height != height)
                    break;
                if (j == b.GetLength(1) - 1)
                {
                    return color == ChessType.Black ? 1000 : -1000;
                }
            }

            for (int j = 0; j < b.GetLength(1); j++)
            {
                if (b[j, i] != null && b[j, i].hasCircleOnTop != hasCircleOnTop)
                    break;
                if (j == b.GetLength(1) - 1)
                {
                    return color == ChessType.Black ? 1000 : -1000;
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
                    return color == ChessType.Black ? 1000 : -1000;
                }
            }

            for (int j = 0; j < b.GetLength(0); j++)
            {
                if (b[j, j] != null && b[j, j].height != height)
                    break;
                if (j == b.GetLength(1) - 1)
                {
                    return color == ChessType.Black ? 1000 : -1000;
                }
            }

            for (int j = 0; j < b.GetLength(0); j++)
            {
                if (b[j, j] != null && b[j, j].hasCircleOnTop != hasCircleOnTop)
                    break;
                if (j == b.GetLength(1) - 1)
                {
                    return color == ChessType.Black ? 1000 : -1000;
                }
            }
        }

        if (b[0, b.GetLength(0) - 1] != null)
        {
            var baseShape = b[0, b.GetLength(0) - 1].baseShape;
            var height = b[0, b.GetLength(0) - 1].height;
            var hasCircleOnTop = b[0, b.GetLength(0) - 1].hasCircleOnTop;
            var color = b[0, b.GetLength(0) - 1].chessType;
            for (int j = 0; j < b.GetLength(0); j++)
            {
                if (b[j, b.GetLength(0) - 1 - j] != null && b[j, b.GetLength(0) - 1 - j].baseShape != baseShape)
                    break;
                if (j == b.GetLength(1) - 1)
                {
                    return color == ChessType.Black ? 1000 : -1000;
                }
            }

            for (int j = 0; j < b.GetLength(0); j++)
            {
                if (b[j, b.GetLength(0) - 1 - j] != null && b[j, b.GetLength(0) - 1 - j].height != height)
                    break;
                if (j == b.GetLength(1) - 1)
                {
                    return color == ChessType.Black ? 1000 : -1000;
                }
            }

            for (int j = 0; j < b.GetLength(0); j++)
            {
                if (b[j, b.GetLength(0) - 1 - j] != null &&
                    b[j, b.GetLength(0) - 1 - j].hasCircleOnTop != hasCircleOnTop)
                    break;
                if (j == b.GetLength(1) - 1)
                {
                    return color == ChessType.Black ? 1000 : -1000;
                }
            }
        }

        return 0;
    }

    private int minimaxTime;

    public int MiniMax(ChessInfo[,] board, int depth, bool isMax)
    {
        print("hey dude");
        minimaxTime++;
        Console.Write("0");
        // Max depth is set in main menu
        if (depth >= searchDepth)
            return 0;
        Console.Write("1");
        int score = Evaluate(board);

        if (score == 1000 || score == -1000)
            return score;
        Console.Write("2");
        if (ChessBoard.instance.IsFull())
            return 0;
        Console.Write("3");
        if (isMax)
        {
            int best = -1000;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (board[i, j] == null)
                    {
                        var randomBlackChess =
                            ChessBoard.instance.availableBlackChess[
                                Random.Range(0, ChessBoard.instance.availableBlackChess.Count)];
                        board[i, j] = randomBlackChess;
                        ChessBoard.instance.availableBlackChess.Remove(randomBlackChess);
                        best = Mathf.Max(best, MiniMax(board, depth + 1, false));
                        ChessBoard.instance.availableBlackChess.Add(board[i, j]);
                        board[i, j] = null;
                    }
                }
            }

            return best;
        }
        else // Player is trying to minimize the opponent's gain
        {
            int best = 1000;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    // Find an empty place to place a chess
                    if (board[i, j] == null)
                    {
                        var randomWhiteChess =
                            ChessBoard.instance.availableWhiteChess[
                                Random.Range(0, ChessBoard.instance.availableWhiteChess.Count)];
                        board[i, j] = randomWhiteChess;
                        ChessBoard.instance.availableWhiteChess.Remove(board[i, j]);
                        best = Mathf.Min(best, MiniMax(board, depth + 1, true));
                        ChessBoard.instance.availableWhiteChess.Add(board[i, j]);
                        board[i, j] = null;
                    }
                }
            }
            Console.WriteLine("4");

            return best;
        }
    }

    public class Move
    {
        public int row;
        public int col;
    }

    public Move FindBestMove(ChessInfo[,] board)
    {
        print("AI is thinking...");
        int bestVal = -1000;
        Move bestMove = new Move();
        bestMove.row = -1;
        bestMove.col = -1;

        // Traverse all cells, evaluate minimax function  
        // for all empty cells. And return the cell  
        // with optimal value.
        int passTime = 0;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                // Check if cell is empty 
                if (board[i, j] == null)
                {
                    passTime++;
                    board[i, j] = currentPickChess;
                    ChessBoard.instance.availableBlackChess.Remove(currentPickChess);
                    minimaxTime = 0;
                    int moveVal = MiniMax(board, 0, false);
                    print("Minimax time: "+minimaxTime);
                    minimaxTime = 0;
                    ChessBoard.instance.availableBlackChess.Add(currentPickChess);

                    if (moveVal > bestVal)
                    {
                        bestMove.row = i;
                        bestMove.col = j;
                        bestVal = moveVal;
                    }

                    board[i, j] = null;
                }
            }
        }
        
        print("how many place to choose from: "+passTime);



        print("AI finished thinking...");
        return bestMove;
    }
}