using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 引入UI命名空间

public class Player : Character
{

    public Image life1, life2, life3, life4, life5; // 将GameObject改为Image
    public Sprite blackHeartSprite; // 变黑的心的图片
    public Sprite redHeartSprite;
    
    private static Player instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
   
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        switch (currentHealth)
        {
            case 80:
                life5.sprite = blackHeartSprite; // 血量降到80，更换第五颗心的图片
                break;
            case 60:
                life4.sprite = blackHeartSprite; // 血量降到60，更换第四颗心的图片
                life5.sprite = blackHeartSprite; // 更换第五颗心的图片
                break;
            case 40:
                life3.sprite = blackHeartSprite; // 血量降到40，更换第三颗心的图片
                life4.sprite = blackHeartSprite; // 更换第四颗心的图片
                life5.sprite = blackHeartSprite; // 更换第五颗心的图片
                break;
            case 20:
                life2.sprite = blackHeartSprite; // 血量降到20，更换第二颗心的图片
                life3.sprite = blackHeartSprite; // 更换第三颗心的图片
                life4.sprite = blackHeartSprite; // 更换第四颗心的图片
                life5.sprite = blackHeartSprite; // 更换第五颗心的图片
                break;
            case 0:
                life1.sprite = blackHeartSprite; // 血量降到0，更换第一颗心的图片
                life2.sprite = blackHeartSprite; // 更换第二颗心的图片
                life3.sprite = blackHeartSprite; // 更换第三颗心的图片
                life4.sprite = blackHeartSprite; // 更换第四颗心的图片
                life5.sprite = blackHeartSprite; // 更换第五颗心的图片
                break;
        }
    }
    public void Restore(){
        currentHealth = 100;
        life1.sprite = redHeartSprite;
        life2.sprite = redHeartSprite;
        life3.sprite = redHeartSprite;
        life4.sprite = redHeartSprite;
        life5.sprite = redHeartSprite;
    }
}