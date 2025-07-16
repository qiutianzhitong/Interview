using System.Collections;
using UnityEngine;

public class DamageFeedback : MonoBehaviour
{
    [Header("反馈设置")]
    public Color damageColor = Color.red; // 受伤时闪烁的颜色
    public float flashDuration = 0.1f; // 闪烁持续时间

    private SpriteRenderer spriteRenderer; // 角色的 SpriteRenderer 组件

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // 获取角色的 SpriteRenderer 组件
        
    }

    // 触发伤害反馈效果
    public void TriggerDamageFeedback()
    {
        StartCoroutine(DamageFlash()); // 启动闪烁效果
    }

    // 闪烁效果
    private IEnumerator DamageFlash()
    {
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color; // 保存原始颜色
            spriteRenderer.color = damageColor; // 设置为伤害时的颜色
            yield return new WaitForSeconds(flashDuration); // 等待闪烁持续时间
            spriteRenderer.color = originalColor; // 恢复原始颜色
        }
    }
    
}