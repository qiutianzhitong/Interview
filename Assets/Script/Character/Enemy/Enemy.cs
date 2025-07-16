using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    public float damage;
    public float chaseRange; // 追击范围（敌人在玩家多远范围内开始追击）
    public Transform playerTransform; // 玩家的 Transform 组件
    private bool isDead = false;

    private Animator anim; // 敌人的 Animator 组件
    private SpriteRenderer sr; // 敌人的 SpriteRenderer 组件
    private Rigidbody2D rb; // 敌人的 Rigidbody2D 组件
    private bool isKnockback = false; // 标记敌人是否处于击退状态
    public NavMeshAgent navMeshAgent;

    // 新增静态变量，用于记录死亡的敌人数量
    public static int deathCount = 0;

    private void Start()
    {
        // 获取玩家的 Transform 组件
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // 获取组件
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();    
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    private void Update()
    {
        // 检查玩家是否在追击范围内
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer <= chaseRange&&isDead == false)
        {
            // 朝玩家移动
            ChasePlayer();
        }
        else
        {
            // 停止移动并更新动画
            StopMoving();
        }

        // 更新动画参数
        UpdateAnimationParameters();
    }

    void ChasePlayer()
    {
        if (isKnockback)
        {
            // 如果敌人正在击退，则不进行移动
            return;
        }
        // 重置 NavMeshAgent 的停止状态
        navMeshAgent.isStopped = false;
        // 计算朝玩家的移动方向
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        // 移动敌人
        navMeshAgent.SetDestination(playerTransform.position);
        // 更新朝向
        if (direction.x < 0)
        {
            sr.flipX = true;
        }
        else if (direction.x > 0)
        {
            sr.flipX = false;
        }
    }

    void StopMoving()
    {
        // 停止移动
        navMeshAgent.isStopped = true;
    }

    void UpdateAnimationParameters()
    {
        // 获取当前移动速度
        Vector2 velocity = new Vector2(navMeshAgent.velocity.x, navMeshAgent.velocity.y);
        if (velocity.magnitude > 0.1f) // 确保有一定的移动速度
        {
            // 更新 isMove 参数
            anim.SetBool("isMove", true);

            // 判断方向并更新 Direction 参数
            if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
            {
                // 水平方向移动
                anim.SetInteger("Direction", 2);
            }
            else
            {
                // 垂直方向移动
                if (velocity.y < 0)
                {
                    anim.SetInteger("Direction", 0); // 向下
                }
                else if (velocity.y > 0)
                {
                    anim.SetInteger("Direction", 1); // 向上
                }
            }
        }
        else
        {
            // 停止移动时
            anim.SetBool("isMove", false);
            anim.SetInteger("Direction", 0); 
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Character>().TakeDamage(damage);
          
        }
    }

    public override void Die()
    {
        // 增加死亡数量
        deathCount++;

        // 设置 isDead 参数为 true
        anim.SetBool("isDead", true);
        isDead = true;

        // 获取 ChatController 实例并传递死亡数量
        ChatController chatController = FindObjectOfType<ChatController>();
        if (chatController != null)
        {
            chatController.ReceiveEnemyDeathCount(deathCount);
        }

        // 启动协程等待动画播放完毕
        StartCoroutine(WaitForDeathAnimation());
    }

    // 协程：等待死亡动画播放完毕
    private IEnumerator WaitForDeathAnimation()
    {
        // 等待动画播放完毕
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        // 销毁角色
       base.Die();
    }

    // 新增击退方法
    public void Knockback(Vector2 direction, float force)
    {
        isKnockback = true;
        navMeshAgent.isStopped = true; // 停止 NavMeshAgent 的移动
        rb.AddForce(direction * force, ForceMode2D.Impulse); // 使用 Impulse 模式
        StartCoroutine(ResetKnockback()); // 启动协程
    }

    private IEnumerator ResetKnockback()
    {
        yield return new WaitForSeconds(0.5f); // 等待击退效果结束
        isKnockback = false; // 恢复移动逻辑
        navMeshAgent.isStopped = false; // 恢复 NavMeshAgent 的移动
    }
   
}