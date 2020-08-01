using UnityEngine;

public class ChessPlayer : MonoBehaviour
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

    public void TryDropCurrentChessToGrid(ChessBoardCube chessBoardCube)
    {
        if (currentPickChess == null)
        {
            Debug.LogWarning("The player hasn't picked up any chess");
            return;
        }

        if (ChessBoard.instance.board[chessBoardCube.colNum, chessBoardCube.rowNum] != null)
        {
            Debug.LogWarning(
                $"There is already chess at the position ({chessBoardCube.colNum}, {chessBoardCube.rowNum}");
            return;
        }

        ChessBoard.instance.board[chessBoardCube.colNum, chessBoardCube.rowNum] = currentPickChess;
        ChessBoard.instance.RefreshBoard();
        currentPickChess = null;
        TurnManager.instance.currentState = TurnManager.State.PlayerPickForAI;

        TurnManager.instance.currentState = TurnManager.State.PlayerPickForAI;
    }
}