using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.Collections.LowLevel.Unsafe;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class CardStore : MonoBehaviour
{
    public TextAsset cardData;
    public List<Card> cards = new List<Card>();

    public GameObject hintObject;
    public Text hinttext;

    string[] storehint = new string[2];

    int hintcount = 0;
    bool hintover = false;
    // Start is called before the first frame update
    void Start()
    {
        storehint[0] = "你的面前出现了一个破旧的小屋";
        storehint[1] = "小屋里的木桌上摆着一个包裹";
        showHint();
        //LoadCardData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void showHint()
    {
        if (hintcount > 1)
        {
            hintObject.SetActive(false);
            hintover = true;
            SetUI();
            return;
        }
        hinttext.text = storehint[hintcount];
        hintcount++;
        SetUI();
        return;
    }
    public void LoadCardData()
    {
        string[] dataRow = cardData.text.Split("\n");
        foreach (string row in dataRow) {
            string[] rowArray = row.Split(',');
            if (rowArray[0]=="#")
            {
                continue;
            }
            else if (rowArray[0] == "monster")
            {
                int id = int.Parse(rowArray[1]);
                string name = rowArray[2];
                int attack = int.Parse(rowArray[3]);
                int health = int.Parse(rowArray[4]);
                int sacrifice = int.Parse(rowArray[5]);
                Stamp stamp = (Stamp)Enum.Parse(typeof(Stamp), rowArray[6]);

                MonsterCard monsterCard = new MonsterCard(id,name, attack, health, sacrifice,stamp);
                cards.Add(monsterCard);

                Debug.Log("读取到怪兽卡:"+monsterCard .cardName);
            }
        }
    }
        public Card RandomCard()
    {
        Card card = cards[UnityEngine.Random.Range(1, cards.Count-3)];
        return card;
    }

    public void SetUI()
    {
        if (hintover) hintObject.SetActive(false);
    }
}
