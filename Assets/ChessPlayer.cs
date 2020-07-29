using UnityEngine;

public class ChessPlayer: MonoBehaviour
{
    public static ChessPlayer instance;

    public ChessInfo currentPickChess;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // public void TryPickingChess(ChessInfo chessInfo)
    // {
    //     if (currentPickedChess != null)
    //     {
    //         Debug.LogWarning("Player already picked a chess");
    //     }
    //     else
    //     {
    //         Debug.Log("Chess is picked up");
    //         currentPickedChess = chessInfo;
    //     }
    // }

    public void TryDropCurrentChessToGrid(ChessBoardCube chessBoardCube)
    {
        if (currentPickChess == null)
        {
            Debug.LogWarning("The player hasn't picked up any chess");
        }
        else
        {
            Debug.Log($"Drop current chess to grid ({chessBoardCube.rowNum}, {chessBoardCube.colNum}");
            if (ChessBoard.instance.board[chessBoardCube.colNum, chessBoardCube.rowNum] != null)
            {
                Debug.LogWarning($"There is already chess at the position ({chessBoardCube.colNum}, {chessBoardCube.rowNum}");
            }
            else
            {
                ChessBoard.instance.board[chessBoardCube.colNum, chessBoardCube.rowNum] = currentPickChess;
                ChessBoard.instance.RefreshBoard();
                currentPickChess = null;
                TurnManager.instance.currentState = TurnManager.State.PlayerPickForAI;
            }
        }
    }
}