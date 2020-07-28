using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIChessPlayer: MonoBehaviour
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
        var chess = ChessBoard.instance.availableWhiteChess;
        ChessPlayer.instance.currentPickedChess = chess[Random.Range(0, chess.Length - 1)];
        TurnManager.instance.currentState = TurnManager.State.PlayerMove;
        print("Player Move");
    }

    public IEnumerator PlaceChess()
    {
        yield return new WaitForSeconds(1f);
        var board = ChessBoard.instance.board;
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j] == null)
                {
                    board[i, j] = currentPickChess;
                    ChessBoard.instance.RefreshBoard();
                    currentPickChess = null;
                    TurnManager.instance.currentState = TurnManager.State.AIPickForPlayer;
                }
            }
        }
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
}

