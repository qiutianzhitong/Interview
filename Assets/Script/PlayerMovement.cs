using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody2D  rb;
    Vector2 moveDirection;

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
      rb=GetComponent<Rigidbody2D>();  
      animator=GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
      
    }
    private void FixedUpdate(){

        //获取玩家输入
        float moveX=Input.GetAxis("Horizontal");
        float moveY=Input.GetAxis("Vertical");

        //计算移动方向
        moveDirection=new Vector2(moveX,moveY).normalized;


        animator.SetFloat("Horizontal",moveX);
        animator.SetFloat("Vertical",moveY);
        animator.SetFloat("Speed",moveDirection.sqrMagnitude);
        //应用移动
        rb.velocity=moveDirection*moveSpeed;
        
    }
}
