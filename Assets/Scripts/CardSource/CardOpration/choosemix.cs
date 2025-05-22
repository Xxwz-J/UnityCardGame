using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class choosemix : MonoBehaviour, IPointerDownHandler
{
    public CardMix cardMix;
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
        if (cardMix != null)
        {
            cardMix.choosing(gameObject);
        }
    }
}
