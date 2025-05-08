using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.Collections.LowLevel.Unsafe;

public class PlayerData : MonoBehaviour
{
    public TextAsset playerData;
    public CardStore CardStore;

    //public int[] playerCards;
    public LinkedList<Card>[] playerCards;
    // Start is called before the first frame update
    void Start()
    {
        CardStore.LoadCardData();
        LoadPlayerData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadPlayerData() {
        string[] dataRow = playerData.text.Split("\n");
        //playerCards = new int[CardStore.cards .Count];
        playerCards = new LinkedList<Card>[CardStore.cards.Count];
        for (int i = 0; i < playerCards.Length; i++)
        {
            playerCards[i] = new LinkedList<Card>(); 
        }
        foreach (string row in dataRow)
        {
            string[] rowArray = row.Split(',');
            if (rowArray[0] == "#")
            {
                continue;
            }
            else if (rowArray[0] == "card")
            {
                int id = int.Parse(rowArray[1]);
                var card =playerCards[id].AddLast(new MonsterCard((MonsterCard)CardStore.cards[id]));
                card.Value.Update(row);
            }
        }
    }
    public void SavePlayerData() {
        string path = Application.dataPath + "/Datas/playerdata.csv";
        List<string> datas = new List<string>();
        for (int i = 0; i < playerCards.Length; i++) {
            int count = playerCards[i].Count;
            LinkedListNode<Card> card=playerCards[i].First;
            while (count > 0) { 
                datas.Add("card," + i.ToString() + "," + card.Value.ToString()); 
                count--;
                card = card.Next;
            }
        }
        File.WriteAllLines(path, datas);
    }
}
