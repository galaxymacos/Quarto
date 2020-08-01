using System;
using TMPro;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;
    public TextMeshProUGUI chessPickIndex;
    public TextMeshProUGUI dropCol;
    public TextMeshProUGUI dropRow;

    public State currentState;
    private State prevState;
    public event Action<State> onStateChange;

    public AIChessPlayer aiPlayer;
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
        
    }

    private void Update()
    {
        if (currentState != prevState)
        {
            onStateChange?.Invoke(currentState);
        }
        prevState = currentState;
    }

    public void UpdateState(State newState)
    {
        currentState = newState;
    }

    private void Start()
    {
        aiPlayer.PickChessForPlayer();
    }


    // Player Select Chess
    public void OnConfirm()
    {
        int index = Convert.ToInt32(chessPickIndex.text);
        if (ChessBoard.instance.availableBlackChess[index] != null)
        {
            ChessPlayer.instance.currentPickChess = ChessBoard.instance.availableBlackChess[index];
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
            ChessBoard.instance.board[Convert.ToInt32(dropRow), Convert.ToInt32(dropCol)] = ChessPlayer.instance.currentPickChess.GetComponent<ChessInfo>();
        }
    }


    public enum State
    {
        AIPickForPlayer,
        PlayerMove,
        PlayerPickForAI,
        AIMove,
        PlayerWin,
        AIWin
    }
}