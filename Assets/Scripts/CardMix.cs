using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.Collections.LowLevel.Unsafe;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;
using UnityEngine.UIElements;
using UnityEngine.UI;

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
    public GameObject pushbutton;
    public GameObject cardPrefab;
    public PlayerData playerData;
    public LinkedList<GameObject> mixableCardList=new LinkedList<GameObject>();
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
        setUi();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LayoutPlayerCards()           //չʾ��ҿ���
    {
        int i = 0;
        foreach (var Samenamecard in playerData.playerCards)
        {
            bool mixable = (Samenamecard.Count > 1);
            foreach (Card card in Samenamecard)                 //ȫ��չʾ����ֻ�п��ںϵ�Ϊ�ɼ�
            {
                GameObject newcard = GameObject.Instantiate(cardPrefab, cardpool.transform);
                newcard.GetComponent<CardDisplay>().card = card;
                newcard.SetActive(mixable);
                newcard.AddComponent<choosemix>().cardMix = this;
                if(mixable)mixableCardList.AddLast(newcard);
            }
            if(mixable)
            {
                havesame = true;
            }
            i++;
        }
        if(!havesame)                     //�˴�Ӧ��ת��cardstore����������ȡһ���������͵ĸ��ƿ�
        {
            Debug.Log("��Ŀ���û�п��Ը���ѧ���ںϵĿ�Ƭ");
        }
    }
    public void choosing(GameObject gameObject)     //ѡ����Ҫǿ���Ŀ�
    {
        if (abletochoose) return;
        if(gameObject.GetComponentInParent<GridLayoutGroup>().gameObject==mixpool1)
        {
            mixcard1.SetActive(true);
            Destroy(gameObject);
            mixcard1 = null;
            card1 = null;
            pool1 = false;
            showmixablecard();
            setUi();
            return;
        }
        if (gameObject.GetComponentInParent<GridLayoutGroup>().gameObject == mixpool2)
        {
            mixcard2.SetActive(true);
            Destroy(gameObject);
            mixcard2 = null;
            card2 = null;
            pool2 = false;
            showmixablecard();
            setUi();
            return;
        }
        if (!pool1)                           //����ںϳ�(���ںϳ�1)Ϊ��
        {
            gameObject.SetActive(false);
            GameObject temp= GameObject.Instantiate(cardPrefab,mixpool1.transform);
            temp.GetComponent<CardDisplay>().card = gameObject.GetComponent<CardDisplay>().card;
            temp.AddComponent<choosemix>().cardMix = this;
            preid =gameObject.GetComponent<CardDisplay>().card.cardID;
            pool1 = true;
            mixcard1 = gameObject;
            card1 = temp;
            hidediffrent();
            setUi();
            return;
        }
        else if(!pool2)                      //�Ҳ��ںϳ�(���ںϳ�2)Ϊ��
        {
            if(preid!=gameObject.GetComponent<CardDisplay>().card.cardID ) { return; }
            gameObject.SetActive(false);
            GameObject temp = GameObject.Instantiate(cardPrefab, mixpool2.transform);
            temp.GetComponent<CardDisplay>().card = gameObject.GetComponent<CardDisplay>().card;
            temp.AddComponent<choosemix>().cardMix = this;
            pool2 = true;
            mixcard2 = gameObject;
            card2 = temp;
            setUi();
            return;
        }
        return;
    }

    public void cutandmix()
    {
        if(!pool1 || !pool2) return;
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
           // a = temp1.countstamp();
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
        if (mixedcard.stamps[1] != Stamp.NullStamp) mixedcard.carved = true;
        GameObject newcard=GameObject.Instantiate(cardPrefab,mixedpool.transform);
        newcard.GetComponent<CardDisplay>().card = mixedcard;
        GameObject getnewcard = GameObject.Instantiate(cardPrefab, cardpool.transform);
        getnewcard.GetComponent<CardDisplay>().card = mixedcard;
        getnewcard.SetActive(false);
        DestroyImmediate(mixcard1);
        DestroyImmediate(mixcard2);
        doneIt();
    }

    public void doneIt()                        //�����ںϰ�ť
    {
        done = true;
        string path = Application.dataPath + "/Datas/playerdata.csv";
        List<string> datas = new List<string>();
        foreach (Transform child in cardpool.transform)
        {
            if (child.gameObject == null) continue;
            var monster = child.GetComponent<CardDisplay>().card as MonsterCard;
            //Debug.Log(monster.carved);
            datas.Add("card," + monster.cardID.ToString() + "," + monster.attack.ToString() + "," +
                monster.healthmax.ToString() + "," + monster.stamps[0].ToString() + "," +
                monster.stamps[1].ToString() + "," + monster.stamps[2].ToString() + "," + monster.carved);
        }
        File.WriteAllLines(path, datas);
    }

    public void hidediffrent()              //���ں����п������ؿ��������в�ͬ�Ŀ�
    {
        foreach (Transform child in cardpool.transform)
        {
            if(child.GetComponent<CardDisplay>().card.cardID != preid) 
                child.gameObject.SetActive(false);
        }
    }

    public void showmixablecard()
    {
        if (pool1 || pool2 == true) return;
        foreach (var hidcard in mixableCardList)
        {
            hidcard.SetActive(true);
        }
    }

    public void setUi()
    {
        if(pool1&&pool2)pushbutton.SetActive(true);
        else pushbutton.SetActive(false);
        if(done)
        {
            mixpool1.SetActive(false);
            mixpool2.SetActive(false);
            mixedpool.SetActive(true);
        }
    }
}
