using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChooseCard : MonoBehaviour, IPointerDownHandler
{
    public Card card;
    public GameObject cardOnFire;
    public WarmCard warmCard;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if(warmCard != null)
        {
            warmCard.choosing(gameObject);
        }
    }
}
