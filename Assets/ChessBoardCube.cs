using UnityEngine;

public class ChessBoardCube: MonoBehaviour
{
    public int rowNum;
    public int colNum; 
    private void OnMouseDown()
    {
        ChessPicker.instance.DropCurrentChessToGrid(this);
    }
    
}