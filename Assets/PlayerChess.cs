using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChess : MonoBehaviour
{

    private Light chessPointLight;

    private void Awake()
    {
        chessPointLight = GetComponentInChildren<Light>();
    }


    private void Update()
    {
        chessPointLight.enabled = ChessPlayer.instance.currentPickChess == GetComponent<ChessInfo>();
    }
}
