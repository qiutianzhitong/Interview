using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flower;  
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class End1Story : MonoBehaviour
{
    FlowerSystem fs;
    public InputActions inputActions; 
    void Start()
    {
    fs = FlowerManager.Instance.CreateFlowerSystem("End1", false);
    inputActions = new InputActions();
    inputActions.Enable(); // 启用 InputActions
    fs.SetupDialog();
    fs.ReadTextFromResource("End1");
    fs.SetupButtonGroup();
    
     fs.RegisterCommand("load", (str) =>
    {
        SceneManager.LoadScene(str[0]);
    });
    }
    void Update()
    {if (inputActions.Gameplay.Attack.triggered) fs.Next();
    }
     void OnDestroy()
    {
        // 在对象销毁时禁用 InputActions
        if (inputActions != null)
        {
            inputActions.Gameplay.Disable();
            inputActions.Dispose();
        }
    }
}
