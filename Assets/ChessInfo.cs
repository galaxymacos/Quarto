using System;
using UnityEngine;

public class ChessInfo : MonoBehaviour
{
    public int height;

    public bool IsConcave;

    public ChessBaseShape baseShape;

    public ChessType chessType;
    
    public GameObject indicateLight;

   
}

public enum ChessBaseShape
{
    Cube, Cylinder
}



public enum ChessType
{
    Black,
    White,
    Null
}

