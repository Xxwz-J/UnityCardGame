using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.Collections.LowLevel.Unsafe;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;
using UnityEngine.UIElements;

public class CardMix : MonoBehaviour
{
    public GameObject cardpool;
    public GameObject mixpool1;
    public GameObject mixpool2;
    public GameObject mixedpool;
    public GameObject mixcard1;
    public GameObject mixcard2;
    public GameObject card1;
    public GameObject card2;
    public GameObject cardPrefab;
    public PlayerData playerData;
    public LinkedList<GameObject>[] mixableCardList;
    bool havesame=false;
    bool abletochoose = false;
    bool pool1=false;
    bool pool2=false;
    bool done = false;
    int preid = -1;
    // Start is called before the first frame update
    void Start()
    {
        LayoutPlayerCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LayoutPlayerCards()           //展示玩家卡组
    {
        mixableCardList = new LinkedList<GameObject>[playerData.playerCards.Length];
        int i = 0;
        foreach (var Samenamecard in playerData.playerCards)
        {
            LinkedList<GameObject> list = new LinkedList<GameObject>();
            bool mixable = (Samenamecard.Count > 1);
            foreach (Card card in Samenamecard)                 //全部展示，但只有可融合的为可见
            {
                GameObject newcard = GameObject.Instantiate(cardPrefab, cardpool.transform);
                newcard.GetComponent<CardDisplay>().card = card;
                newcard.SetActive(mixable);
                newcard.AddComponent<choosemix>().cardMix = this;
                list.AddLast(newcard);
            }
            if(mixable)
            {
                mixableCardList[i] = list;
                havesame = true;
            }
            i++;
        }
        if(!havesame)
        {
            Debug.Log("你的卡组没有可以给菌学家融合的卡片");
        }
    }
    public void choosing(GameObject gameObject)     //选择想要强化的卡
    {
        if (abletochoose) return;
        if(!pool1)                           //左侧融合池(即融合池1)为空
        {
            gameObject.SetActive(false);
            GameObject temp= GameObject.Instantiate(cardPrefab,mixpool1.transform);
            temp.GetComponent<CardDisplay>().card = gameObject.GetComponent<CardDisplay>().card;
            preid=gameObject.GetComponent<CardDisplay>().card.cardID;
            pool1 = true;
            mixcard1 = gameObject;
            card1 = temp;
            return;
        }
        else if(!pool2)                      //右侧融合池(即融合池2)为空
        {
            if(preid!=gameObject.GetComponent<CardDisplay>().card.cardID ) { return; }
            gameObject.SetActive(false);
            GameObject temp = GameObject.Instantiate(cardPrefab, mixpool2.transform);
            temp.GetComponent<CardDisplay>().card = gameObject.GetComponent<CardDisplay>().card;
            pool2 = true;
            mixcard2 = gameObject;
            card2 = temp;
            return;
        }
        return;
    }

    public void cutandmix()
    {
        if(!pool1 || !pool2) return;
        Debug.Log("0");
        if (done) return;
        card1.SetActive(false);
        card2.SetActive(false);
        MonsterCard temp1 = mixcard1.GetComponent<CardDisplay>().card as MonsterCard;
        MonsterCard temp2 = mixcard2.GetComponent<CardDisplay>().card as MonsterCard;
        Stamp[] total = new Stamp[3];
        int a = 0;
        if (temp1.stamps[0]!=Stamp.NullStamp)
        {
            total=temp1.stamps;
            a = temp1.countstamp();
        }
        if (temp2.stamps[0]!=Stamp.NullStamp)
        {
            foreach (Stamp stamp in temp2.stamps)
            {
                if (stamp == Stamp.NullStamp) continue;
                if (stamp == total[0]) continue;
                if (stamp == total[1]) continue;
                if (stamp == total[2]) continue;
                total[a] = stamp;
            }
        }
        MonsterCard mixedcard = new MonsterCard(preid, temp1.cardName, temp1.attack + temp2.attack,
            temp1.healthmax + temp2.healthmax, temp1.sacrifice, total);
        GameObject newcard=GameObject.Instantiate(cardPrefab,mixedpool.transform);
        newcard.GetComponent<CardDisplay>().card = mixedcard;
        GameObject getnewcard = GameObject.Instantiate(cardPrefab, cardpool.transform);
        getnewcard.GetComponent<CardDisplay>().card = mixedcard;
        getnewcard.SetActive(false);
        DestroyImmediate(mixcard1);
        DestroyImmediate(mixcard2);
        doneIt();
    }

    public void doneIt()                        //卡被吃掉或者玩家主动结束强化，锁死融合按钮
    {
        done = true;
        string path = Application.dataPath + "/Datas/playerdata.csv";
        List<string> datas = new List<string>();
        foreach (Transform child in cardpool.transform)
        {
            if (child.gameObject == null) continue;
            var monster = child.GetComponent<CardDisplay>().card as MonsterCard;
            datas.Add("card," + monster.cardID.ToString() + "," + monster.attack.ToString() + "," +
                monster.healthmax.ToString() + "," + monster.stamps[0].ToString() + "," +
                monster.stamps[1].ToString() + "," + monster.stamps[2].ToString() + ",FALSE");
        }
        File.WriteAllLines(path, datas);
    }
}
