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
            for (var card = playerData.playerCards[i].First;card!=null; card = card.Next) {
                GameObject newcard=Instantiate(cardPrefab, libraryPanel.transform);
                newcard.GetComponent<CardDisplay>().card = card.Value;
            }
        }
    }
}
