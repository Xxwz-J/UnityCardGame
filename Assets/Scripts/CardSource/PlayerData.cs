using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;

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
        //LoadInitialData();
        LoadPlayerData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadPlayerData() {
        // 修正路径
        string path = Path.Combine(Application.persistentDataPath, "playerdata.csv");

        // 确保目录存在
        Directory.CreateDirectory(Path.GetDirectoryName(path));

        string[] dataRow = File.ReadAllLines(path); ;
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
    //public void SavePlayerData() {
    //    string path = Application.dataPath + "/Datas/playerdata.csv";
    //    List<string> datas = new List<string>();
    //    for (int i = 0; i < playerCards.Length; i++) {
    //        int count = playerCards[i].Count;
    //        LinkedListNode<Card> card=playerCards[i].First;
    //        while (count > 0) { 
    //            datas.Add("card," + i.ToString() + "," + card.Value.ToString()); 
    //            count--;
    //            card = card.Next;
    //        }
    //    }
    //    File.WriteAllLines(path, datas);
    //}

    public void SavePlayerData()
    {
        // 修正路径
        string path = Path.Combine(Application.persistentDataPath, "playerdata.csv");

        // 确保目录存在
        Directory.CreateDirectory(Path.GetDirectoryName(path));

        List<string> datas = new List<string>();
        for (int i = 0; i < playerCards.Length; i++)
        {
            int count = playerCards[i].Count;
            LinkedListNode<Card> card = playerCards[i].First;
            while (count > 0)
            {
                datas.Add("card," + i.ToString() + "," + card.Value.ToString());
                count--;
                card = card.Next;
            }
        }

        File.WriteAllLines(path, datas);

        // 仅在编辑器下刷新
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }

    public void LoadInitialData() {
        string path = Application.dataPath + "/Assets/Datas/InitialCards.csv";
       // foreach (var list in playerCards)
       // {
       //    if (list!=null)
      //     list.Clear();
      //  }
        
        string[] dataRow = File.ReadAllLines(path); ;
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
                var card = playerCards[id].AddLast(new MonsterCard((MonsterCard)CardStore.cards[id]));
                card.Value.Update(row);
            }
        }
        SavePlayerData();
    }
}
