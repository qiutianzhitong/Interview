using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    public InputActions inputActions;
    public Vector2 inputDirection;
    public float moveSpeed;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    Vector2 moveDirection;
    private Vector2 lastInputDirection = Vector2.zero;
    public bool lockplay = false;

    public static bool isBow3 = false;

    // 引用对象池脚本
    public ArrowObjectPool arrowObjectPool;

    private void Awake()
    {
        inputActions = new InputActions();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        // 攻击
        inputActions.Gameplay.Attack.started += Attack;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        inputDirection = inputActions.Gameplay.Move.ReadValue<Vector2>();
        if (inputDirection != Vector2.zero)
        {
            lastInputDirection = inputDirection;
        }

        // 更新 Animator 中的 Direction 参数
        UpdateDirection();
    }

    private void FixedUpdate()
    {
        if (!lockplay)
            Move();
    }

    void Move()
    {
        rb.velocity = inputDirection * moveSpeed;
        if (inputDirection.x < 0)
        {
            sr.flipX = true;
        }
        if (inputDirection.x > 0)
        {
            sr.flipX = false;
        }

        // 获取玩家输入
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // 计算移动方向
        moveDirection = new Vector2(moveX, moveY).normalized;
        anim.SetFloat("Horizontal", moveX);
        anim.SetFloat("Vertical", moveY);
        anim.SetFloat("Speed", moveDirection.sqrMagnitude);
    }

    void UpdateDirection()
    {
        // 根据玩家的最后输入方向设置 Direction 变量
        float deadZone = 0.2f; // 可以根据需要调整这个值

        // 判断输入方向
        if (inputDirection.magnitude > deadZone) // 摇杆有有效输入
        {
            // 垂直方向判断
            if (inputDirection.y > 0) // 向上
            {
                if (Math.Abs(inputDirection.y) > Math.Abs(inputDirection.x))
                    anim.SetInteger("Direction", 2);
                else if (Math.Abs(inputDirection.y) < Math.Abs(inputDirection.x))
                    anim.SetInteger("Direction", 0);
            }
            else if (inputDirection.y < 0) // 向下
            {
                if (Math.Abs(inputDirection.y) > Math.Abs(inputDirection.x))
                    anim.SetInteger("Direction", 1);
                else if (Math.Abs(inputDirection.y) < Math.Abs(inputDirection.x))
                    anim.SetInteger("Direction", 0);
            }
            // 水平方向判断
            else
            {
                anim.SetInteger("Direction", 0);
            }
        }
    }

    private void Attack(InputAction.CallbackContext obj)
    {
        if (lockplay) return;
        // 设置攻击动画
        anim.SetBool("isAttack", true);
        // 延迟0.5秒后生成箭
        if (anim.GetBool("isBow"))
        {
            if (isBow3)
                Invoke(nameof(SpawnArrows), 0.5f);
            else
                Invoke(nameof(SpawnArrow), 0.5f);
        }
    }

    private void SpawnArrow()
    {
        // 从对象池获取箭
        GameObject arrow = arrowObjectPool.GetArrow();

        // 根据角色的朝向设置箭的位置和旋转
        Vector2 spawnOffset = GetSpawnOffset();
        Vector2 spawnPosition = (Vector2)transform.position + spawnOffset;
        arrow.transform.position = spawnPosition;
        arrow.transform.rotation = GetArrowRotation();

        // 设置箭的方向
        Vector2 direction = GetArrowDirection();
        Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();
        arrowRb.velocity = direction * moveSpeed;

        // 如果角色朝左，箭也会朝左
        SpriteRenderer arrowSr = arrow.GetComponent<SpriteRenderer>();
        if (sr.flipX)
        {
            arrowSr.flipX = true;
        }
    }

    private void SpawnArrows()
    {
        // 偏移量，可根据需要调整
        float offset = 0.1f;
        // 生成三只箭
        for (int i = -1; i <= 1; i++)
        {
            // 从对象池获取箭
            GameObject arrow = arrowObjectPool.GetArrow();

            // 根据角色的朝向设置箭的位置和旋转
            Vector2 spawnOffset = GetSpawnOffset();
            if (anim.GetInteger("Direction") == 0) // 左右方向
            {
                spawnOffset.y += i * offset;
            }
            else // 上下方向
            {
                spawnOffset.x += i * offset;
            }
            Vector2 spawnPosition = (Vector2)transform.position + spawnOffset;
            arrow.transform.position = spawnPosition;
            arrow.transform.rotation = GetArrowRotation();

            // 设置箭的方向
            Vector2 direction = GetArrowDirection();
            Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();
            arrowRb.velocity = direction * moveSpeed;

            // 如果角色朝左，箭也会朝左
            SpriteRenderer arrowSr = arrow.GetComponent<SpriteRenderer>();
            if (sr.flipX)
            {
                arrowSr.flipX = true;
            }
        }
    }

    // 获取箭的生成偏移量
    private Vector2 GetSpawnOffset()
    {
        int direction = anim.GetInteger("Direction");

        switch (direction)
        {
            case 0: // 右或左
                return sr.flipX ? new Vector2(-0.2f, -0.05f) : new Vector2(0.2f, -0.05f);
            case 1: // 下
                return new Vector2(0f, -0.2f);
            case 2: // 上
                return new Vector2(0f, 0.2f);
            default:
                return new Vector2(0.2f, 0f);
        }
    }

    // 获取箭的飞行方向
    private Vector2 GetArrowDirection()
    {
        int direction = anim.GetInteger("Direction");

        switch (direction)
        {
            case 0: // 右或左
                return sr.flipX ? Vector2.left : Vector2.right;
            case 1: // 下
                return Vector2.down;
            case 2: // 上
                return Vector2.up;
            default:
                return Vector2.right;
        }
    }

    // 获取箭的旋转角度
    private Quaternion GetArrowRotation()
    {
        int direction = anim.GetInteger("Direction");

        switch (direction)
        {
            case 0: // 右或左
                return sr.flipX ? Quaternion.Euler(0f, 0f, 90f) : Quaternion.Euler(0f, 0f, -90f);
            case 1: // 下
                return Quaternion.Euler(0f, 0f, 180f);
            case 2: // 上
                return Quaternion.Euler(0f, 0f, 0);
            default:
                return Quaternion.identity;
        }
    }
}