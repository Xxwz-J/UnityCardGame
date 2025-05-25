using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class LoadEnemy : MonoBehaviour
{
    public TextAsset data1;
    public TextAsset data2;
    public TextAsset data3;
    public TextAsset data4_1;
    public TextAsset data4_2;
    private TextAsset mydata;
    private TextAsset mydata2;
    private gamecontroller con;
    private int index = 0;
    private string[] strings;
    public bool inited;
    private int len;
    private bool over = false;
    private CardStore cards;
    // Start is called before the first frame update
    void Start()
    {
        switch (GlobalData.levelid)
        { case 1:
                mydata = data1;
                break;
            case 2:
                mydata = data2;
                break;
            case 3:
                mydata = data3;
                break;
            case 4:
                mydata = data4_1;
                mydata2 = data4_2;
                break;
        }
        con = GetComponent<gamecontroller>();
        cards = GetComponent<CardStore>();
    }

    public void SetEnemy()
    {
        if (!inited)
        {
            if (!con.isActive)
            {
                strings = mydata.text.Split("\n");
                len = strings.Length;
                index = 0;
            }
            else
            {
                strings = mydata2.text.Split("\n");
                len = strings.Length;
                index = 0;
            }
            inited = true;
        }
        if (index >= len || strings[0]=="")
            over = true;
        if (!over)
        {
            string[] rows = strings[index].Split(",");
            
            for(int i=0;i<4;i++)
            {
                int num = int.Parse(rows[i]);
                Debug.Log("cardid:"+ num);
                if (num == -1)
                    con.allEnemy[i] = null;
                else
                    con.allEnemy[i] = cards.SpecMonster(num);
            }
            if (con.isActive && index == 1)
            {
                con.idofThi = 2;
            }
            index++;
        }
        else
        {
            for (int i = 0; i < 4; i++)
                con.allEnemy[i] = null;
        }
    }
}
