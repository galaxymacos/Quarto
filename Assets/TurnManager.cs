using System;
using TMPro;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;
    public TextMeshProUGUI chessPickIndex;
    public TextMeshProUGUI dropCol;
    public TextMeshProUGUI dropRow;

    public GameObject[] availableChess;

    public GameObject currentPickedChess;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    
    
    // Select Chess
    public void OnConfirm()
    {
        int index = Convert.ToInt32(chessPickIndex.text);
        if (availableChess[index] != null)
        {
            currentPickedChess = availableChess[index];
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
            ChessBoard.instance.board[Convert.ToInt32(dropRow), Convert.ToInt32(dropCol)] = currentPickedChess.GetComponent<ChessInfo>();
        }
    }
    
    
}