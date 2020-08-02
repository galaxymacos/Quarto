using System;
using System.Collections;
using System.Collections.Generic;
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
        if (ChessBoard.instance.availableWhiteChess.Count == 0)
        {
            print("There is no more chess available");
            print("Game over");
            return;
        }
        ChessPlayer.instance.currentPickChess = ChessBoard.instance.availableWhiteChess[0];
        ChessBoard.instance.availableWhiteChess.RemoveAt(0);
        TurnManager.instance.currentState = TurnManager.State.PlayerMove;
    }

    private void PlaceChess()
    {
        if (ChessBoard.instance.IsFull())
        {
            print("Board is full, game over, no winner");
            return;
        }
        // TODO add it back
        var bestMove = FindBestMove();
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


    private List<Tuple<int,int>> GetRidOfUnqualifiedPlaces()
    {
        List<Tuple<int, int>> result = new List<Tuple<int, int>>();
        
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                // top left
                if (i >= 1 && j >= 1)
                {
                    if (ChessBoard.instance.board[i - 1, j - 1] != null)
                    {
                        result.Add(new Tuple<int, int>(i, j));
                        continue;
                    }
                }

                // left
                if (i >= 1)
                {
                    if (ChessBoard.instance.board[i - 1, j] != null)
                    {
                        result.Add(new Tuple<int, int>(i, j));
                        continue;
                    }
                }
                
                // bottom left
                if (i >= 1 && j <= 2)
                {
                    if (ChessBoard.instance.board[i - 1, j+1] != null)
                    {
                        result.Add(new Tuple<int, int>(i, j));
                        continue;
                    }
                }
                
                // top right
                if (i <=2  && j >= 1)
                {
                    if (ChessBoard.instance.board[i + 1, j - 1] != null)
                    {
                        result.Add(new Tuple<int, int>(i, j));
                        continue;
                    }
                }

                // right
                if (i <= 2)
                {
                    if (ChessBoard.instance.board[i + 1, j] != null)
                    {
                        result.Add(new Tuple<int, int>(i, j));
                        continue;
                    }
                }
                
                // bottom right
                if (i <= 2 && j <= 2)
                {
                    if (ChessBoard.instance.board[i+1, j+1] != null)
                    {
                        result.Add(new Tuple<int, int>(i, j));
                        continue;
                    }
                }
                
                // top
                if (j >= 1)
                {
                    if (ChessBoard.instance.board[i, j - 1] != null)
                    {
                        result.Add(new Tuple<int, int>(i, j));
                        continue;
                    }
                }
                
                // bottom
                if (j <= 2)
                {
                    if (ChessBoard.instance.board[i, j + 1] != null)
                    {
                        result.Add(new Tuple<int, int>(i, j));
                        continue;
                    }
                }
                
            }
        }

        return result;
    }

    public int Evaluate()
    {

        for (int i = 0; i < ChessBoard.instance.board.GetLength(0); i++)
        {
            if (ChessBoard.instance.board[i, 0] == null) continue;
            var baseShape = ChessBoard.instance.board[i, 0].baseShape;
            var height = ChessBoard.instance.board[i, 0].height;
            var isConcave = ChessBoard.instance.board[i, 0].IsConcave;
            var color = ChessBoard.instance.board[i, 0].chessType;
            for (int j = 0; j < ChessBoard.instance.board.GetLength(1); j++)
            {
                if (ChessBoard.instance.board[i, j] == null || ChessBoard.instance.board[i, j].baseShape != baseShape)
                {
                    break;
                }
                if (j == ChessBoard.instance.board.GetLength(1) - 1)
                {
                    switch (color)
                    {
                        case ChessType.Black:
                            return 1000;
                        case ChessType.White:
                            return -1000;
                    }                }
            }

            for (int j = 0; j < ChessBoard.instance.board.GetLength(1); j++)
            {
                if (ChessBoard.instance.board[i, j] == null || ChessBoard.instance.board[i, j].height != height)
                    break;
                if (j == ChessBoard.instance.board.GetLength(1) - 1)
                {
                    switch (color)
                    {
                        case ChessType.Black:
                            return 1000;
                        case ChessType.White:
                            return -1000;
                    }
                }
            }

            for (int j = 0; j < ChessBoard.instance.board.GetLength(1); j++)
            {
                if (ChessBoard.instance.board[i, j] == null || ChessBoard.instance.board[i, j].IsConcave != isConcave)
                    break;
                if (j == ChessBoard.instance.board.GetLength(1) - 1)
                {
                    switch (color)
                    {
                        case ChessType.Black:
                            return 1000;
                        case ChessType.White:
                            return -1000;
                    }
                }
            }
        }

        for (int i = 0; i < ChessBoard.instance.board.GetLength(0); i++)
        {
            if (ChessBoard.instance.board[0, i] == null) continue;
            var baseShape = ChessBoard.instance.board[0, i].baseShape;
            var height = ChessBoard.instance.board[0, i].height;
            var hasCircleOnTop = ChessBoard.instance.board[0, i].IsConcave;
            var color = ChessBoard.instance.board[0, i].chessType;
            for (int j = 0; j < ChessBoard.instance.board.GetLength(1); j++)
            {
                if (ChessBoard.instance.board[j, i] == null || ChessBoard.instance.board[j, i].baseShape != baseShape)
                    break;
                if (j == ChessBoard.instance.board.GetLength(1) - 1)
                {
                    switch (color)
                    {
                        case ChessType.Black:
                            return 1000;
                        case ChessType.White:
                            return -1000;
                    }
                }
            }

            for (int j = 0; j < ChessBoard.instance.board.GetLength(1); j++)
            {
                if (ChessBoard.instance.board[j, i] == null || ChessBoard.instance.board[j, i].height != height)
                    break;
                if (j == ChessBoard.instance.board.GetLength(1) - 1)
                {
                    switch (color)
                    {
                        case ChessType.Black:
                            return 1000;
                        case ChessType.White:
                            return -1000;
                    }
                }
            }

            for (int j = 0; j < ChessBoard.instance.board.GetLength(1); j++)
            {
                if (ChessBoard.instance.board[j, i] == null || ChessBoard.instance.board[j, i].IsConcave != hasCircleOnTop)
                    break;
                if (j == ChessBoard.instance.board.GetLength(1) - 1)
                {
                    switch (color)
                    {
                        case ChessType.Black:
                            return 1000;
                        case ChessType.White:
                            return -1000;
                    }
                }
            }
        }

        if (ChessBoard.instance.board[0, 0] != null)
        {
            var baseShape = ChessBoard.instance.board[0, 0].baseShape;
            var height = ChessBoard.instance.board[0, 0].height;
            var hasCircleOnTop = ChessBoard.instance.board[0, 0].IsConcave;
            var color = ChessBoard.instance.board[0, 0].chessType;
            for (int j = 1; j < ChessBoard.instance.board.GetLength(0); j++)
            {
                if (ChessBoard.instance.board[j, j] == null || ChessBoard.instance.board[j, j].baseShape != baseShape)
                    break;
                if (j == ChessBoard.instance.board.GetLength(1) - 1)
                {
                    switch (color)
                    {
                        case ChessType.Black:
                            return 1000;
                        case ChessType.White:
                            return -1000;
                    }
                }
            }

            for (int j = 1; j < ChessBoard.instance.board.GetLength(0); j++)
            {
                if (ChessBoard.instance.board[j, j] == null || ChessBoard.instance.board[j, j].height != height)
                    break;
                if (j == ChessBoard.instance.board.GetLength(1) - 1)
                {
                    switch (color)
                    {
                        case ChessType.Black:
                            return 1000;
                        case ChessType.White:
                            return -1000;
                    }
                }
            }

            for (int j = 0; j < ChessBoard.instance.board.GetLength(0); j++)
            {
                if (ChessBoard.instance.board[j, j] == null || ChessBoard.instance.board[j, j].IsConcave != hasCircleOnTop)
                    break;
                if (j == ChessBoard.instance.board.GetLength(1) - 1)
                {
                    switch (color)
                    {
                        case ChessType.Black:
                            return 1000;
                        case ChessType.White:
                            return -1000;
                    }
                }
            }
        }

        if (ChessBoard.instance.board[0, ChessBoard.instance.board.GetLength(0) - 1] != null)
        {
            var baseShape = ChessBoard.instance.board[0, ChessBoard.instance.board.GetLength(0) - 1].baseShape;
            var height = ChessBoard.instance.board[0, ChessBoard.instance.board.GetLength(0) - 1].height;
            var isConcave = ChessBoard.instance.board[0, ChessBoard.instance.board.GetLength(0) - 1].IsConcave;
            var color = ChessBoard.instance.board[0, ChessBoard.instance.board.GetLength(0) - 1].chessType;
            for (int j = 0; j < ChessBoard.instance.board.GetLength(0); j++)
            {
                if (ChessBoard.instance.board[j, ChessBoard.instance.board.GetLength(0) - 1 - j] == null || ChessBoard.instance.board[j, ChessBoard.instance.board.GetLength(0) - 1 - j].baseShape != baseShape)
                    break;
                if (j == ChessBoard.instance.board.GetLength(1) - 1)
                {
                    switch (color)
                    {
                        case ChessType.Black:
                            return 1000;
                        case ChessType.White:
                            return -1000;
                    }
                }
            }

            for (int j = 0; j < ChessBoard.instance.board.GetLength(0); j++)
            {
                if (ChessBoard.instance.board[j, ChessBoard.instance.board.GetLength(0) - 1 - j] == null || ChessBoard.instance.board[j, ChessBoard.instance.board.GetLength(0) - 1 - j].height != height)
                    break;
                if (j == ChessBoard.instance.board.GetLength(1) - 1)
                {
                    switch (color)
                    {
                        case ChessType.Black:
                            return 1000;
                        case ChessType.White:
                            return -1000;
                    }
                }
            }

            for (int j = 0; j < ChessBoard.instance.board.GetLength(0); j++)
            {
                if (ChessBoard.instance.board[j, ChessBoard.instance.board.GetLength(0) - 1 - j] == null ||
                    ChessBoard.instance.board[j, ChessBoard.instance.board.GetLength(0) - 1 - j].IsConcave != isConcave)
                    break;
                if (j == ChessBoard.instance.board.GetLength(1) - 1)
                {
                    switch (color)
                    {
                        case ChessType.Black:
                            return 1000;
                        case ChessType.White:
                            return -1000;
                    }
                }
            }
        }

        return 0;
    }

    private int minimaxTime;

    public int MiniMax(int depth, bool isMax)
    {
        if (ChessBoard.instance.IsFull())
        {
            print("chess board is full");
            return 0;
        }

        minimaxTime++;
        // Max depth is set in main menu
        if (depth >= searchDepth)
        {
            return 0;
        }
        print($"Depth is {depth}; search depth is {searchDepth}");

        int score = Evaluate();

        if (score == 1000 || score == -1000)
        {
            print("score is 1000 or -1000");
            return score;
        }
        print("2");
        if (ChessBoard.instance.IsFull())
            return 0;
        print("3");
        if (isMax)
        {
            int best = -1000;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (ChessBoard.instance.board[i, j] == null)
                    {
                        print($"Test space ({i},{j})");
                        var randomBlackChess =
                            ChessBoard.instance.availableBlackChess[
                                Random.Range(0, ChessBoard.instance.availableBlackChess.Count)];
                        ChessBoard.instance.board[i, j] = randomBlackChess;
                        ChessBoard.instance.availableBlackChess.Remove(randomBlackChess);
                        best = Mathf.Max(best, MiniMax(depth + 1, false));
                        ChessBoard.instance.availableBlackChess.Add(ChessBoard.instance.board[i, j]);
                        ChessBoard.instance.board[i, j] = null;
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
                    if (ChessBoard.instance.board[i, j] == null)
                    {
                        var randomWhiteChess =
                            ChessBoard.instance.availableWhiteChess[
                                Random.Range(0, ChessBoard.instance.availableWhiteChess.Count)];
                        ChessBoard.instance.board[i, j] = randomWhiteChess;
                        ChessBoard.instance.availableWhiteChess.Remove(ChessBoard.instance.board[i, j]);
                        best = Mathf.Min(best, MiniMax( depth + 1, true));
                        ChessBoard.instance.availableWhiteChess.Add(ChessBoard.instance.board[i, j]);
                        ChessBoard.instance.board[i, j] = null;
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

    public Move FindBestMove()
    {
        
        print("AI is thinking...");
        int bestVal = Int32.MinValue;
        Move bestMove = new Move {row = -1, col = -1};

        // Traverse all cells, evaluate minimax function  
        // for all empty cells. And return the cell  
        // with optimal value.
        int passTime = 0;
        
        var placesToCheck = GetRidOfUnqualifiedPlaces();
        foreach (var cell in placesToCheck)
        {
            int i = cell.Item1;
            int j = cell.Item2;
            // Check if cell is empty 
                passTime++;
                ChessBoard.instance.board[i, j] = currentPickChess;
                ChessBoard.instance.availableBlackChess.Remove(currentPickChess);
                int moveVal = MiniMax( 0, false);
                ChessBoard.instance.availableBlackChess.Add(currentPickChess);

                if (moveVal > bestVal)
                {
                    bestMove.row = i;
                    bestMove.col = j;
                    bestVal = moveVal;
                }

                ChessBoard.instance.board[i, j] = null;
        }
                
        
        print("how many place to choose from: "+passTime);



        print("AI finished thinking...");
        return bestMove;
    }
}