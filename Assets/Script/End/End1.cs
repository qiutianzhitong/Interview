using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class End1 : MonoBehaviour
{
    [HideInInspector] public GameObject player, canvasObject;
    private Canvas canvas;
    private Hideplayers hidePlayers;

    



    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        canvasObject = GameObject.Find("Canvas");
        hidePlayers = FindObjectOfType<Hideplayers>();



        // 订阅场景加载完成事件
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void Result1()
    {

        SceneManager.LoadScene("End1");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void Back()
    {
          if (hidePlayers != null)
        {
            hidePlayers.Show(); 
        }
        else
        {
            player.transform.position = new Vector2(0.94f, -0.311f);
        } 
        SceneManager.LoadScene("02-Main");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "End1")
        {
            if (player != null)
            {
                player.SetActive(false);
                canvasObject.SetActive(false);
            }
        }
    }

    private void OnDestroy()
    {
        // 取消订阅场景加载完成事件
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
     public void EnablePlayers()
    {
        if (player== null)
        {
            player.SetActive(true);
            canvasObject.SetActive(true);
            player.transform.position = new Vector2(0.94f, -0.311f);
        }
       
    }
}    