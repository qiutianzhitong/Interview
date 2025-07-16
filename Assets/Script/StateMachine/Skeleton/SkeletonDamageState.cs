using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDamageState :Istate
{
    private Skeleton skeleton;
    private Vector2 direction;
    private float Timer;
    public SkeletonDamageState(Skeleton skeleton)
    {
        this.skeleton = skeleton;
    }
    public void OnEnter()
    {
        skeleton.anim.Play("DAMAGED");
        skeleton.navMeshAgent.enabled = false;
        
    }

    public void OnExit()
    {
         skeleton.isDamage=false;
         skeleton.navMeshAgent.enabled = true;
        
    }

    public void OnFixUpdate()
    {
        if(Timer<=skeleton.InvulnerableDuration){
           skeleton.rb.AddForce(-0.1f*direction,ForceMode2D.Impulse);
           Timer+=Time.fixedDeltaTime;
        }
        else{
            Timer=0;
            skeleton.isDamage=false;
            skeleton.TransitionToState(SkeletonStateType.IDLE);
        }
        
    }

    public void OnUpdate()
    {
        if(skeleton.isDeath){
            skeleton.TransitionToState(SkeletonStateType.DEATH);
        }
       if(skeleton.isDamage){
        direction=(skeleton.playerTransform.position-skeleton.transform.position).normalized;
       }
    }

 
}
