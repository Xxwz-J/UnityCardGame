using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GetAward : MonoBehaviour
{
    public GameObject[] card = new GameObject[3];
    public GameObject[] newcard = new GameObject[3];
    public GameObject playerCards;
    public bool[] ready = new bool[3];
    public int[] ids = new int[3];
    public bool selected;
    // Start is called before the first frame update
    void Start()
    {
        System.Random random = new System.Random();
        HashSet<int> randomNumbers = new HashSet<int>();

        while (randomNumbers.Count < 3)
        {
            int randomNumber = random.Next(1, 5); // 生成1到11的随机数
            randomNumbers.Add(randomNumber); // 将随机数添加到集合中
        }
        int i = 0;
        foreach(var id in randomNumbers)
        {
            newcard[i] = null;
            ClickEvent c = card[i].GetComponent<ClickEvent>();
            c.idofCard = id;
            c.con = this;
            c.id = i;
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (newcard[0]!=null&& newcard[1] != null&& newcard[2] != null)
        {
            for (int i = 0; i < 3; i++)
                newcard[i].GetComponent<ClickEvent>().enabled = true;
        }
        else if(selected)
        {
            for (int i = 0; i < 3; i++)
                if (newcard[i] != null)
                    newcard[i].GetComponent<ClickEvent>().enabled = false;
        }
    }
}
