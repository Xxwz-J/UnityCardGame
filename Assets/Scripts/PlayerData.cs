using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.Collections.LowLevel.Unsafe;

public class PlayerData : MonoBehaviour
{
    public TextAsset playerData;
    public CardStore CardStore;

    public int[] playerCards;
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
        playerCards = new int[CardStore.cards .Count];
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
                int quantity = int.Parse(rowArray[2]);
                playerCards[id]= quantity;

            }
        }
    }
    public void SavePlayerData() {
        string path = Application.dataPath + "/Datas/playerdata.csv";
        List<string> datas = new List<string>();
        for (int i = 0; i < playerCards.Length; i++) {
            if(playerCards[i]>0) datas.Add("card," + i.ToString()+","+playerCards[i].ToString());
        }
        File.WriteAllLines(path, datas);
    }
}
