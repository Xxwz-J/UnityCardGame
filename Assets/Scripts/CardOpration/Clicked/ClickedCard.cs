using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;



public class ClickedCard : MonoBehaviour,IPointerDownHandler
{
    private PlayerData PlayerData;
    private CardStore CardStore;
    private OpenPackage OpenPackage;
    // Start is called before the first frame update
    void Start()
    {
        PlayerData = GameObject.Find("PlayerData").GetComponent <PlayerData>(); 
        CardStore = GameObject .Find("CardStore").GetComponent <CardStore>();
        OpenPackage= GameObject.Find("CardStore").GetComponent<OpenPackage>();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerDown(PointerEventData pointerEventData) {
        int id = this.GetComponent<CardDisplay>().card.cardID;
        PlayerData.playerCards[id].AddLast(new MonsterCard((MonsterCard)CardStore.cards[id]));
        PlayerData.SavePlayerData();
        OpenPackage.ClearPool();
    }

}
