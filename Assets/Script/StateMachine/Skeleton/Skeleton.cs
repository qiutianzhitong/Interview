using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum SkeletonStateType
{
    IDLE,
    MOVE,
    ATTACK,
    DAMAGED,
    DEATH
}

public class Skeleton : Character
{
    private Dictionary<SkeletonStateType, Istate> stateDictionary = new Dictionary<SkeletonStateType, Istate>();
    public Istate currentState;

    public float chaseRange=2; 
    public float AttackRange=0.5f; 
    public Transform playerTransform; // 玩家的 Transform 组件
    [HideInInspector]public Animator anim;
    [HideInInspector]public SpriteRenderer sr; // 敌人的 SpriteRenderer 组件
    [HideInInspector]public Rigidbody2D rb; // 敌人的 Rigidbody2D 组件
    public NavMeshAgent navMeshAgent;
    public float damage;
    [HideInInspector]public Vector3 eulerAngles;
    [HideInInspector]public bool isKnockback = false;
    public LayerMask PlayerLayer;
    public float AttackDuration=2f;
    [HideInInspector]public bool isDamage = false;
    [HideInInspector]public bool isDeath = false;
    [HideInInspector]public float knockbackDuration=0.5f;


    void Awake()
    {
        
        eulerAngles=GetComponent<Transform>().eulerAngles;
        rb = GetComponent<Rigidbody2D>();
        sr=GetComponent<SpriteRenderer>();

      
    }

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
         stateDictionary.Add(SkeletonStateType.IDLE, new SkeletonIdleState(this));
        stateDictionary.Add(SkeletonStateType.MOVE, new SkeletonMoveState(this));
        stateDictionary.Add(SkeletonStateType.ATTACK, new SkeletonAttackState(this));
        stateDictionary.Add(SkeletonStateType.DAMAGED, new SkeletonDamageState(this));
        stateDictionary.Add(SkeletonStateType.DEATH, new SkeletonDeathState(this));
        TransitionToState(SkeletonStateType.IDLE);
    }
    void FixedUpdate()
    {
        currentState.OnFixUpdate(); 
         eulerAngles.z=0;  
        transform.eulerAngles=eulerAngles;
    }

    void Update()
    {
        currentState.OnUpdate(); 
        eulerAngles.z=0;  
        transform.eulerAngles=eulerAngles;
       
        
    }
    public void TransitionToState(SkeletonStateType stateType)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = stateDictionary[stateType];
        currentState.OnEnter();
    }
    public void Attack()
    {
      Collider2D [] hitColliders = Physics2D.OverlapCircleAll(transform.position, AttackRange, PlayerLayer);
      foreach (Collider2D hitCollider in hitColliders)
      {
          if (hitCollider.CompareTag("Player"))
          {
              hitCollider.GetComponent<Character>().TakeDamage(damage);
          }
      }
    } 
    public override void Die()
    {
        isDeath = true;
        TransitionToState(SkeletonStateType.DEATH);
         Destroy(this.gameObject, 0.6f);
       
    }
    
    
    
}