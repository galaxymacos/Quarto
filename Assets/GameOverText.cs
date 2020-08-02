using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverText : MonoBehaviour
{
    public static GameOverText instance;
    private TextMeshProUGUI gameOverText;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameOverText = GetComponent<TextMeshProUGUI>();
    }

    public void ActivateText(ChessType winner)
    {
        
        switch (winner)
        {
            case ChessType.Black:
                gameOverText.text = "Congratulations, your opponent wins~";
                break;
            case ChessType.White:
                gameOverText.text = "Congratulations, you win";
                break;
            case ChessType.Null:
                gameOverText.text = "Great match! It is a draw";
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
