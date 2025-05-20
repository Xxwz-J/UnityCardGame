using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public bool[] select = { false,false };// 0-GS 1-BS
    // Start is called before the first frame update
    void Start()
    {
        if (Button != null) Button.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
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

}
