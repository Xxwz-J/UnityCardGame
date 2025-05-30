using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class OpenPackage : MonoBehaviour
{
    public GameObject cardPrefab;
    public GameObject cardPool;

    CardStore cardStore;
    public List<GameObject> cardObjects = new List<GameObject>();
    public PlayerData playerData;
    public bool opend = false;

    // Start is called before the first frame update
    void Start()
    {
        opend = false;
        cardStore = GetComponent<CardStore>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GetCards()
    {   if (opend) return;
        opend = true;
        ClearPool();
        int[] uniqueCards = new int[3] { -1, -1, -1 };
        uniqueCards[0] = UnityEngine.Random.Range(1, cardStore.cards.Count - 3);
        while (uniqueCards[1] < 0 | uniqueCards[0] == uniqueCards[1]) uniqueCards[1] = UnityEngine.Random.Range(1, cardStore.cards.Count - 3);
        while (uniqueCards[2] < 0 | uniqueCards[1] == uniqueCards[2]) uniqueCards[2] = UnityEngine.Random.Range(1, cardStore.cards.Count - 3);
        for (int i = 0; i < 3; i++)
        {
            GameObject card = GameObject.Instantiate(cardPrefab,cardPool.transform); //生成的card放到cardPool中
            card.GetComponent<CardDisplay>().card = cardStore.SpecCard(uniqueCards[i]);
            cardObjects.Add(card);
        }
    }

    public void ClearPool() { 
        foreach(var card in cardObjects)
        {
            Destroy(card);
        }
    cardObjects.Clear();
    }


}
