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
            Debug.Log("Pick chess for enemy, now it is enemy's turn");
            TurnManager.instance.currentState = TurnManager.State.AIMove;
        }
        else
        {
            Debug.LogWarning("It is not the player's turn to pick for enemy");
        }
    }
}
