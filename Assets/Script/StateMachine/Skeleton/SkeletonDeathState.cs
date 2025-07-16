using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDeathState :Istate
{
    private Skeleton skeleton;
    public SkeletonDeathState(Skeleton skeleton)
    {
        this.skeleton = skeleton;
    }
    public void OnEnter()
    { 
        skeleton.navMeshAgent.enabled = false;
        skeleton.anim.Play("DEATH");
    }

    public void OnExit()
    { 
    }

    public void OnFixUpdate()
    {
        
    }

    public void OnUpdate()
    {
       
    }

   
}
