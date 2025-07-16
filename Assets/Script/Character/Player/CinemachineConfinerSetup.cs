using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class CinemachineConfinerSetup : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        // 注册场景加载事件
        SceneManager.sceneLoaded += OnSceneLoaded;
        // 首次启动时也设置一下
        SetupConfiner();
    }

    private void OnDestroy()
    {
        // 注销场景加载事件
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 场景加载完成后设置 Confiner
        SetupConfiner();
    }

    private void SetupConfiner()
    {
        // 获取 Cinemachine Confiner 组件
        CinemachineConfiner confiner = virtualCamera.GetComponent<CinemachineConfiner>();
        if (confiner == null)
        {
            Debug.LogError("Cinemachine Confiner component not found on the virtual camera.");
            return;
        }

        // 查找场景中的 Grid 的 Polygon Collider 2D
        PolygonCollider2D gridCollider = FindGridPolygonCollider();
        if (gridCollider != null)
        {
            // 设置 Bounding Shape 2D
            confiner.m_BoundingShape2D = gridCollider;
            // 刷新 Cinemachine Confiner
            confiner.InvalidatePathCache();
           // Debug.Log("Cinemachine Confiner Bounding Shape 2D set to Grid's Polygon Collider 2D.");
        }
        else
        {
            Debug.LogWarning("No Grid's Polygon Collider 2D found in the scene.");
        }
    }

    private PolygonCollider2D FindGridPolygonCollider()
    {
        // 查找场景中的所有 Polygon Collider 2D
        PolygonCollider2D[] colliders = FindObjectsOfType<PolygonCollider2D>();
        foreach (PolygonCollider2D collider in colliders)
        {
            // 假设 Grid 的 Polygon Collider 2D 挂载在名为 "Grid" 的 GameObject 上
            if (collider.gameObject.name == "Grid")
            {
                return collider;
            }
        }
        return null;
    }
}    