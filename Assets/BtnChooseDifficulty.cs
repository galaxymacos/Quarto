using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BtnChooseDifficulty : MonoBehaviour
{
    public int depth;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnClick_SetAIDifficulty()
    {
        AIChessPlayer.instance.searchDepth = depth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
