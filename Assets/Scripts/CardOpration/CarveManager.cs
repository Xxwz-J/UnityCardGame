using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CarveManager : MonoBehaviour
{
    public GameObject GScardPrefab;
    public GameObject BScardPrefab;

    public GameObject GScardPool;
    public GameObject BScardPool;

    public GameObject GScardObject;
    public GameObject BScardObject;

    public Card GScard;
    public Card BScard;

    public GameObject Button;
    public PlayerData PlayerData;

    public GameObject hintObject;
    public Text hinttext;
    public GameObject pool1;
    public GameObject pool2;
    public GameObject pool3;

    public bool[] select = { false,false };// 0-GS 1-BS

    string[] carvehint = new string[5];
    int hintcount = 0;
    bool hintover = false;
    // Start is called before the first frame update
    void Start()
    {
        carvehint[0] = "你走进一座建筑物";
        carvehint[1] = "你惊讶地发现这是一座神殿";
        carvehint[2] = "神像里传出极具蛊惑力的低语";
        carvehint[3] = "“献祭放入左侧的卡牌”";
        carvehint[4] = "“放入右侧的卡牌将获得新生”";
        showHint();
        if (Button != null) Button.SetActive(false);
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
            hintover = true;
            SetUI();
            return;
        }
        hinttext.text = carvehint[hintcount];
        hintcount++;
        SetUI();
        return;
    }
    public void ClearGS()
    {
        Destroy(GScardObject);
        GScard = null;
        Button.SetActive(false);
    }
    public void ClearBS()
    {
        Destroy(BScardObject);
        BScard = null;
        Button.SetActive(false);
    }
    
    public void Carve()
    {
        PlayerData.playerCards[BScard.cardID].Remove(BScard);
        PlayerData.playerCards[GScard.cardID].Remove(GScard);

        MonsterCard bsCard = (MonsterCard)BScard;
        MonsterCard gsCard = (MonsterCard)GScard;
        foreach (var bstamp in bsCard.stamps)
        {
            if (bstamp == Stamp.NullStamp) break;
            for (int i = 0; i < 3; i++)
            {
                if (gsCard.stamps[i] == bstamp) break;
                if (gsCard.stamps[i] == Stamp.NullStamp)
                {
                    gsCard.stamps[i] = bstamp;
                    break;
                }
            }
        }
        gsCard.carved = true;
        PlayerData.playerCards[GScard.cardID].AddLast(gsCard);


        PlayerData.SavePlayerData();
        ClearGS();
        ClearBS();
        Button.SetActive(false);
        SceneManager.LoadScene("PlayScenes");
    }
    public void SetUI()
    {
        if (!hintover)
        {
            pool1.SetActive(false);
            pool2.SetActive(false);
            Button.SetActive(false);
            foreach (Transform child in pool3.transform)
            {
                child.gameObject.SetActive(false);
            }
            return;
        }
        pool1.SetActive(true);
        pool2.SetActive(true);
        foreach (Transform child in pool3.transform)
        {
            child.gameObject.SetActive(true);
        }
        return;
    }
}
