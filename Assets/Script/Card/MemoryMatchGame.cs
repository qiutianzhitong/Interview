using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MemoryMatchGame : MonoBehaviour
{
    public int rows = 4;
    public int cols = 4;
    public GameObject cardPrefab;
    public GameObject CardContainer;
    public float cardSpacing = 1.2f;
    public float flipDelay = 1f;

    private List<GameObject> cards = new List<GameObject>();
    private GameObject firstCard;
    private GameObject secondCard;
    private int matchedPairs = 0;
    private int totalPairs;
    private int moveCount = 0;
    public Canvas winCanvas; 
    [HideInInspector] public GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
        totalPairs = rows * cols / 2;
        CreateGameBoard();
        ShuffleCards();
    }

    private void CreateGameBoard()
    {
        // 找到 Canvas 对象
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("未找到 Canvas 对象，请确保场景中存在 Canvas。");
            return;
        }

        int[] icons = new int[rows * cols];
        for (int i = 0; i < totalPairs; i++)
        {
            icons[i * 2] = i;
            icons[i * 2 + 1] = i;
        }

        // 对图标数组进行洗牌
        ShuffleArray(icons);

        // 计算起始位置，使游戏面板居中
        float startX = -(cols - 1) * cardSpacing / 2;
        float startY = (rows - 1) * cardSpacing / 2;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                int index = row * cols + col;
                // 实例化预制体并指定父对象为 CardContainer
                GameObject card = Instantiate(cardPrefab, CardContainer.transform);

                if (card != null)
                {
                    Card cardComponent = card.GetComponent<Card>();
                    if (cardComponent != null)
                    {
                        cardComponent.SetIcon(icons[index]);
                    }
                    else
                    {
                        Debug.LogError($"卡片 {card.name} 上未挂载 Card 脚本");
                    }

                    Button buttonComponent = card.GetComponent<Button>();
                    if (buttonComponent != null)
                    {
                        buttonComponent.onClick.AddListener(() => OnCardClick(card));
                    }
                    else
                    {
                        Debug.LogError($"卡片 {card.name} 上未挂载 Button 组件");
                    }

                    // 设置卡片的位置
                    float x = startX + col * cardSpacing;
                    float y = startY - row * cardSpacing;
                    card.transform.localPosition = new Vector2(x, y);

                    cards.Add(card);
                }
                else
                {
                    Debug.LogError("卡片实例化失败，cardPrefab 可能未正确赋值");
                }
            }
        }
    }

    private void ShuffleArray(int[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int randomIndex = Random.Range(i, array.Length);
            int temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    private void ShuffleCards()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            int randomIndex = Random.Range(i, cards.Count);
            GameObject temp = cards[i];
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
            cards[i].transform.position = new Vector2(cards[i].transform.position.x, cards[i].transform.position.y);
        }
    }

    private void OnCardClick(GameObject card)
    {
        if (firstCard == null)
        {
            firstCard = card;
            firstCard.GetComponent<Card>().Flip();
        }
        else if (secondCard == null && card != firstCard)
        {
            secondCard = card;
            secondCard.GetComponent<Card>().Flip();
            moveCount++;
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(flipDelay);

        if (firstCard.GetComponent<Card>().GetIcon() == secondCard.GetComponent<Card>().GetIcon())
        {
            firstCard.GetComponent<Button>().interactable = false;
            secondCard.GetComponent<Button>().interactable = false;
            firstCard = null;
            secondCard = null;
            matchedPairs++;

            if (matchedPairs == totalPairs)
            {
                winCanvas.gameObject.SetActive(true);
                if (player != null)
                {
                 player.GetComponent<PlayerController>().moveSpeed= 1.5f;
                }
               
            }
        }
        else
        {
            firstCard.GetComponent<Card>().FlipBack();
            secondCard.GetComponent<Card>().FlipBack();
            firstCard = null;
            secondCard = null;
        }
    }

   
}