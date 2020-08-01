using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChess : MonoBehaviour
{
    
    
    private void OnMouseDown()
    {
        if (TurnManager.instance.currentState == TurnManager.State.PlayerPickForAI)
        {
            AIChessPlayer.instance.currentPickChess = GetComponent<ChessInfo>();
            ChessBoard.instance.availableBlackChess.Remove(GetComponent<ChessInfo>());
            TurnManager.instance.currentState = TurnManager.State.AIMove;
        }
        else
        {
            Debug.LogWarning("It is not the player's turn to pick for enemy");
        }
    }
}
