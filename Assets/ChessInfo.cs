using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ChessInfo : MonoBehaviour
{
    public int height;

    public bool hasCircleOnTop;

    public ChessBaseShape baseShape;

    public ChessColor chessColor;

    public bool isOnBoard;

}

public enum ChessBaseShape
{
    Square, Cylinder
}

public enum ChessColor
{
    Black,
    White
}