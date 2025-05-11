using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStore : MonoBehaviour
{
    public TextAsset cardData;
    public List<Card> cards = new List<Card>();




    // Start is called before the first frame update
    void Start()
    {
        //LoadCardData();
    }

    // Update is called once per frame
    void Update()
    {
        
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

                Debug.Log("∂¡»°µΩπ÷ ﬁø®:"+monsterCard .cardName);
            }
        }
    }
        public Card RandomCard()
    {
        Card card = cards[UnityEngine.Random.Range(1, cards.Count-3)];
        return card;
    }

    
}
