using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.UI;

public class WarmCard : MonoBehaviour
{
    public GameObject cardpool;
    public GameObject onFire;
    public GameObject cardPrefab;
    public PlayerData playerData;
    public List<GameObject> cardObjects = new List<GameObject>();
    public GameObject chosenCard;
    public GameObject onfirecard;
    public Card card;
    //int count =0;
    bool abletochoose=false;
    bool abletoroast = false;
    int roastTime = 0;
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
        string path = Application.dataPath + "/Datas/playerdata.csv";
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
            upgrade();
            roastTime++;
            return;
        }
        if (roastTime == 1 && Random.Range(0, 2) == 0)
        {
            upgrade();
            roastTime++;
            return;
        }
        else
        {
            Debug.Log("幸存者们一拥而上，吃掉了" + chosenCard.GetComponent<CardDisplay>().card.cardName);
            DestroyImmediate(onfirecard);
            DestroyImmediate(chosenCard);
            abletoroast = true;
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
}
