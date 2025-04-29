using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LibraryManager : MonoBehaviour
{
    public GameObject libraryPanel;
    public GameObject cardPrefab;
   
    
    public PlayerData playerData;
    public CardStore cardStore;

    // Start is called before the first frame update
    void Start()
    {
        UpdateLibrary();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateLibrary() {
        for (int i = 0; i < playerData.playerCards.Length; i++) {

            for (int j = playerData.playerCards[i]; j > 0; j--) {
                GameObject newcard=Instantiate(cardPrefab, libraryPanel.transform);
                newcard.GetComponent<CardDisplay>().card = cardStore.cards[i];
            }
        }
    }
}
