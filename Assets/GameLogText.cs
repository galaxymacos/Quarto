using System;
using TMPro;
using UnityEngine;

public class GameLogText : MonoBehaviour
{
    public TextMeshProUGUI logText;

    public TextMeshProUGUI whiteChessCountText;
    public TextMeshProUGUI blackChessCountText;

    // Start is called before the first frame update
    void Start()
    {
        TurnManager.instance.onStateChange += DisplayStateText;
    }

    private void OnDestroy()
    {
        TurnManager.instance.onStateChange -= DisplayStateText;
    }

    private void DisplayStateText(TurnManager.State currentState)
    {
        switch (currentState)
        {
            case TurnManager.State.AIPickForPlayer:
                logText.text = "AI is picking a chess for you";
                break;
            case TurnManager.State.PlayerMove:
                logText.text = "Please play your move";
                break;
            case TurnManager.State.PlayerPickForAI:
                logText.text = "Please pick a chess for your opponent";
                break;
            case TurnManager.State.AIMove:
                logText.text = "AI is playing a move";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(currentState), currentState, null);
        }
    }

    public void Display()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        whiteChessCountText.text = "White count: "+ChessBoard.instance.availableWhiteChess.Count;
        blackChessCountText.text = "Black count: "+ChessBoard.instance.availableBlackChess.Count;
    }
}
