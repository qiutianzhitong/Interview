using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class ArrowObjectPool : MonoBehaviour
{
    public static ArrowObjectPool Instance;

    // 使用 AssetReference 代替直接的 GameObject 引用
    public AssetReference arrowPrefabReference;
    private ObjectPool<GameObject> arrowPool;
    private AsyncOperationHandle<GameObject> loadHandle;
    private bool isInitialized = false;
    private bool isSceneChanging = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 注册场景加载事件
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        // 开始异步初始化
        InitializePoolAsync();
    }

    // 异步初始化对象池
    private async void InitializePoolAsync()
    {
        // 如果已经初始化，不再重复初始化
        if (isInitialized) return;
        
        // 加载弓箭预制体
        loadHandle = arrowPrefabReference.LoadAssetAsync<GameObject>();
        await loadHandle.Task;

        if (loadHandle.Status == AsyncOperationStatus.Succeeded)
        {
            // 创建对象池
            arrowPool = new ObjectPool<GameObject>(
                CreateArrow,
                OnTakeFromPool,
                OnReturnedToPool,
                OnDestroyPoolObject,
                false, 10, 100
            );
            
            isInitialized = true;
            Debug.Log("弓箭对象池初始化完成");
        }
        else
        {
            Debug.LogError("加载弓箭预制体失败: " + loadHandle.OperationException);
        }
    }

    private GameObject CreateArrow()
    {
        // 使用加载的预制体实例化对象
        if (loadHandle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject arrow = Instantiate(loadHandle.Result);
            // 确保对象不会在场景切换时被自动销毁
            DontDestroyOnLoad(arrow);
            return arrow;
        }
        
        Debug.LogError("无法创建弓箭，预制体加载失败");
        return null;
    }

    private void OnTakeFromPool(GameObject arrow)
    {
        if (arrow != null)
        {
            arrow.SetActive(true);
        }
    }

    private void OnReturnedToPool(GameObject arrow)
    {
        if (arrow != null)
        {
            arrow.SetActive(false);
        }
    }

    private void OnDestroyPoolObject(GameObject arrow)
    {
        if (arrow != null)
        {
            Destroy(arrow);
        }
    }

    // 获取弓箭实例（添加错误检查）
    public GameObject GetArrow()
    {
        // 如果正在切换场景或未初始化完成，尝试重新初始化
        if (isSceneChanging || !isInitialized)
        {
            Debug.LogWarning("对象池状态异常，尝试重新初始化");
            InitializePoolAsync();
            return null;
        }
        
        if (arrowPool == null)
        {
            Debug.LogError("对象池未正确初始化");
            InitializePoolAsync();
            return null;
        }
        
        return arrowPool.Get();
    }

    // 释放弓箭实例
    public void ReleaseArrow(GameObject arrow)
    {
        if (!isInitialized || arrow == null || arrowPool == null)
            return;
            
        // 检查对象是否已被销毁
        if (arrow != null)
        {
            arrowPool.Release(arrow);
        }
    }

    // 场景加载完成回调
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isSceneChanging = false;
        Debug.Log("场景加载完成，对象池状态: " + (isInitialized ? "已初始化" : "未初始化"));
    }

    // 场景卸载回调
    private void OnSceneUnloaded(Scene scene)
    {
        isSceneChanging = true;
        
        // 清理对象池，但保留预制体引用
        if (isInitialized && arrowPool != null)
        {
            arrowPool.Clear();
            Debug.Log("场景切换，已清理弓箭对象池");
        }
    }

    private void OnDestroy()
    {
        // 释放 Addressables 资源
        if (loadHandle.IsValid())
        {
            Addressables.Release(loadHandle);
        }
        
        // 取消注册事件
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        
        // 清理对象池
        if (arrowPool != null)
        {
            arrowPool.Dispose();
            arrowPool = null;
        }
        
        isInitialized = false;
    }
}