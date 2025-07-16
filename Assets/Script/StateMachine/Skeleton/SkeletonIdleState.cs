using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : Istate
{
    private readonly Skeleton skeleton;

    public SkeletonIdleState(Skeleton skeleton)
    {
        this.skeleton = skeleton;
    }
    public void OnEnter()
    {
        skeleton.anim.Play("IDLE");
        
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
            if (skeleton.currentState is SkeletonIdleState)
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
