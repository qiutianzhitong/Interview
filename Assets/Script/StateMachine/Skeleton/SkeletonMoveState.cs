using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : Istate
{
    private readonly Skeleton skeleton;
    public SkeletonMoveState(Skeleton skeleton)
    {
        this.skeleton = skeleton;
    }
    public void OnEnter()
    {
       skeleton.anim.Play("MOVE");
       skeleton.navMeshAgent.SetDestination(skeleton.playerTransform.position);
       Vector2 direction = (skeleton.playerTransform.position - skeleton.transform.position).normalized;
       if (direction.x < 0)
        {
           skeleton.eulerAngles.y=0;
        }
        else if (direction.x > 0)
        {
             skeleton.eulerAngles.y=180;
        }
    }

    public void OnExit()
    {
        
    }

    public void OnFixUpdate()
    {
       
    }

    public void OnUpdate()
    {
        if(skeleton.isDeath){
            skeleton.TransitionToState(SkeletonStateType.DEATH);
        }
        
        if(skeleton.isDamage)
        {
            skeleton.TransitionToState(SkeletonStateType.DAMAGED);
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
