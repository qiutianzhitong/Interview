using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Istate 
{
    //进入状态
    void OnEnter();
    //更新状态
    void OnUpdate();
    //物理更新状态
    void OnFixUpdate();
    //退出状态
    void OnExit();
}
