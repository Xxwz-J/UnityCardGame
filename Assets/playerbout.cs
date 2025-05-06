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
    public List<GameObject> cards;
    public List<GameObject> handcards;
    public int num;
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
    // Start is called before the first frame update
    void Start()
    {
        inited = false;
        ready = false;
        selected = true;
        con = controller.GetComponent<gamecontroller>();
        GetInitCards();
        InitTarget();
        ShowCards();
    }
    private void GetInitCards()
    {
        num = 4;
        handcards = new List<GameObject>(4);
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
        GameObject square = new GameObject("UI Square");

        square.transform.SetParent(FindObjectOfType<Canvas>().transform);

        RectTransform rectTransform = square.AddComponent<RectTransform>();
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
    public void OnClick1()
    {
        if (con.isplayerbout && !selected)
        {
            GameObject newcard = cards[0];
            cards.Remove(newcard);
            handcards.Add(newcard);
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
            GameObject newcard = new GameObject();
            handcards.Add(newcard);
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
