using System.Linq;
using UnityEngine;

public class ChessPicker: MonoBehaviour
{
    public static ChessPicker instance;

    private ChessInfo currentPickedChess;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void TryPickingChess(ChessInfo chessInfo)
    {
        if (currentPickedChess != null)
        {
            Debug.LogWarning("Player already picked a chess");
        }
        else
        {
            Debug.Log("Chess is picked up");
            currentPickedChess = chessInfo;
        }
    }

    public void DropCurrentChessToGrid(ChessBoardCube chessBoardCube)
    {
        if (currentPickedChess == null)
        {
            Debug.LogWarning("The player hasn't picked up any chess");
        }
        else
        {
            Debug.Log($"Drop current chess to grid ({chessBoardCube.rowNum}, {chessBoardCube.colNum}");
            if (ChessBoard.instance.board[chessBoardCube.rowNum, chessBoardCube.colNum] != null)
            {
                Debug.LogWarning($"There is always chess at the position ({chessBoardCube.rowNum}, {chessBoardCube.colNum}");
            }
            else
            {
                ChessBoard.instance.board[chessBoardCube.colNum, chessBoardCube.rowNum] = currentPickedChess;
                ChessBoard.instance.OrganizeBoard();
                currentPickedChess = null;
            }
        }
    }
}

public class AIChessPlayer: MonoBehaviour
{
    public ChessInfo PickChessForPlayer()
    {
        var chess = FindObjectsOfType<ChessInfo>()
            .Where(info => info.isOnBoard == false && info.chessColor == ChessColor.White).ToList();
        return chess[Random.Range(0, chess.Count - 1)];
    }

    public void PlaceChess()
    {
        
    }
}