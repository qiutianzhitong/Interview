using UnityEngine;

public class EnemyDropGem : MonoBehaviour
{
    public GameObject gemPrefab; // 宝石预制体
  
    
    private void OnDestroy()
    {
        if (gemPrefab != null)
        {
            // 在敌人当前位置生成宝石预制体
            Instantiate(gemPrefab, transform.position, Quaternion.identity);

        }
       
    }
}    