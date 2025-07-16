using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneTo03 : MonoBehaviour
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

            // 找到名为 Story 的 GameObject
            GameObject storyObject = GameObject.Find("Story");
            if (storyObject != null)
            {
                // 获取 Story 对象上的 Scene02 组件
                Scene02 scene02 = storyObject.GetComponent<Scene02>();
                if (scene02 != null)
                {
                    // 禁用 Scene02 中的输入动作
                    scene02.inputActions.Disable();
                }
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

            // 订阅场景加载完成事件
            SceneManager.sceneLoaded += OnSceneLoaded;

            // 切换到目标场景
            SceneManager.LoadScene("03-Map");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 取消订阅场景加载完成事件，避免重复调用
        SceneManager.sceneLoaded -= OnSceneLoaded;

        // 找到新场景中的玩家对象
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
             // 设置玩家的位置
                player.transform.position = new Vector2(0.94f, -0.311f);
            // 获取玩家对象上的 PlayerController 组件
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // 启用玩家的输入动作
                playerController.inputActions.Gameplay.Enable();
            }
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
}    