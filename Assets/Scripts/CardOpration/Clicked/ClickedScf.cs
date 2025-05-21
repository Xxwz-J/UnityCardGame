using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


public class ClickedScf : MonoBehaviour,IPointerDownHandler
{
    private PlayerData PlayerData;
    private CardStore CardStore;
    private CarveManager CarveManager;
    private LibraryManager LibraryManager;
    // Start is called before the first frame update
    void Start()
    {
        PlayerData = GameObject.Find("DataManager").GetComponent<PlayerData>();
        CardStore = GameObject.Find("DataManager").GetComponent<CardStore>();
        CarveManager = GameObject.Find("CarveManager").GetComponent<CarveManager>();
        LibraryManager= GameObject.Find("LibraryManager").GetComponent<LibraryManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnPointerDown(PointerEventData pointerEventData)
    {

        if (CarveManager.select[0])//GS
        {
            CarveManager.ClearGS();
            CarveManager.GScardObject = GameObject.Instantiate(CarveManager.GScardPrefab, CarveManager.GScardPool.transform);
            CarveManager.GScardObject.GetComponent<CardDisplay>().card = this.GetComponent<CardDisplay>().card;
            CarveManager.GScard = this.GetComponent<CardDisplay>().card;
        }
        else if (CarveManager.select[1])//BS
        {
            CarveManager.ClearBS();
            CarveManager.BScardObject = GameObject.Instantiate(CarveManager.BScardPrefab, CarveManager.BScardPool.transform);
            CarveManager.BScardObject.GetComponent<CardDisplay>().card = this.GetComponent<CardDisplay>().card;
            CarveManager.BScard = this.GetComponent<CardDisplay>().card;
        }
        LibraryManager.ClearPool(); 
        LibraryManager.library.SetActive(false);
        if (CarveManager.BScard != null && CarveManager.GScard != null) { CarveManager.Button.SetActive(true); }
    }

}
