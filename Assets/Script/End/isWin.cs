using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isWin : MonoBehaviour
{
     public Canvas winCanvas; // 胜利的 Canvas

    // Update is called once per frame
    void Update()
    {
        if (winCanvas != null&& PropManager.gemCount==1)
        {
            // 显示胜利的 Canvas
            winCanvas.gameObject.SetActive(true);
        }
        
    }
}
