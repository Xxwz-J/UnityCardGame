using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.Collections.LowLevel.Unsafe;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CardMix : MonoBehaviour
{
    public GameObject cardpool;
    public GameObject mixpool1;
    public GameObject mixpool2;
    public GameObject mixedpool;
    public GameObject left;
    public GameObject right;
    public GameObject middle;
    public GameObject mixcard1;
    public GameObject mixcard2;
    public GameObject card1;
    public GameObject card2;
    public GameObject pushbutton;
    public GameObject cardPrefab;
    public PlayerData playerData;
    public LinkedList<GameObject> mixableCardList=new LinkedList<GameObject>();

    public GameObject hintObject;
    public Text hinttext;

    bool havesame=false;
    bool abletochoose = false;
    bool pool1=false;
    bool pool2=false;
    bool done = false;
    bool hintover = false;
    int preid = -1;
    int hintcount = 0;
    bool jump = false;

    string[] mixhint = new string[5];
    // Start is called before the first frame update
    void Start()
    {
        mixhint[0] = "你来到一处林中空地";
        mixhint[1] = "地上有一朵双生蘑菇";
        mixhint[2] = "菌学家在研究这朵蘑菇";
        mixhint[3] = "“如果你有像双生蘑菇一样的造物”";
        mixhint[4] = "“也许我能给你展示一下我的研究成果”";
        showHint();
        LayoutPlayerCards();
        setUi();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void showHint()
    {
        if (hintcount > 4)
        {
            if(jump)
            {
                SceneManager.LoadScene("cardstore");
            }
            if (!havesame)                     //卡组中没有同名卡
            {
                hinttext.text="你没有可以给菌学家合成的卡牌";
                jump = true;
            }
            if(!jump)
            {
                hintObject.SetActive(false);
                hintover = true;
                setUi();
            }
            return;
        }
        hinttext.text = mixhint[hintcount];
        hintcount++;
        setUi();
        return;
    }
    public void LayoutPlayerCards()           //展示玩家全部卡组，不能融合的被隐藏
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
        //if(!havesame)                     //卡组中没有同名卡
        //{
        //    Debug.Log("你没有可以给菌学家合成的卡牌");
        //    SceneManager.LoadScene("cardstore");
        //}
    }
    public void choosing(GameObject gameObject)     //选择要融合的卡牌
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
        mixedcard.carved = temp1.carved || temp2.carved;
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
        setUi();
        string path = Path.Combine(Application.persistentDataPath, "playerdata.csv");
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
        if(!hintover)
        {
            pushbutton.SetActive(false);
            left.SetActive(false);
            right.SetActive(false);
            middle.SetActive(false);
            foreach (Transform child in cardpool.transform)
            {
                child.gameObject.SetActive(false);
            }
            return;
        }
        showmixablecard();
        if (pool1&&pool2)pushbutton.SetActive(true);
        else pushbutton.SetActive(false);
        if (done)
        {
            left.SetActive(false);
            right.SetActive(false);
            middle.SetActive(true);
        }
        else
        {
            left.SetActive(true);
            right.SetActive(true);
            middle.SetActive(false);
        }
    }
}
