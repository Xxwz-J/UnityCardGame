using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static System.Net.Mime.MediaTypeNames;
using static Unity.Burst.Intrinsics.Arm;

public class playerbout : MonoBehaviour
{
    public Sprite sprite;
    public GameObject controller;
    public GameObject button;
    public GameObject newCard;//卡牌的预制体
    public MonsterCard[] cards; //牌堆
    public List<MonsterCard> handcards=new List<MonsterCard>();//手牌
    public List<GameObject> showedCards = new List<GameObject>();
    private int index; //抽牌堆中第几张牌
    private gamecontroller con;
    private bool inited;
    private bool ready;
    public bool selected;
    private Vector2 positionInit1 = new Vector2(565, -140);
    private Vector2 positionInit2 = new Vector2(815, -140);
    private Vector2[] position = new Vector2[4];
    private Vector2 size = new Vector2(105, 140);
    private GameObject targetObj1;
    private GameObject targetObj2;
    private GameObject targetObj3;
    private GameObject targetObj4;
    private CardStore allCards;
    private PlayerData data;
    private bool[] showed = new bool[9];
    private int timer = 0;
    private bool limit;
    // Start is called before the first frame update
    void Start()
    {
        inited = false;
        ready = false;
        selected = true;
        index = -1;
        position[0] = new Vector2(-520, -140);
        position[1] = new Vector2(-260, -140);
        position[2] = new Vector2(0, -140);
        position[3] = new Vector2(260, -140);
        con = controller.GetComponent<gamecontroller>();
        data = controller.GetComponent<PlayerData>();
        allCards = controller.GetComponent<CardStore>();
        for (int i = 0; i < 7; i++)
            showed[i] = false;
        InitTarget();
        GetInitCards();
    }
    private void GetInitCards1()
    {
        MonsterCard newone = GetBasicCard();
        handcards.Add(newone);
        BuildShowedcard(positionInit2, size, newone);
        CardsMove();
        MonsterCard newone1 = new MonsterCard((MonsterCard)allCards.cards[9]);
        handcards.Add(newone1);
        BuildShowedcard(positionInit1, size, newone1);
        CardsMove();
        MonsterCard newone2 = new MonsterCard((MonsterCard)allCards.cards[13]);
        handcards.Add(newone2);
        BuildShowedcard(positionInit1, size, newone2);
        CardsMove();
        MonsterCard newone3 = new MonsterCard((MonsterCard)allCards.cards[13]);
        handcards.Add(newone3);
        BuildShowedcard(positionInit1, size, newone3);
        CardsMove();
    }
    private void GetInitCards()
    {
        int n = 0;
        for(int i=1;i<11;i++)
        {
            n += data.playerCards[i].Count;
        }
        Debug.Log(n);
        cards = new MonsterCard[n];

        int id = 0;
        for (int i = 1; i < 11; i++)
        {
            int nu = data.playerCards[i].Count;
            LinkedListNode<Card> q = data.playerCards[i].First;
            for (int j = 0; j < nu; j++)
            {
                MonsterCard p = (MonsterCard)q.Value;
                cards[id] = p;
                id++;
                if (j != nu - 1)
                    q = q.Next;
            }
        }

        for (int i = 0; i < n; i++)
        {
            Debug.Log(cards[i].ToString());
            Debug.Log(cards[i].sacrifice);
        }

        for (int i = 0; i < n; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, n);
            MonsterCard temp = cards[i];
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
        }
        for (int i=0;i<n;i++)
        {
            Debug.Log(cards[i].ToString());
            Debug.Log(cards[i].sacrifice);
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
                    break;
                }
                else m++;
            }
        }
        MonsterCard newone = GetBasicCard();
        handcards.Add(newone);
        BuildShowedcard(positionInit2, size, newone);
        MonsterCard newone0 = GetOneHandCard();
        handcards.Add(newone0);
        BuildShowedcard(positionInit1, size, newone0);
        MonsterCard newone1 = GetOneHandCard();
        handcards.Add(newone1);
        BuildShowedcard(positionInit1, size, newone1);
        MonsterCard newone2 = GetOneHandCard();
        handcards.Add(newone2);
        BuildShowedcard(positionInit1, size, newone2);
        CardsMove();
    }
    private void BuildShowedcard(Vector2 position, Vector2 size,MonsterCard card)
    {
        GameObject square = Instantiate(newCard);
        square.GetComponent<CardDisplay>().card = card;

        square.transform.SetParent(FindObjectOfType<Canvas>().transform);

        RectTransform rectTransform = square.GetComponent<RectTransform>();
        square.transform.SetParent(transform, false);

        rectTransform.anchoredPosition = position;
        rectTransform.localScale = new Vector2(0.7f, 0.7f);
        rectTransform.rotation = transform.Find("deck").rotation;

        square.AddComponent<playhand>();
        playhand thisone = square.GetComponent<playhand>();
        thisone.TargetArea1 = targetObj1;
        thisone.TargetArea2 = targetObj2;
        thisone.TargetArea3 = targetObj3;
        thisone.TargetArea4 = targetObj4;
        thisone.controller = controller;
        thisone.card = card;
        thisone.showedcard = square;
        thisone.player = gameObject;

        square.AddComponent<SelectObl>().sprite = sprite;
        Attack a=square.AddComponent<Attack>();
        a.enabled = false;
        a.contorller = controller;
        Shake s=square.AddComponent<Shake>();
        s.enabled = false;
        DelEvent d=square.AddComponent<DelEvent>();
        d.controller = controller;
        d.enabled = false;
        square.AddComponent<MoveE>().enabled = false;
        square.GetComponent<MoveE>().controller = controller;
        showedCards.Add(square);
    }
    public void CardsMove()
    {
        int num = handcards.Count;
        float setx = 70 - 120 * num;
        for(int i=0;i<num;i++)
        {
            Vector3 position = new Vector3(setx, -510, 0);
            MoveE e= showedCards[i].GetComponent<MoveE>();
            e.position = position;
            e.enabled = true;
            setx += 240;
        }
    }
    private void InitTarget()
    {
        targetObj1 = new GameObject("DragTarget1");
        RectTransform tempTarget1 = targetObj1.AddComponent<RectTransform>();
        tempTarget1.transform.SetParent(transform, false);
        tempTarget1.anchoredPosition = position[0];
        tempTarget1.sizeDelta = size;

        targetObj2 = new GameObject("DragTarget2");
        RectTransform tempTarget2 = targetObj2.AddComponent<RectTransform>();
        tempTarget2.transform.SetParent(transform, false);
        tempTarget2.anchoredPosition = position[1];
        tempTarget2.sizeDelta = size;

        targetObj3 = new GameObject("DragTarget3");
        RectTransform tempTarget3 = targetObj3.AddComponent<RectTransform>();
        tempTarget3.transform.SetParent(transform, false);
        tempTarget3.anchoredPosition = position[2];
        tempTarget3.sizeDelta = size;

        targetObj4 = new GameObject("DragTarget4");
        RectTransform tempTarget4 = targetObj4.AddComponent<RectTransform>();
        tempTarget4.transform.SetParent(transform, false);
        tempTarget4.anchoredPosition = position[3];
        tempTarget4.sizeDelta = size;

        inited = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (con.isFirstGame && !showed[8])
        {
            FirstGame();
        }
        if (con.isplayerbout && !inited)
        {
            ready = false;
            selected = false;
        }
        if (con.isplayerbout && ready)
        {
            con.isplayerbout = false;
            ready = false;
            selected = false;
            inited = false;
        }
    }
    private MonsterCard GetOneHandCard()
    {
        index++;
        if(index==cards.Length-1)
        {
            button.SetActive(false);
        }
        return cards[index];
    }
    private MonsterCard GetBasicCard()
    {
        MonsterCard newcard = new MonsterCard((MonsterCard)allCards.cards[0]);
        return newcard;
    }
    public void OnClick1()
    {
        if (con.isplayerbout && !selected)
        {
            MonsterCard newone = GetOneHandCard();
            handcards.Add(newone);
            BuildShowedcard(positionInit1, size, newone);
            CardsMove();
            selected = true;
        }
    }
    public void Onclik2()
    {
        if (con.isplayerbout && !selected)
        {
            MonsterCard newone = GetBasicCard();
            handcards.Add(newone);
            BuildShowedcard(positionInit2, size, newone);
            CardsMove();
            selected = true;
        }
    }
    public void OnClick3()
    {
        if (!con.isFirstGame || showed[7])
            if (selected)
            {
                ready = true;
                selected = false;
            }
            else con.ShowText(6);
    }

    public void GetAward1()
    {
        MonsterCard newone = GetBasicCard();
        handcards.Add(newone);
        BuildShowedcard(positionInit2, size, newone);
        CardsMove();
        MonsterCard newone2 = new MonsterCard((MonsterCard)allCards.cards[13]);
        handcards.Add(newone2);
        BuildShowedcard(positionInit1, size, newone2);
        CardsMove();
        MonsterCard newone3 = new MonsterCard((MonsterCard)allCards.cards[10]);
        handcards.Add(newone3);
        BuildShowedcard(positionInit1, size, newone3);
        CardsMove();
        MonsterCard newone4 = new MonsterCard((MonsterCard)allCards.cards[2]);
        handcards.Add(newone4);
        BuildShowedcard(positionInit1, size, newone4);
        CardsMove();
    }

    public void GetAward0()
    {
        MonsterCard newone1 = GetBasicCard();
        handcards.Add(newone1);
        BuildShowedcard(positionInit2, size, newone1);
        CardsMove();
    }

    public void ReBegin()
    {
        if(con.isplayerbout)
        {
            selected = false;
            ready = false;
        }
    }

    private void FirstGame()
    {
        if (!showed[0])
        {
            if(!limit)
                LimitAction(0);
            ShowText(0);
            timer++;
            if (timer == 1000)
            {
                showed[0] = true;
                timer = 0;
                limit = false;
                return;
            }
        }
        else if (!showed[1])
        {
            ShowText(1);
            timer++;
            if (timer == 1000)
            {
                showed[1] = true;
                timer = 0;
                return;
            }
        }
        else if (!showed[2])
        {
            if (!limit)
                LimitAction(1);
            ShowText(2);
            for(int i=0;i<4;i++)
            {
                if (!con.isEmpty[i])
                {
                    showed[2] = true;
                    limit = false;
                    return;
                }
            }
        }
        else if (!showed[3])
        {
            ShowText(3);
            for (int i = 0; i < 4; i++)
                if (con.isUsed[i])
                {
                    showed[3] = true;
                    limit = false;
                    return;
                }
        }
        else if (!showed[4])
        {
            ShowText(4);
            timer++;
            if (timer == 1000)
            {
                showed[4] = true;
                timer = 0;
                return;
            }
        }
        else if (!showed[7])
        {
            ShowText(7);
            timer++;
            if (timer == 1000)
            {
                showed[7] = true;
                timer = 0;
                return;
            }
        }
        else if (!showed[5])
        {
            if (!limit)
                DelLimit();
            if (showedCards.Count == 2)
                ShowText(5);
            if (ready)
            {
                showed[5] = true;
                limit = false;
                con.text.SetActive(false);
                return;
            }
        }
        else if (!showed[6])
        {
            if (!con.begin && con.ind == 3&&!limit)
            {
                con.enabled = false;
                ShowText(6);
            }
            timer++;
            if (timer == 1000)
            {
                showed[6] = true;
                timer = 0;
                limit = false;
                con.enabled = true;
                return;
            }
        }
        else if (!showed[8])
        {
            if(!limit)
            {
                ShowText(8);
            }
            if(selected)
            {
                showed[8] = true;
                con.text.SetActive(false);
            }
        }
    }

    private void ShowText(int id)
    {
        con.text.SetActive(true);
        TextMeshProUGUI tmpText = con.text.GetComponent<TextMeshProUGUI>();
        switch (id)
        {
            case 0:
                tmpText.text = "Are you awake?";
                break;
            case 1:
                tmpText.text = "Still remember how to play?";
                break;
            case 2:
                tmpText.text = "Put a squirrel card on it.";
                break;
            case 3:
                tmpText.text = "Sacrifice this poor little fellow.";
                break;
            case 4:
                tmpText.text = "Sometimes one offering might not be enough.";
                break;
            case 7:
                tmpText.text = "Now your creation can come into play.";
                break;
            case 5:
                tmpText.text = "Are you ready? Press this bell.";
                break;
            case 6:
                tmpText.text = "If the plane is completely tilted, you will win.";
                limit = true;
                break;
            case 8:
                tmpText.text = "You can draw a card from the deck or a squirrel card.";
                limit = true;
                break;
        }
    }
    private void HidText()
    {
        con.text.SetActive(false);
        con.enabled = true;
        showed[6] = true;
        limit = false;
    }
    private void LimitAction(int num)
    {
        if(num==0)
            showedCards[0].GetComponent<playhand>().allowDragging = false;
        else
            showedCards[0].GetComponent<playhand>().allowDragging = true;
        showedCards[1].GetComponent<playhand>().allowDragging = false;
        showedCards[2].GetComponent<playhand>().allowDragging = false;
        showedCards[3].GetComponent<playhand>().allowDragging = false;
        limit = true;
    }
    private void DelLimit()
    {
        showedCards[0].GetComponent<playhand>().allowDragging = true;
        showedCards[1].GetComponent<playhand>().allowDragging = true;
        showedCards[2].GetComponent<playhand>().allowDragging = true;
        limit = true;
    }
}
