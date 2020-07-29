using System;
using UnityEngine;

public class ChessBoardCube: MonoBehaviour
{
    public int rowNum;
    public int colNum;

    
    private void OnMouseDown()
    {
        if (TurnManager.instance.currentState == TurnManager.State.PlayerMove)
        {
            ChessPlayer.instance.TryDropCurrentChessToGrid(this);
            TurnManager.instance.currentState = TurnManager.State.PlayerPickForAI;
        }
        else
        {
            print("It is not player's turn");
        }

    }
    
}