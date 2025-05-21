using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LibraryManager : MonoBehaviour
{
    public GameObject libraryPanel;
    public GameObject library;
    public GameObject cardPrefab;

    
    public PlayerData playerData;
    public CardStore cardStore;

    public List<GameObject> cardObjects = new List<GameObject>();

    private CarveManager CarveManager;
    // Start is called before the first frame update
    void Start()
    {
        UpdateLibrary();
        //CarveManager = GameObject.Find("CarveManager").GetComponent<CarveManager>();
        CarveManager = GameObject.Find("CarveManager")?.GetComponent<CarveManager>() ?? null;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateLibrary() {
        ClearPool();
        for (int i = 0; i < playerData.playerCards.Length; i++) {
            for (var card = playerData.playerCards[i].First;card!=null; card = card.Next) {
                GameObject newcard=Instantiate(cardPrefab, libraryPanel.transform);
                newcard.GetComponent<CardDisplay>().card = card.Value;
                cardObjects.Add(newcard);
            }
        }
    }

    public void UpdateBeSacrificedLibrary()
    {
        CarveManager.select[0] = false;
        CarveManager.select[1] = true;
        CarveManager.ClearBS();
        ClearPool();
        library.SetActive(true);
        for (int i = 0; i < playerData.playerCards.Length; i++)
        {
            for (var card = playerData.playerCards[i].First; card != null; card = card.Next)
            {
                if(!card.Value.CanBeSacrificed(CarveManager.GScard)) continue;
                GameObject newcard = Instantiate(cardPrefab, libraryPanel.transform);
                newcard.GetComponent<CardDisplay>().card = card.Value;
                cardObjects.Add(newcard);
            }
        }
    }
    public void UpdateGetSacrificeLibrary()
    {
        CarveManager.select[1] = false;
        CarveManager.select[0] = true;
        CarveManager.ClearGS();
        ClearPool();
        library.SetActive(true);
        for (int i = 0; i < playerData.playerCards.Length; i++)
        {
            for (var card = playerData.playerCards[i].First; card != null; card = card.Next)
            {
                if (!card.Value.CanGetSacrifice(CarveManager.BScard)) continue;

                GameObject newcard = Instantiate(cardPrefab, libraryPanel.transform);
                newcard.GetComponent<CardDisplay>().card = card.Value;
                cardObjects.Add(newcard);
            }
        }
    }
    public void ClearPool()
    {
        foreach (var card in cardObjects)
        {
            Destroy(card);
        }
        cardObjects.Clear();
    }
}
