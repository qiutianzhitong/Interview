using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneTo02 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检查进入触发区域的是否为玩家
        if (other.CompareTag("Player"))
        {
            // 获取玩家对象上的 PlayerController 组件
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // 禁用玩家的输入动作
                playerController.inputActions.Gameplay.Disable();
            }
               // 找到挂载 ChatController 脚本的 GameObject（假设对象名为 ChatControllerObj，可按需修改）
            GameObject chatControllerObj = GameObject.Find("Canvas");
            
            if (chatControllerObj != null)
            {
                // 获取 ChatController 组件
                ChatController chatController = chatControllerObj.GetComponent<ChatController>();
                if (chatController != null)
                {
                    // 禁用 ChatController 中的输入动作
                    chatController.inputActions.Disable();
                }
            }
            // 监听场景加载完成事件
            SceneManager.sceneLoaded += OnSceneLoaded;
            // 切换到目标场景
            SceneManager.LoadScene("02-Main");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "02-Main")
        {
            // 找到玩家对象
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                // 设置玩家的位置
                player.transform.position = new Vector2(-0.27f, 0.83f);
                PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // 启用玩家的输入动作
                playerController.inputActions.Gameplay.Enable();
            }
             // 找到挂载 ChatController 脚本的 GameObject（假设对象名为 ChatControllerObj，可按需修改）
        GameObject chatControllerObj = GameObject.Find("Canvas");
        if (chatControllerObj != null)
        {
            // 获取 ChatController 组件
            ChatController chatController = chatControllerObj.GetComponent<ChatController>();
            if (chatController != null)
            {
                // 启用 ChatController 中的输入动作
                chatController.inputActions.Enable();
            }
        }
            }
            // 取消监听场景加载完成事件
            SceneManager.sceneLoaded -= OnSceneLoaded;
            
        }
        
    }
}    