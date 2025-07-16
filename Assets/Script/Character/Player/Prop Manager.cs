using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropManager : MonoBehaviour
{
    public static int coinCount;
    public static int gemCount;
    // Start is called before the first frame update
    public bool PickUpItem(GameObject obj){
        switch(obj.tag){
            case Constants.TAG_COIN:
            PickUpCoin();
            return true;
            case Constants.TAG_GEM:
            PickUpGem();
            return true;
            default:
            Debug.Log("不可拾取");
            Debug.Log("123");
            return false;
        }
    }
    private void OnGUI(){
        GUI.skin.label.fontSize=50;
        GUI.Label(new Rect(20,80,500,500),"Coin Num:"+coinCount);
        GUI.Label(new Rect(20,160,500,500),"Gem Num:"+gemCount);
    }
   private void PickUpCoin(){
    coinCount++;
   }
   private void PickUpGem(){
    gemCount++;
   }
}
