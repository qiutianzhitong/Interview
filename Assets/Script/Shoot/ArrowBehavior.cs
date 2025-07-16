using UnityEngine;


public class ArrowBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    public float damage = 50;
    public float knockbackForce = 0.1f;

    // 引用对象池脚本
    private ArrowObjectPool arrowObjectPool;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // 获取对象池脚本
        arrowObjectPool = FindObjectOfType<ArrowObjectPool>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 如果箭碰到其他物体（例如墙壁或敌人），将其返回到对象池
        arrowObjectPool.ReleaseArrow(gameObject);
    }

    private void OnBecameInvisible()
    {
        // 如果箭飞出屏幕，将其返回到对象池
        arrowObjectPool.ReleaseArrow(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // 获取敌人组件
            Enemy enemy = other.GetComponent<Enemy>();

            // 造成伤害
            enemy.TakeDamage(damage);

            // 计算击退方向
            Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;
            enemy.Knockback(knockbackDirection, knockbackForce);

            // 将箭返回到对象池
            arrowObjectPool.ReleaseArrow(gameObject);
        }
        if (other.CompareTag("Skeleton"))
        {
            // 获取敌人组件
            Skeleton skeleton = other.GetComponent<Skeleton>();

            // 造成伤害
            skeleton.TakeDamage(damage);

            skeleton.isDamage = true;
            skeleton.TransitionToState(SkeletonStateType.DAMAGED);

            // 将箭返回到对象池
            arrowObjectPool.ReleaseArrow(gameObject);
        }
    }
}