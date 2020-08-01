using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BtnChooseDifficulty : MonoBehaviour
{
    public int depth;

    public void OnClick_SetAIDifficulty()
    {
        AIChessPlayer.instance.searchDepth = depth;
    }

}
