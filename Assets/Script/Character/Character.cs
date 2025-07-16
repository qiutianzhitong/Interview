using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("属性")]
    [SerializeField] protected float maxHealth; // 最大生命值
    [SerializeField] protected float currentHealth; // 当前生命值

    [Header("无敌")]
    public bool Invulnerable; // 是否无敌
    public float InvulnerableDuration; // 无敌时间

    private DamageFeedback damageFeedback; // 用于触发伤害反馈的组件

    protected virtual void OnEnable()
    {
        currentHealth = maxHealth; // 初始化当前生命值
        damageFeedback = GetComponent<DamageFeedback>(); // 获取 DamageFeedback 组件
    }

    // 角色受到伤害时的处理
    public virtual void TakeDamage(float damage)
    {
        if (Invulnerable)
            return; // 如果无敌，跳过伤害

        currentHealth -= damage; // 扣除生命值

        // 触发伤害反馈
        if (damageFeedback != null)
        {
            damageFeedback.TriggerDamageFeedback(); // 调用伤害反馈函数
        }

        // 启动无敌状态协程
        StartCoroutine(nameof(InvulnerableCoroutine));

        // 如果生命值小于等于零，角色死亡
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    // 角色死亡处理
    public virtual void Die()
    {
        currentHealth = 0f;
        Destroy(this.gameObject); // 销毁角色
    }

    // 无敌时间协程
    protected virtual IEnumerator InvulnerableCoroutine()
    {
        Invulnerable = true; // 设置为无敌
        yield return new WaitForSeconds(InvulnerableDuration); // 等待无敌时间
        Invulnerable = false; // 恢复为可受伤状态
    }
}
