using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flower;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ChatController : MonoBehaviour
{
    private Animator anim;
    public InputActions inputActions; 
    FlowerSystem flowerSys;
    [HideInInspector] public GameObject hunter, player,Merchant;
    public float interactionDistance = 0.5f;
    public float merchantInteractionDistance = 0.5f;
    private bool isInRange = false,isMerchantInRange = false;
    public bool isFirstClick = true, isChoose = false;
    public bool isMerchantFirstClick = true;
    public static int huntercount=0,Merchantcount = 0;
    private static bool isFlowerSystemCreated = false; // 静态变量用于标记是否已创建 FlowerSystem
    private static bool isCommandsRegistered = false; // 静态变量用于标记是否已注册命令
    private static bool isSetupDialog=false;
    private static ChatController instance;

    // 新增静态变量来保存 isBow 的状态
    public static bool isPlayerBow = false; 

    private void Awake()
    {
        player = GameObject.Find("Player");
     
        if (!isFlowerSystemCreated)
        {
            flowerSys = FlowerManager.Instance.CreateFlowerSystem("FlowerSample", false);
            isFlowerSystemCreated = true;
        }
        else
        {
            flowerSys = FlowerManager.Instance.GetFlowerSystem("FlowerSample");
        }
        flowerSys.Stop();
        inputActions = new InputActions();
        anim = player.GetComponent<Animator>(); 
        inputActions.Gameplay.Attack.started += Attack;
         if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 保持单例不销毁
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 仅在首次使用时注册命令
        if (!isCommandsRegistered)
        {
            flowerSys.RegisterCommand("lock", (str) =>
            {
                GameObject.Find("Player").GetComponent<PlayerController>().lockplay = true;
            });
            flowerSys.RegisterCommand("release", (str) =>
            {
                GameObject.Find("Player").GetComponent<PlayerController>().lockplay = false;
            });
            flowerSys.RegisterCommand("hunterchoose", (str) => { HunterChoose(); }); 
            flowerSys.RegisterCommand("SetFirstClick", (str) => { Invoke("SetFirstClick", 0.5f); });
            flowerSys.RegisterCommand("Ende1Choose", (str) => { Ende1Choose(); }); 
            flowerSys.RegisterCommand("load", (str) =>
            {
                DisableAllInputActions(); // 跳转前禁用输入
                SceneManager.LoadScene(str[0]);
            });
             flowerSys.RegisterCommand("Remove", (str) =>
            {
               flowerSys.RemoveDialog();
            });
            isCommandsRegistered = true;
        }

        // 在新创建 Player 时应用 isBow 状态
        anim.SetBool("isBow", isPlayerBow);

        SceneManager.sceneLoaded += OnSceneLoaded; // 订阅场景加载事件（仅一次，因为单例不销毁）
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        hunter = GameObject.Find("Hunter");
        Merchant= GameObject.Find("Merchant");
        EnableAllInputActions(); // 场景加载完成后启用输入
    }

    private void Update()
    {
        CheckHunterRange();
        CheckMerchantRange();
    }

    private void OnEnable()
    {
        inputActions.Enable(); // 对象启用时启用输入（可能用于手动控制）
    }

    private void OnDisable()
    {
        inputActions.Disable(); // 对象禁用时禁用输入（可能用于手动控制）
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if (isInRange)
        {
           if (isFirstClick)
            {
                switch (huntercount)
                {
                    case 0:
                        huntercount++;
                        flowerSys.ReadTextFromResource("Hunter");
                        flowerSys.Resume();
                        if(!isSetupDialog){
                             flowerSys.SetupDialog();
                             isSetupDialog=true;
                        }
                        isFirstClick = false;
                        break;
                    case 1:
                        flowerSys.ReadTextFromResource("Hunter");
                        flowerSys.Resume();
                        isFirstClick = false;
                        break;
                    case 2:
                        flowerSys.ReadTextFromResource("Hunter1");
                        player.GetComponent<Player>().Restore();
                        flowerSys.Resume();
                        isFirstClick = false;
                        break;
                    case 3:
                        flowerSys.ReadTextFromResource("Hunter2");
                        player.GetComponent<Player>().Restore();
                        PlayerController.isBow3 = true;
                        flowerSys.Resume();
                        isFirstClick = false;
                        break;
                }
            }
            else
            {
                flowerSys.Next();
            }
        }
        else if (isMerchantInRange)
        {
            if(isMerchantFirstClick)
            {
                Merchantcount = PropManager.coinCount;
                if(Merchantcount>=1){
                     if(!isSetupDialog){
                             flowerSys.SetupDialog();
                             isSetupDialog=true;
                        }
                      flowerSys.ReadTextFromResource("Merchant");
                     flowerSys.Resume();
                    isMerchantFirstClick = false;
                }
              
            }
            else
            {
                flowerSys.Next();
            }
        }
    }

    private void CheckHunterRange()
    {
        if (hunter != null)
        {
            float distance = Vector2.Distance(player.transform.position, hunter.transform.position);
            isInRange = distance <= interactionDistance;
        }
    }
     private void CheckMerchantRange()
    {
        if (Merchant != null)
        {
            float distance = Vector2.Distance(player.transform.position, Merchant.transform.position);
            isMerchantInRange = distance <= merchantInteractionDistance;
        }
    }

    void HunterChoose()
    {
        flowerSys.Stop();
        flowerSys.SetupButtonGroup();
        flowerSys.SetupButton("好的", () =>
        {
            anim.SetBool("isBow", true);
            // 更新静态变量
            isPlayerBow = true; 
            huntercount++;
            flowerSys.ReadTextFromResource("Getbow");
            flowerSys.Resume(); // Resume system.
            flowerSys.RemoveButtonGroup(); // Remove the button group.
        });

        flowerSys.SetupButton("不要", () =>
        {
            anim.SetBool("isBow", false);
            // 更新静态变量
            isPlayerBow = false; 
            flowerSys.ReadTextFromResource("Refuse");
            flowerSys.Resume(); // Resume system.
            flowerSys.RemoveButtonGroup(); 
            Invoke("SetFirstClick", 0.5f);
        }); 
    }

    void SetFirstClick()
    {
        isFirstClick = true;
    }

    public void ReceiveEnemyDeathCount(int count)
    {
       if (count >= 10)
            huntercount = 3;
    }

    void Ende1Choose()
    {
        flowerSys.Stop();
        flowerSys.SetupButtonGroup();
        flowerSys.SetupButton("好的", () =>
        {
            anim.SetBool("isBow", true);
            // 更新静态变量
            isPlayerBow = true; 
            flowerSys.ReadTextFromResource("LoadBoss");
            flowerSys.Resume(); // Resume system.
            flowerSys.RemoveButtonGroup(); // Remove the button group.
        });

        flowerSys.SetupButton("不要", () =>
        {
            flowerSys.ReadTextFromResource("Refuse");
            flowerSys.Resume(); // Resume system.
            flowerSys.RemoveButtonGroup(); 
            Invoke("SetFirstClick", 0.5f);
        }); 
    }

    private void EnableAllInputActions()
    {
        //inputActions.Gameplay.Enable(); // 启用当前控制器的输入
        PlayerController playerController = player?.GetComponent<PlayerController>();
        playerController?.inputActions.Gameplay.Enable(); // 启用玩家的输入
    }

    private void DisableAllInputActions()
    {
        inputActions.Gameplay.Disable(); // 禁用当前控制器的输入
        PlayerController playerController = player?.GetComponent<PlayerController>();
        playerController?.inputActions.Gameplay.Disable(); // 禁用玩家的输入
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
    }

   
}