using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ClickEvent : MonoBehaviour, IPointerClickHandler
{
    public GameObject newCard;
    public GetAward con;
    public int idofCard;
    public int id;
    public bool isShow = false;
    private MonsterCard card;
    private GameObject showedCard;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!isShow)
        {
            Debug.Log("UI元素被点击: " + gameObject.name);
            // 这里添加你的点击处理逻辑
            ShowCard();
        }
        else
        {
            AddCard();
            CardMove();
        }
    }
    private void ShowCard()
    {
        card = new MonsterCard((MonsterCard)GetComponent<CardStore>().cards[idofCard]);

        GameObject square = Instantiate(newCard);
        square.GetComponent<CardDisplay>().card = card;

        square.transform.SetParent(FindObjectOfType<Canvas>().transform);

        RectTransform rectTransform = square.GetComponent<RectTransform>();

        rectTransform.anchoredPosition = new Vector3(200 * id - 200, 50,0);
        rectTransform.sizeDelta = new Vector2(120, 160);
        square.AddComponent<MoveE>().enabled = false;

        showedCard = square;
        ClickEvent ce=square.AddComponent<ClickEvent>();
        ce.con = con;
        ce.isShow = true;
        ce.card = card;
        ce.showedCard = showedCard;
        ce.enabled = false;

        con.newcard[id] = square;
        Destroy(gameObject);
    }
    private void AddCard()
    {
        Debug.Log("UI元素被点击: " + gameObject.name + gameObject.name);
        PlayerData p = GetComponent<PlayerData>();
        p.playerCards[card.cardID].AddLast(card);
        //返回地图
    }
    private void CardMove()
    {
        MoveE me = showedCard.GetComponent<MoveE>();
        me.position = new Vector3(355, -156, 0);
        me.enabled = true;
        con.newcard[id] = null;
        con.selected = true;
    }
}
