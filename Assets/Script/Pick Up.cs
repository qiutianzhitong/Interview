using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public AudioClip SFX;
   //public GameObject VFX;

    // 当与其他物体发生碰撞时调用
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PropManager manager = collision.GetComponent<PropManager>();
        if (manager)
        {
            bool pickedUp = manager.PickUpItem(gameObject); 
            if (pickedUp)
                RemoveItem();
        }
    }

    // 移除物品，播放音效和特效，销毁当前对象
    private void RemoveItem()
    {
        AudioSource.PlayClipAtPoint(SFX, transform.position); 
        //Instantiate(VFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

   
}
