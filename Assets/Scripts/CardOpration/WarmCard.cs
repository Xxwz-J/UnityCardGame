using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEditor.Animations;


public class WarmCard : MonoBehaviour
{
    public GameObject cardpool;
    public GameObject onFire;
    public GameObject FireObject;
    public GameObject cardPrefab;
    public PlayerData playerData;
    public List<GameObject> cardObjects = new List<GameObject>();
    public GameObject chosenCard;
    public GameObject onfirecard;
    public GameObject firebutton;
    public GameObject leavebutton;
    public Card card;

    public GameObject hintObject;
    public Text hinttext;
    //int count =0;
    bool hintover = false;
    bool abletochoose=false;
    bool abletoroast = false;
    int roastTime = 0;
    int hintcount = 0;

    string[] firehint = new string[5];
    // Start is called before the first frame update
    void Start()
    {
        firehint[0] = "你看到森林中有一个火堆";
        firehint[1] = "火堆周围有一些饥肠辘辘的幸存者";
        firehint[2] = "其中一个幸存者对你说";
        firehint[3] = "“让你的造物过来烤烤火吧”";
        firehint[4] = "“这会增强他们的力量”";
        showHint();
        LayoutPlayerCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showHint()
    {
        if (hintcount > 4)
        {
            hintObject.SetActive(false);
            hintover= true;
            SetUI();
            return;
        }
        hinttext.text = firehint[hintcount];
        hintcount++;
        SetUI();
        return;
    }
    public void LayoutPlayerCards()           //展示玩家卡组
    {
        foreach(var Samenamecard in playerData.playerCards)
        {
            foreach(Card card in Samenamecard)
            {
                //count++;
                //MonsterCard monster = card as MonsterCard;
                //Debug.Log(monster.attack.ToString()+" "+monster.healthmax.ToString()+" "+count.ToString());
                GameObject newcard = GameObject.Instantiate(cardPrefab, cardpool.transform);
                newcard.GetComponent<CardDisplay>().card = card;
                newcard.AddComponent<ChooseCard>().warmCard = this;
                cardObjects.Add(newcard);
            }
        }
    }
    public void choosing(GameObject gameObject)     //选择想要强化的卡
    {
        if (abletochoose) return;
        if (gameObject.GetComponentInParent<GridLayoutGroup>().gameObject == onFire)
        {
            chosenCard.SetActive(true);
            Destroy(onfirecard);
            onfirecard = null;
            return;
        }
        if (chosenCard!=null)
        {
            chosenCard.SetActive(true);
        }
        gameObject.SetActive(false);
        chosenCard = gameObject;
        
        if(onfirecard!=null)
        {
            Destroy(onfirecard);
        }
        GameObject temp = GameObject.Instantiate(cardPrefab, onFire.transform);
        onfirecard = temp;
        onfirecard.AddComponent<ChooseCard>().warmCard = this;
        temp.GetComponent<CardDisplay>().card = chosenCard.GetComponent<CardDisplay>().card;
    }

    public void doneIt()                        //卡被吃掉或者玩家主动结束强化，锁死烤火按钮
    {
        abletoroast = true;
        SetUI();
        string path = Path.Combine(Application.persistentDataPath, "playerdata.csv");
        List<string> datas = new List<string>();
        foreach (Transform child in cardpool.transform)
        {
            if (child.gameObject == null) continue;
            var monster = child.GetComponent<CardDisplay>().card as MonsterCard;
            datas.Add("card," + monster.cardID.ToString() + "," + monster.attack.ToString() + "," +
                monster.healthmax.ToString() + "," + monster.stamps[0].ToString() + "," +
                monster.stamps[1].ToString() + "," + monster.stamps[2].ToString() + "," + monster.carved);
        }
        File.WriteAllLines(path, datas);
    }

    public void roastCard()                  //第一次强化必定成功，第二次百分之五十，第三次卡必定被吃掉
    {
        if (abletoroast) return;
        if (onfirecard == null) return;
        if (roastTime == 0)                    //锁死onfire卡槽，禁用choosing函数
        {
            abletochoose = true;
            SetUI();

            hintObject.SetActive(true);
            hinttext.text = "一个幸存者摸着腰上的刀";

            upgrade();
            roastTime++;
            return;
        }
        if (roastTime == 1 && Random.Range(0, 2) == 0)
        {
            hintObject.SetActive(true);
            hinttext.text = "另一个幸存者舔了舔嘴唇";
            upgrade();
            roastTime++;
            return;
        }
        else
        {
            hintObject.SetActive(true);
            hinttext.text = "幸存者们一拥而上，吃掉了" + chosenCard.GetComponent<CardDisplay>().card.cardName;
            //Debug.Log("幸存者们一拥而上，吃掉了" + chosenCard.GetComponent<CardDisplay>().card.cardName);
            DestroyImmediate(onfirecard);
            DestroyImmediate(chosenCard);
            abletoroast = true;
            SetUI();
            doneIt();
        }
    }
    public void upgrade()
    {
        if(onfirecard.GetComponent<CardDisplay>().card is MonsterCard)
        {
            var monster= onfirecard.GetComponent<CardDisplay>().card as MonsterCard;
            monster.attack+=1;
            //Debug.Log(monster.cardName + monster.attack.ToString() + monster.healthmax.ToString());
            onfirecard.GetComponent<CardDisplay>().card = monster;
            Destroy(onfirecard);                           //以下三行更新强化卡面，未测试
            GameObject temp = GameObject.Instantiate(cardPrefab, onFire.transform);
            temp.GetComponent<CardDisplay>().card = monster;
            onfirecard = temp;
            chosenCard.GetComponent<CardDisplay>().card= monster;
        }
    }

    public void SetUI()
    {
        if(!hintover)
        {
            FireObject.SetActive(false);
            cardpool.SetActive(false);
            firebutton.SetActive(false);
            leavebutton.SetActive(false);
            return;
        }
        if(abletochoose)
        {
            foreach (Transform child in cardpool.transform)
            { 
                child.gameObject.SetActive(false);
            }
        }
        if(abletoroast)
        {
            firebutton.SetActive(false);
            return;
        }
        FireObject.SetActive(true);
        cardpool.SetActive(true);
        firebutton.SetActive(true);
        leavebutton.SetActive(true);
    }
}
