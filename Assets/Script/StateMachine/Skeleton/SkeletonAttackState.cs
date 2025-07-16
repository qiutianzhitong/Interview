using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : Istate
{
    private readonly Skeleton skeleton;
     public SkeletonAttackState (Skeleton skeleton)
    { 
        this.skeleton = skeleton;
    }
    public void OnEnter()
    {
        skeleton.anim.Play("ATTACK");
        skeleton.navMeshAgent.enabled = false;
        
    }

    public void OnExit()
    {
        skeleton.navMeshAgent.enabled = true;
        
    }

    public void OnFixUpdate()
    {
       
    }

    public void OnUpdate()
    {
        
        if(skeleton.isDamage)
        {
            skeleton.TransitionToState(SkeletonStateType.DAMAGED);
        }
        if(skeleton.isDeath){
            skeleton.TransitionToState(SkeletonStateType.DEATH);
        }
        
        float distanceToPlayer = Vector2.Distance(skeleton.transform.position, skeleton.playerTransform.position);
          if (distanceToPlayer <= skeleton.chaseRange)
        {
           if (distanceToPlayer <= skeleton.AttackRange)
               {
                skeleton.TransitionToState(SkeletonStateType.ATTACK);
               }
           else
           {
            skeleton.TransitionToState(SkeletonStateType.MOVE);
            
           }
                
        }
        else
        {
            if (skeleton.currentState is SkeletonMoveState)
            {
                skeleton.TransitionToState(SkeletonStateType.IDLE);
            }
        }
    }


}
