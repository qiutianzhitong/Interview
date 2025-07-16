using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flower;  
using UnityEngine.InputSystem;


public class Scene02 : MonoBehaviour
{
    private static bool isFlowerSystemCreated = false; // 静态变量用于标记是否已创建 FlowerSystem
    private static bool isCommandsRegistered = false;
    FlowerSystem fs;
    public InputActions inputActions; 

    // Start is called before the first frame update
     void Start()
    {

        if (!isFlowerSystemCreated)
        {
            fs = FlowerManager.Instance.CreateFlowerSystem("new", false);
            fs.SetupDialog();
            isFlowerSystemCreated = true; // 标记为已创建
        }
        else
        {
            fs = FlowerManager.Instance.GetFlowerSystem("new");
        }

        inputActions = new InputActions();
        inputActions.Enable(); // 启用 InputActions

        if (fs != null&&!isCommandsRegistered)
        {
            fs.ReadTextFromResource("StartStory");
            fs.RegisterCommand("lock", (str) =>
            {
                GameObject.Find("Player").GetComponent<PlayerController>().lockplay = true;
            });
            fs.RegisterCommand("release", (str) =>
            {
                GameObject.Find("Player").GetComponent<PlayerController>().lockplay = false;
            });
            fs.RegisterCommand("Remove", (str) =>
            {
               fs.RemoveDialog();
            });
            isCommandsRegistered = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (fs != null && inputActions.Gameplay.Attack.triggered) 
        {
            fs.Next();
        }
    }
}