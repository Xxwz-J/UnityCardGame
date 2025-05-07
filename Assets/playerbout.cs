using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class playerbout : MonoBehaviour
{
    public Canvas controller;
    public GameObject newCard;//卡牌的预制体
    public MonsterCard[] cards; //牌堆
    public List<Card> handcards;//手牌
    public int num; //手牌数量
    private int index; //抽牌堆中第几张牌
    private gamecontroller con;
    private bool inited;
    private bool ready;
    private bool selected;
    private Vector2 position1 = new Vector2(-268, 103);
    private Vector2 position2 = new Vector2(-118, 103);
    private Vector2 position3 = new Vector2(32, 103);
    private Vector2 position4 = new Vector2(182, 103);
    private Vector2 size1 = new Vector2(50, 50);
    private GameObject targetObj1;
    private GameObject targetObj2;
    private GameObject targetObj3;
    private GameObject targetObj4;
    private CardStore allCards;
    private PlayerData data;
    // Start is called before the first frame update
    void Start()
    {
        inited = false;
        ready = false;
        selected = true;
        index = 0;
        con = controller.GetComponent<gamecontroller>();
        data = controller.GetComponent<PlayerData>();
        allCards = GetComponent<CardStore>();
        //GetInitCards();
        num = 4;
        InitTarget();
        ShowCards();
    }
    private void GetInitCards()
    {
        int n = 0;
        for(int i=0;i<11;i++)
        {
            n += data.playerCards[i].Count;
        }
        cards = new MonsterCard[n];

        int id = 0;
        for (int i = 0; i < 11; i++)
        {
            int nu = data.playerCards[i].Count;
            LinkedListNode<Card> p = data.playerCards[i].First;
            for (int j = 0; j < nu; j++)
            {
                cards[id] = (MonsterCard)p.Value;
                if (j != nu - 1)
                    p = p.Next;
            }
        }
        System.Random rng = new System.Random(); // 创建随机数生成器
        int nn = cards.Length;
        while (n > 1)
        {
            n--; // 当前未打乱的元素数量
            int k = rng.Next(n + 1); // 随机选择一个索引
            MonsterCard temp = cards[k]; // 交换当前元素和随机选中的元素
            cards[k] = cards[n];
            cards[n] = temp;
        }
        if (cards[0].sacrifice!=1)
        {
            int m = 1;
            while(m<n)
            {
                if (cards[m].sacrifice == 1)
                {
                    MonsterCard temp = cards[0]; // 交换当前元素和随机选中的元素
                    cards[0] = cards[m];
                    cards[m] = temp;
                }
                else m++;
            }
        }
        handcards.Add(GetBasicCard());
        handcards.Add(GetOneHandCard());
        handcards.Add(GetOneHandCard());
        handcards.Add(GetOneHandCard());
    }
    private void ShowCards()
    {
        Vector2 size = new Vector2(107, 133);
        int setx = 60 - 120 * (num / 2);
        for(int i=0;i<num;i++)
        {
            Vector2 position = new Vector2(setx, -80);
            BuildShowedcard(position, size,i);
            setx += 120;
        }
    }
    private void BuildShowedcard(Vector2 position, Vector2 size,int id)
    {
        GameObject square = Instantiate(newCard);

        square.transform.SetParent(FindObjectOfType<Canvas>().transform);

        RectTransform rectTransform = square.GetComponent<RectTransform>();
        square.transform.SetParent(transform, false);

        rectTransform.anchoredPosition = position;
        rectTransform.sizeDelta = size;

        square.AddComponent<playhand>();
        playhand thisone = square.GetComponent<playhand>();
        thisone.TargetArea1 = targetObj1;
        thisone.TargetArea2 = targetObj2;
        thisone.TargetArea3 = targetObj3;
        thisone.TargetArea4 = targetObj4;
        thisone.controller = controller;
        //thisone.card = handcards[id];

        square.AddComponent<SelectObl>();
    }
    private void Destroyshowedcards()
    {
        for(int i=0;i<num;i++)
        {
            GameObject square = GameObject.Find("UI Square");
            if (square != null)
            {
                Destroy(square);
            }
            else break;
        }
    }
    private void InitTarget()
    {
        targetObj1 = new GameObject("DragTarget1");
        RectTransform tempTarget1 = targetObj1.AddComponent<RectTransform>();
        tempTarget1.transform.SetParent(transform, false);
        tempTarget1.anchoredPosition = position1;
        tempTarget1.sizeDelta = size1;

        targetObj2 = new GameObject("DragTarget2");
        RectTransform tempTarget2 = targetObj2.AddComponent<RectTransform>();
        tempTarget2.transform.SetParent(transform, false);
        tempTarget2.anchoredPosition = position2;
        tempTarget2.sizeDelta = size1;

        targetObj3 = new GameObject("DragTarget3");
        RectTransform tempTarget3 = targetObj3.AddComponent<RectTransform>();
        tempTarget3.transform.SetParent(transform, false);
        tempTarget3.anchoredPosition = position3;
        tempTarget3.sizeDelta = size1;

        targetObj4 = new GameObject("DragTarget4");
        RectTransform tempTarget4 = targetObj4.AddComponent<RectTransform>();
        tempTarget4.transform.SetParent(transform, false);
        tempTarget4.anchoredPosition = position4;
        tempTarget4.sizeDelta = size1;

        inited = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (con.isplayerbout && !inited)
        {
            ready = false;
            selected = false;
        }
        if (con.isplayerbout && ready)
        {
            con.isplayerbout = false;
            ready = false;
        }
    }
    private Card GetOneHandCard()
    {
        index++;
        return cards[index];
    }
    private Card GetBasicCard()
    {
        Card newcard = new Card();
        newcard = allCards.cards[0];
        return newcard;
    }
    public void OnClick1()
    {
        if (con.isplayerbout && !selected)
        {
            handcards.Add(GetOneHandCard());
            Destroyshowedcards();
            num++;
            ShowCards();
            selected = true;
        }
    }
    public void Onclik2()
    {
        if (con.isplayerbout && !selected)
        {
            handcards.Add(GetBasicCard());
            Destroyshowedcards();
            num++;
            ShowCards();
            selected = true;
        }
    }
    public void OnClick3()
    {
        if (selected)
        {
            ready = true;
            selected = false;
        }
    }
}
