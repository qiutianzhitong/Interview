using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hideplayers : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector] public GameObject player, canvasObject;
    private Canvas canvas;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        canvasObject = GameObject.Find("Canvas");
        Hide();

    }

    void Hide()
    {
        if (player != null)
        {
            player.SetActive(false);
            canvasObject.SetActive(false);
        }
    }
   public void Show()
    {
        if (player != null)
        {
            player.SetActive(true);
            canvasObject.SetActive(true);
             player.transform.position = new Vector2(0.94f, -0.311f);
        }
   }
}
