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

        switch (ChessBoard.instance.CheckVictoryCondition(ChessBoard.instance.board))
        {
            case ChessType.Black:
                print("AI wins");
                break;
            case ChessType.White:
                print("Player wins");
                break;
        }

        var value = QuickAlgorithm.CalThreatValue();
        Move bestMove = new Move();
        if (value != (-1, -1))
        {
            print("use new algorithm");
            bestMove.row = value.Item1;
            bestMove.col = value.Item2;
        }
        else
        {
            print("use old algorithm");
            bestMove = FindBestMove();
        }

        // TODO add it back
        ChessBoard.instance.board[bestMove.row, bestMove.col] = currentPickChess;
        ChessBoard.instance.availableBlackChess.Remove(currentPickChess);
        ChessBoard.instance.RefreshBoard();
        if (ChessBoard.instance.CheckVictoryCondition(ChessBoard.instance.board) == ChessType.Black)
        {
            print("AI wins, game over");
            TurnManager.instance.currentState = TurnManager.State.AIWin;
            return;
        }

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


    private List<Tuple<int, int>> GetRidOfUnqualifiedPlaces()
    {
        List<Tuple<int, int>> result = new List<Tuple<int, int>>();

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (ChessBoard.instance.board[i, j] == null)
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
                        if (ChessBoard.instance.board[i - 1, j + 1] != null)
                        {
                            result.Add(new Tuple<int, int>(i, j));
                            continue;
                        }
                    }

                    // top right
                    if (i <= 2 && j >= 1)
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
                        if (ChessBoard.instance.board[i + 1, j + 1] != null)
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
        }

        return result;
    }

    public int Evaluate()
    {
        var winner = ChessBoard.instance.CheckVictoryCondition(ChessBoard.instance.board);
        switch (winner)
        {
            case ChessType.Black:
                return 1000;
            case ChessType.White:
                return -1000;
            case ChessType.Null:
                return 0;
            default:
                return 0;
        }

        // for (int i = 0; i < 4; i++)
        // {
        //     var baseShape = ChessBoard.instance.board[i, 0].baseShape;
        //     var height = ChessBoard.instance.board[i, 0].height;
        //     var isConcave = ChessBoard.instance.board[i, 0].IsConcave;
        //     var chessType = ChessBoard.instance.board[i, 0].chessType;
        //     
        //     
        //     for (int j = 0; j < 4; j++)
        //     {
        //         if (ChessBoard.instance.board[i, j] == null || ChessBoard.instance.board[i, j].baseShape != baseShape || ChessBoard.instance.board[i,j].chessType != chessType)
        //         {
        //             break;
        //         }
        //
        //         if (j == ChessBoard.instance.board.GetLength(1) - 1)
        //         {
        //             switch (chessType)
        //             {
        //                 case ChessType.Black:
        //                     return 1000;
        //                 case ChessType.White:
        //                     return -1000;
        //             }
        //         }
        //     }
        //
        //     for (int j = 0; j < ChessBoard.instance.board.GetLength(1); j++)
        //     {
        //         if (ChessBoard.instance.board[i, j] == null || ChessBoard.instance.board[i, j].height != height || ChessBoard.instance.board[i,j].chessType != chessType)
        //             break;
        //         if (j == ChessBoard.instance.board.GetLength(1) - 1)
        //         {
        //             switch (chessType)
        //             {
        //                 case ChessType.Black:
        //                     return 1000;
        //                 case ChessType.White:
        //                     return -1000;
        //             }
        //         }
        //     }
        //
        //     for (int j = 0; j < ChessBoard.instance.board.GetLength(1); j++)
        //     {
        //         if (ChessBoard.instance.board[i, j] == null || ChessBoard.instance.board[i, j].IsConcave != isConcave || ChessBoard.instance.board[i,j].chessType != chessType)
        //             break;
        //         if (j == ChessBoard.instance.board.GetLength(1) - 1)
        //         {
        //             switch (chessType)
        //             {
        //                 case ChessType.Black:
        //                     return 1000;
        //                 case ChessType.White:
        //                     return -1000;
        //             }
        //         }
        //     }
        // }

        // for (int i = 0; i < ChessBoard.instance.board.GetLength(0); i++)
        // {
        //     if (ChessBoard.instance.board[0, i] == null) continue;
        //     var baseShape = ChessBoard.instance.board[0, i].baseShape;
        //     var height = ChessBoard.instance.board[0, i].height;
        //     var isConcave = ChessBoard.instance.board[0, i].IsConcave;
        //     var chessType = ChessBoard.instance.board[0, i].chessType;
        //     for (int j = 0; j < ChessBoard.instance.board.GetLength(1); j++)
        //     {
        //         if (ChessBoard.instance.board[j, i] == null || ChessBoard.instance.board[j, i].baseShape != baseShape || ChessBoard.instance.board[j,i].chessType != chessType)
        //             break;
        //         if (j == ChessBoard.instance.board.GetLength(1) - 1)
        //         {
        //             switch (chessType)
        //             {
        //                 case ChessType.Black:
        //                     return 1000;
        //                 case ChessType.White:
        //                     return -1000;
        //             }
        //         }
        //     }
        //
        //     for (int j = 0; j < ChessBoard.instance.board.GetLength(1); j++)
        //     {
        //         if (ChessBoard.instance.board[j, i] == null || ChessBoard.instance.board[j, i].height != height || ChessBoard.instance.board[j,i].chessType != chessType)
        //             break;
        //         if (j == ChessBoard.instance.board.GetLength(1) - 1)
        //         {
        //             switch (chessType)
        //             {
        //                 case ChessType.Black:
        //                     return 1000;
        //                 case ChessType.White:
        //                     return -1000;
        //             }
        //         }
        //     }
        //
        //     for (int j = 0; j < ChessBoard.instance.board.GetLength(1); j++)
        //     {
        //         if (ChessBoard.instance.board[j, i] == null ||
        //             ChessBoard.instance.board[j, i].IsConcave != isConcave || ChessBoard.instance.board[j, i].chessType != chessType)
        //             break;
        //         if (j == ChessBoard.instance.board.GetLength(1) - 1)
        //         {
        //             switch (chessType)
        //             {
        //                 case ChessType.Black:
        //                     return 1000;
        //                 case ChessType.White:
        //                     return -1000;
        //             }
        //         }
        //     }
        // }

        // if (ChessBoard.instance.board[0, 0] != null)
        // {
        //     var baseShape = ChessBoard.instance.board[0, 0].baseShape;
        //     var height = ChessBoard.instance.board[0, 0].height;
        //     var hasCircleOnTop = ChessBoard.instance.board[0, 0].IsConcave;
        //     var chessType = ChessBoard.instance.board[0, 0].chessType;
        //     for (int j = 1; j < ChessBoard.instance.board.GetLength(0); j++)
        //     {
        //         if (ChessBoard.instance.board[j, j] == null || ChessBoard.instance.board[j, j].baseShape != baseShape || ChessBoard.instance.board[j, j].chessType != chessType)
        //             break;
        //         if (j == ChessBoard.instance.board.GetLength(1) - 1)
        //         {
        //             switch (chessType)
        //             {
        //                 case ChessType.Black:
        //                     return 1000;
        //                 case ChessType.White:
        //                     return -1000;
        //             }
        //         }
        //     }
        //
        //     for (int j = 1; j < ChessBoard.instance.board.GetLength(0); j++)
        //     {
        //         if (ChessBoard.instance.board[j, j] == null || ChessBoard.instance.board[j, j].height != height || ChessBoard.instance.board[j, j].chessType != chessType)
        //             break;
        //         if (j == ChessBoard.instance.board.GetLength(1) - 1)
        //         {
        //             switch (chessType)
        //             {
        //                 case ChessType.Black:
        //                     return 1000;
        //                 case ChessType.White:
        //                     return -1000;
        //             }
        //         }
        //     }
        //
        //     for (int j = 0; j < ChessBoard.instance.board.GetLength(0); j++)
        //     {
        //         if (ChessBoard.instance.board[j, j] == null ||
        //             ChessBoard.instance.board[j, j].IsConcave != hasCircleOnTop || ChessBoard.instance.board[j, j].chessType != chessType)
        //             break;
        //         if (j == ChessBoard.instance.board.GetLength(1) - 1)
        //         {
        //             switch (chessType)
        //             {
        //                 case ChessType.Black:
        //                     return 1000;
        //                 case ChessType.White:
        //                     return -1000;
        //             }
        //         }
        //     }
        // }

        // if (ChessBoard.instance.board[0, ChessBoard.instance.board.GetLength(0) - 1] != null)
        // {
        //     var baseShape = ChessBoard.instance.board[0, ChessBoard.instance.board.GetLength(0) - 1].baseShape;
        //     var height = ChessBoard.instance.board[0, ChessBoard.instance.board.GetLength(0) - 1].height;
        //     var isConcave = ChessBoard.instance.board[0, ChessBoard.instance.board.GetLength(0) - 1].IsConcave;
        //     var chessType = ChessBoard.instance.board[0, ChessBoard.instance.board.GetLength(0) - 1].chessType;
        //     for (int j = 0; j < ChessBoard.instance.board.GetLength(0); j++)
        //     {
        //         if (ChessBoard.instance.board[j, ChessBoard.instance.board.GetLength(0) - 1 - j] == null ||
        //             ChessBoard.instance.board[j, ChessBoard.instance.board.GetLength(0) - 1 - j].baseShape != baseShape ||
        //             ChessBoard.instance.board[j, ChessBoard.instance.board.GetLength(0) - 1 - j].chessType != chessType)
        //             break;
        //         if (j == ChessBoard.instance.board.GetLength(1) - 1)
        //         {
        //             switch (chessType)
        //             {
        //                 case ChessType.Black:
        //                     return 1000;
        //                 case ChessType.White:
        //                     return -1000;
        //             }
        //         }
        //     }
        //
        //     for (int j = 0; j < ChessBoard.instance.board.GetLength(0); j++)
        //     {
        //         if (ChessBoard.instance.board[j, ChessBoard.instance.board.GetLength(0) - 1 - j] == null ||
        //             ChessBoard.instance.board[j, ChessBoard.instance.board.GetLength(0) - 1 - j].height != height ||
        //             ChessBoard.instance.board[j, ChessBoard.instance.board.GetLength(0) - 1 - j].chessType != chessType)
        //             break;
        //         if (j == ChessBoard.instance.board.GetLength(1) - 1)
        //         {
        //             switch (chessType)
        //             {
        //                 case ChessType.Black:
        //                     return 1000;
        //                 case ChessType.White:
        //                     return -1000;
        //             }
        //         }
        //     }
        //
        //     for (int j = 0; j < ChessBoard.instance.board.GetLength(0); j++)
        //     {
        //         if (ChessBoard.instance.board[j, ChessBoard.instance.board.GetLength(0) - 1 - j] == null ||
        //             ChessBoard.instance.board[j, ChessBoard.instance.board.GetLength(0) - 1 - j].IsConcave != isConcave ||
        //             ChessBoard.instance.board[j, ChessBoard.instance.board.GetLength(0) - 1 - j].chessType != chessType)
        //             break;
        //         if (j == ChessBoard.instance.board.GetLength(1) - 1)
        //         {
        //             switch (chessType)
        //             {
        //                 case ChessType.Black:
        //                     return 1000;
        //                 case ChessType.White:
        //                     return -1000;
        //             }
        //         }
        //     }
        // }

        // return 0;
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
                        best = Mathf.Min(best, MiniMax(depth + 1, true));
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
            int moveVal = MiniMax(0, false);
            ChessBoard.instance.availableBlackChess.Add(currentPickChess);

            if (moveVal > bestVal)
            {
                bestMove.row = i;
                bestMove.col = j;
                bestVal = moveVal;
            }

            ChessBoard.instance.board[i, j] = null;
        }


        print("how many place to choose from: " + passTime);


        print("AI finished thinking...");
        return bestMove;
    }
}

public static class QuickAlgorithm
{
    public enum LineType
    {
        Row,
        Col,
        Diagonal,
        AntiDiagonal
    }

    public static Difficulty gameDifficulty;

    public static List<Tuple<int, int>> diagonal = new List<Tuple<int, int>>
    {
        new Tuple<int, int>(0, 0),
        new Tuple<int, int>(1, 1),
        new Tuple<int, int>(2, 2),
        new Tuple<int, int>(3, 3),
    };

    public static List<Tuple<int, int>> antiDiagonal = new List<Tuple<int, int>>
    {
        new Tuple<int, int>(0, 3),
        new Tuple<int, int>(1, 2),
        new Tuple<int, int>(2, 1),
        new Tuple<int, int>(3, 0),
    };

    public static List<Tuple<int, int>> row1 = new List<Tuple<int, int>>
    {
        new Tuple<int, int>(0, 0),
        new Tuple<int, int>(1, 0),
        new Tuple<int, int>(2, 0),
        new Tuple<int, int>(3, 0),
    };

    public static List<Tuple<int, int>> row2 = new List<Tuple<int, int>>
    {
        new Tuple<int, int>(0, 1),
        new Tuple<int, int>(1, 1),
        new Tuple<int, int>(2, 1),
        new Tuple<int, int>(3, 1),
    };

    public static List<Tuple<int, int>> row3 = new List<Tuple<int, int>>
    {
        new Tuple<int, int>(0, 2),
        new Tuple<int, int>(1, 2),
        new Tuple<int, int>(2, 2),
        new Tuple<int, int>(3, 2),
    };

    public static List<Tuple<int, int>> row4 = new List<Tuple<int, int>>
    {
        new Tuple<int, int>(0, 3),
        new Tuple<int, int>(1, 3),
        new Tuple<int, int>(2, 3),
        new Tuple<int, int>(3, 3),
    };

    public static List<Tuple<int, int>> col1 = new List<Tuple<int, int>>
    {
        new Tuple<int, int>(0, 0),
        new Tuple<int, int>(0, 1),
        new Tuple<int, int>(0, 2),
        new Tuple<int, int>(0, 3),
    };

    public static List<Tuple<int, int>> col2 = new List<Tuple<int, int>>
    {
        new Tuple<int, int>(1, 0),
        new Tuple<int, int>(1, 1),
        new Tuple<int, int>(1, 2),
        new Tuple<int, int>(1, 3),
    };

    public static List<Tuple<int, int>> col3 = new List<Tuple<int, int>>
    {
        new Tuple<int, int>(2, 0),
        new Tuple<int, int>(2, 1),
        new Tuple<int, int>(2, 2),
        new Tuple<int, int>(2, 3),
    };

    public static List<Tuple<int, int>> col4 = new List<Tuple<int, int>>
    {
        new Tuple<int, int>(3, 0),
        new Tuple<int, int>(3, 1),
        new Tuple<int, int>(3, 2),
        new Tuple<int, int>(3, 3),
    };

    private static Dictionary<(int, int), int> posAndThreatValue;

    private static List<List<Tuple<int, int>>> lines;

    public enum Difficulty
    {
        Easy,
        Normal,
        Hard,
        Impossible
    }

    public static (int, int) CalThreatValue()
    {
        posAndThreatValue = new Dictionary<(int, int), int>();
        lines = new List<List<Tuple<int, int>>>();
        lines.Add(row1);
        lines.Add(row2);
        lines.Add(row3);
        lines.Add(row4);
        lines.Add(col1);
        lines.Add(col2);
        lines.Add(col3);
        lines.Add(col4);
        lines.Add(diagonal);
        lines.Add(antiDiagonal);

        var board = ChessBoard.instance.board;

        foreach (List<Tuple<int, int>> line in lines)
        {
            int interestValue = 0;
            int blackChessNum = 0;
            int whiteChessNum = 0;
            int emptySpaceNum = 0;
            
            int blackChessMatchingConcave = 0;
            int blackChessMatchingBaseShape = 0;
            int blackChessMatchingHeight = 0;

            bool firstChessInitialized = false;
            int height = -1;
            bool isConcave = false;
            ChessBaseShape baseShape = ChessBaseShape.Cube;
            
            var emptyPositions = (-1, -1);
            foreach (Tuple<int, int> posInBoard in line)
            {
                if (board[posInBoard.Item1, posInBoard.Item2] == null)
                {
                    emptySpaceNum++;
                    emptyPositions = (posInBoard.Item1, posInBoard.Item2);
                }
                else if (board[posInBoard.Item1, posInBoard.Item2].chessType == ChessType.Black)
                {
                    blackChessNum++;

                    if (!firstChessInitialized)
                    {
                        firstChessInitialized = true;
                        height = board[posInBoard.Item1, posInBoard.Item2].height;
                        isConcave = board[posInBoard.Item1, posInBoard.Item2].IsConcave;
                        baseShape = board[posInBoard.Item1, posInBoard.Item2].baseShape;
                    }
                    else // first black chess info got
                    {
                        if (board[posInBoard.Item1, posInBoard.Item2].height ==
                            height)
                        {
                            blackChessMatchingHeight++;
                        }
                        else
                        {
                            blackChessMatchingHeight = 0;
                        }
                        if (board[posInBoard.Item1, posInBoard.Item2].baseShape ==
                            baseShape)
                        {
                            blackChessMatchingBaseShape++;
                        }
                        else
                        {
                            blackChessMatchingBaseShape = 0;
                        }
                    
                        if (board[posInBoard.Item1, posInBoard.Item2].IsConcave ==
                            isConcave)
                        {
                            blackChessMatchingConcave++;
                        }
                        else
                        {
                            blackChessMatchingConcave = 0;
                        }
                    }

                    
                    
                    
                }
                else if (board[posInBoard.Item1, posInBoard.Item2].chessType == ChessType.White)
                {
                    whiteChessNum++;
                    blackChessMatchingConcave = 0;
                    blackChessMatchingBaseShape = 0;
                    blackChessMatchingHeight = 0;
                }
                
            }

            switch (gameDifficulty)
            {
                case Difficulty.Easy:
                    if (emptySpaceNum >= 1)
                    {
                        if (whiteChessNum == 3)
                        {
                            posAndThreatValue.Add(emptyPositions, 100);
                        }
                    }

                    // Random algorithm
                    break;
                case Difficulty.Normal:
                    if (emptySpaceNum >= 1)
                    {
                        if (whiteChessNum == 3)
                        {
                            posAndThreatValue.Add(emptyPositions, 100);
                        }

                        if (posAndThreatValue.ContainsKey(emptyPositions))
                        {
                            posAndThreatValue[emptyPositions] =
                                posAndThreatValue[emptyPositions] + blackChessNum * 34;
                        }
                        else
                        {
                            posAndThreatValue.Add(emptyPositions, blackChessNum * 34);
                        }
                    }


                    break;
                case Difficulty.Hard:
                    if (emptySpaceNum >= 1)
                    {
                        if (whiteChessNum == 3)
                        {
                            if (posAndThreatValue.ContainsKey(emptyPositions))
                            {
                                posAndThreatValue[emptyPositions] = posAndThreatValue[emptyPositions] + 1000;
                            }
                            else
                            {
                                posAndThreatValue.Add(emptyPositions, 1000);
                            }
                        }

                        if (posAndThreatValue.ContainsKey(emptyPositions))
                        {
                            posAndThreatValue[emptyPositions] =  posAndThreatValue[emptyPositions]+blackChessMatchingConcave * 34 +
                                                                blackChessMatchingHeight * 34 + blackChessMatchingBaseShape * 34;
                        }
                        else
                        {
                            posAndThreatValue.Add(emptyPositions, blackChessMatchingConcave * 34 +
                                                                                  blackChessMatchingHeight * 34 + blackChessMatchingBaseShape * 34);
                        }
                    }
                    break;
                case Difficulty.Impossible:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        int maxValue = Int32.MinValue;
        (int, int) targetPos = (-1, -1);
        foreach (KeyValuePair<(int, int), int> pair in posAndThreatValue)
        {
            if (pair.Value > maxValue)
            {
                maxValue = pair.Value;
                targetPos = pair.Key;
            }

            if (pair.Value == 100)
            {
                return pair.Key;
            }
        }

        if (targetPos != (-1, -1))
        {
            return targetPos;
        }
        else
        {
            return ChessBoard.instance.AnyEmptySpace();
        }
    }
}