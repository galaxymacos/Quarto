using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BtnChooseDifficulty : MonoBehaviour
{
    public int depth;
    public QuickAlgorithm.Difficulty difficulty;

    public void OnClick_SetAIDifficulty()
    {
        AIChessPlayer.instance.searchDepth = depth;
        QuickAlgorithm.gameDifficulty = difficulty;
    }

}
